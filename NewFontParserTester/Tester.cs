using NewFontParser;
using Serilog;

namespace NewFontParserTester
{
    public class Tester
    {
        public void Run()
        {
            List<string> errors = [];
            List<string> fonts = [];
            const string rootDirectory1 = "TestFonts";
            const string rootDirectory2 = @"C:\Users\jorda\source\repos\Typography\Demo\Windows\TestFonts";
            fonts.AddRange(Directory.GetFiles(rootDirectory1).Where(f => f.EndsWith(".ttf") || f.EndsWith(".otf")).ToList());
            fonts.AddRange(Directory.GetFiles(rootDirectory2).Where(f => f.EndsWith(".ttf") || f.EndsWith(".otf")).ToList());
            Log.Debug($"Found {fonts.Count} fonts to load.");
            fonts.ForEach(f =>
            {
                Console.WriteLine($"Processing {f.Split("\\").Last()}");
                try
                {
                    var fontReader = new FontReader();
                    FontStructure fontStructure = fontReader.ReadFile(f);
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
