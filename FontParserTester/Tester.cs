using FontParser;
using FontParser.RenderFont;
using FontParser.RenderFont.Interpreter;
using FontParser.Tables;
using FontParser.Tables.Hmtx;
using FontParser.Tables.TtTables;
using FontParser.Tables.TtTables.Glyf;
using Serilog;

namespace FontParserTester
{
    public class Tester
    {
        public void Run()
        {
            List<string> errors = [];
            List<string> fonts = [];
            const string windowsFontsDirectory = @"C:\Windows\Fonts";
            
            // Test Arial and Verdana - both have extensive TrueType hinting
            fonts.Add(Path.Combine(windowsFontsDirectory, "arial.ttf"));
            fonts.Add(Path.Combine(windowsFontsDirectory, "verdana.ttf"));
            
            Log.Debug($"Testing {fonts.Count} fonts with hinting interpreter.");
            Console.WriteLine($"\n{'=',70}");
            Console.WriteLine("  TrueType Hinting Interpreter Test");
            Console.WriteLine($"{'=',70}\n");
            
            fonts.ForEach(f =>
            {
                Console.WriteLine($"{'═',70}");
                Console.WriteLine($"  Font: {Path.GetFileName(f)}");
                Console.WriteLine($"{'═',70}");
                
                try
                {
                    var fontReader = new FontReader();
                    List<FontStructure> fontStructures = fontReader.ReadFile(f);
                    
                    if (fontStructures.Count == 0)
                    {
                        Console.WriteLine("  ✗ ERROR: No font structures found\n");
                        errors.Add(f);
                        return;
                    }
                    
                    FontStructure fontStructure = fontStructures[0];
                    Console.WriteLine("  ✓ Font loaded successfully");
                    
                    // Test key glyphs that should have hinting instructions
                    // These are common characters that demonstrate various hinting features
                    char[] testChars = ['A', 'H', 'a', 'n', 'o', 'e', 'l', 'i'];
                    var glyphsTested = 0;
                    var glyphsWithInstructions = 0;
                    var totalInstructionBytes = 0;
                    var interpreterExecutions = 0;
                    var interpreterErrors = 0;
                    
                    CharacterMapper mapper = fontStructure.GetCharacterMapper();
                    GlyphTable? glyphTable = fontStructure.GetGlyphTable();
                    IFontTable cvtTable = fontStructure.Tables.First(t => t is CvtTable);
                    IFontTable maxPTable = fontStructure.Tables.First(t => t is MaxPTable);
                    IFontTable hmtxTable = fontStructure.Tables.First(t => t is HmtxTable);
                    
                    if (glyphTable is null)
                    {
                        Console.WriteLine("  ✗ ERROR: No glyph table found\n");
                        errors.Add(f);
                        return;
                    }
                    
                    Console.WriteLine("\n  Analyzing glyph hinting instructions:");
                    Console.WriteLine("  " + new string('─', 66));
                    
                    foreach (char testChar in testChars)
                    {
                        ushort glyphId = mapper.GetGlyphId(testChar);
                        GlyphData? glyphData = glyphTable.GetGlyphData(glyphId);
                        
                        if (glyphData?.GlyphSpec is not SimpleGlyph simpleGlyph)
                        {
                            Console.WriteLine($"    '{testChar}' (ID {glyphId}): No simple glyph data");
                            continue;
                        }
                        
                        glyphsTested++;
                        int instrCount = simpleGlyph.Instructions.Count;
                        
                        if (instrCount > 0)
                        {
                            glyphsWithInstructions++;
                            totalInstructionBytes += instrCount;
                            
                            // Show the first few instructions in hex
                            string preview = string.Join(" ", 
                                simpleGlyph.Instructions
                                    .Take(Math.Min(8, instrCount))
                                    .Select(b => $"{b:X2}"));
                            
                            if (instrCount > 8) preview += "...";
                            
                            Console.WriteLine($"    '{testChar}' (ID {glyphId,3}): {instrCount,3} bytes  [{preview}]");
                        }
                        else
                        {
                            Console.WriteLine($"    '{testChar}' (ID {glyphId,3}):   0 bytes  [no hinting]");
                        }
                    }
                    
                    // Summary
                    Console.WriteLine("  " + new string('─', 66));
                    Console.WriteLine($"\n  Instruction Analysis:");
                    Console.WriteLine($"    Glyphs tested:          {glyphsTested}");
                    Console.WriteLine($"    Glyphs with hinting:    {glyphsWithInstructions}");
                    Console.WriteLine($"    Total instruction bytes: {totalInstructionBytes}");
                    Console.WriteLine($"    Average bytes/glyph:    {(glyphsWithInstructions > 0 ? totalInstructionBytes / glyphsWithInstructions : 0)}");
                    
                    if (glyphsWithInstructions == 0)
                    {
                        Console.WriteLine($"\n  ✗ FAIL - No hinting instructions found");
                        errors.Add(f);
                        Console.WriteLine();
                        return;
                    }
                    
                    // Execute the interpreter on each glyph
                    Console.WriteLine($"\n  Executing Interpreter:");
                    Console.WriteLine("  " + new string('─', 66));
                    
                    foreach (char testChar in testChars)
                    {
                        ushort glyphId = mapper.GetGlyphId(testChar);
                        GlyphData? glyphData = glyphTable.GetGlyphData(glyphId);
                        
                        if (glyphData?.GlyphSpec is not SimpleGlyph simpleGlyph)
                        {
                            continue;
                        }
                        
                        if (simpleGlyph.Instructions.Count == 0)
                        {
                            continue;
                        }
                        
                        try
                        {
                            // Create the interpreter with required tables
                            var interpreter = new Interpreter(
                                glyphTable,
                                glyphData,
                                cvtTable as CvtTable,
                                hmtxTable as HmtxTable,
                                maxPTable as MaxPTable,
                                new GraphicsState(),
                                new Dictionary<int, byte[]>(),
                                simpleGlyph.Instructions.ToList()
                            );
                            
                            // Execute the interpreter
                            interpreter.Execute();
                            
                            interpreterExecutions++;
                            Console.WriteLine($"    '{testChar}' (ID {glyphId,3}): ✓ Execution successful");
                        }
                        catch (NotImplementedException nie)
                        {
                            interpreterErrors++;
                            Console.WriteLine($"    '{testChar}' (ID {glyphId,3}): ⚠ Not implemented: {nie.Message}");
                            Log.Warning($"Glyph '{testChar}' (ID {glyphId}) - Not implemented: {nie.Message}");
                        }
                        catch (Exception ex)
                        {
                            interpreterErrors++;
                            Console.WriteLine($"    '{testChar}' (ID {glyphId,3}): ✗ Error: {ex.Message}");
                            Log.Error(ex, $"Glyph '{testChar}' (ID {glyphId}) execution failed");
                        }
                    }
                    
                    // Interpreter execution summary
                    Console.WriteLine("  " + new string('─', 66));
                    Console.WriteLine($"\n  Interpreter Results:");
                    Console.WriteLine($"    Executions attempted:   {interpreterExecutions + interpreterErrors}");
                    Console.WriteLine($"    Successful:             {interpreterExecutions}");
                    Console.WriteLine($"    Errors/Not Impl:        {interpreterErrors}");
                    
                    if (interpreterExecutions > 0)
                    {
                        double successRate = (double)interpreterExecutions / (interpreterExecutions + interpreterErrors) * 100;
                        Console.WriteLine($"    Success rate:           {successRate:F1}%");
                        
                        if (interpreterErrors == 0)
                        {
                            Console.WriteLine($"\n  ✓ PASS - All glyphs executed successfully!");
                        }
                        else
                        {
                            Console.WriteLine($"\n  ⚠ PARTIAL - {interpreterExecutions}/{interpreterExecutions + interpreterErrors} glyphs executed");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"\n  ✗ FAIL - No successful interpreter executions");
                        errors.Add(f);
                    }
                    
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"  ✗ ERROR: {e.Message}");
                    Log.Error(e, "Failed to process font");
                    errors.Add(f);
                    Console.WriteLine();
                }
            });
            
            // Final summary
            Console.WriteLine($"{'═',70}");
            Console.WriteLine("  Final Summary");
            Console.WriteLine($"{'═',70}");
            
            if (errors.Count > 0)
            {
                Console.WriteLine($"\n  ✗ FAILED: {errors.Count}/{fonts.Count} font(s) failed:");
                errors.ForEach(e => Console.WriteLine($"    - {Path.GetFileName(e)}"));
            }
            else
            {
                Console.WriteLine($"\n  ✓ SUCCESS: All {fonts.Count} fonts processed successfully!");
                Console.WriteLine("    Both Arial and Verdana executed with the TrueType interpreter.");
            }
            
            Console.WriteLine($"\n{'═',70}\n");
        }
    }
}
