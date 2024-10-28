using FontParser;
using FontParser.RenderFont;
using Serilog;

namespace FontParserTester
{
    public class Tester
    {
        public void Run()
        {
            List<string> errors = [];
            List<string> fonts = [];
            const string rootDirectory = @"C:\Users\jorda\source\TestFonts";
            const string cff2Directory = @"C:\Users\jorda\source\Cff2Fonts";
            fonts.AddRange(Directory.GetFiles(rootDirectory).Where(f => f.ToLower().EndsWith(".ttf") || f.ToLower().EndsWith(".otf")));
            fonts.AddRange(Directory.GetFiles(rootDirectory).Where(f => f.ToLower().EndsWith(".ttc")).ToList());
            fonts.AddRange(Directory.GetFiles(cff2Directory).Where(f => f.ToLower().EndsWith(".ttf") || f.ToLower().EndsWith(".otf")));
            const string woffDirectory = @"C:\Users\jorda\source\WoffFonts";
            const string woff2Directory = @"C:\Users\jorda\source\Woff2Fonts";
            fonts.AddRange(Directory.GetFiles(woffDirectory).Where(f => f.ToLower().EndsWith(".woff")));
            fonts.AddRange(Directory.GetFiles(woff2Directory).Where(f => f.ToLower().EndsWith(".woff2")));
            //fonts = fonts.Where(f => f.EndsWith("FDArrayTest257.otf")).ToList();
            Log.Debug($"Found {fonts.Count} fonts to load.");
            fonts.ForEach(f =>
            {
                Console.WriteLine($"Processing {f.Split("\\").Last()}");
                try
                {
                    var fontReader = new FontReader();
                    List<FontStructure> fontStructure = fontReader.ReadFile(f);
                    //if (fontStructure.Count > 1)
                    //{
                    //    Console.WriteLine("Multiple fonts found in file.");
                    //}
                    CharacterMapper mapper = fontStructure[0].GetCharacterMapper();
                    for (ushort x = 0; x < 0x100; x++)
                    {
                        ushort glyphId = mapper.GetGlyphId(x);
                        List<string> commands = fontStructure[0].GetGlyph(glyphId);
                    }
                    //Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    errors.Add(f);
                }
            });
            if (errors.Count > 0)
            {
                Console.WriteLine("The following files failed to parse:");
                errors.ForEach(Console.WriteLine);
            }
            else
            {
                Console.WriteLine("All parsed successfully!");
            }
        }
    }
}