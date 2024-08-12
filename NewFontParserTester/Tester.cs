using NewFontParser;
using Serilog;

namespace NewFontParserTester
{
    public class Tester
    {
        public void Run()
        {
            List<string> errors = [];
            const string rootDirectory = @"C:\Users\jorda\source\repos\Typography\Demo\Windows\TestFonts";
            List<string> fonts = Directory.GetFiles(rootDirectory).Where(f => f.EndsWith(".ttf") || f.EndsWith(".otf"))
                .ToList();
            Log.Logger.Debug($"Found {fonts.Count} fonts to load.");
            fonts.ForEach(f =>
            {
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
