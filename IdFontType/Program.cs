using NewFontParser;
using Serilog;

namespace IdFontType
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> errors = [];
            List<string> fonts = [];
            const string rootDirectory = @"C:\Users\jorda\source\TestFonts";
            fonts.AddRange(Directory.GetFiles(rootDirectory).Where(f => f.EndsWith(".ttf") || f.EndsWith(".otf")));
            fonts.AddRange(Directory.GetFiles(rootDirectory).Where(f => f.EndsWith(".ttc")).ToList());
            //fonts = fonts.Where(f => f.EndsWith("NotoSans-Bold.ttf")).ToList();
            Log.Debug($"Found {fonts.Count} fonts to load.");
            fonts.ForEach(f =>
            {
                Console.WriteLine($"Processing {f.Split("\\").Last()}");
                try
                {
                    var fontReader = new FontReader();
                    List<(string, List<string>)> fontStructure = fontReader.GetTableNames(f);
                    fontStructure.ForEach(s =>
                    {
                        Console.WriteLine(s.Item1);
                        s.Item2.ForEach(c => Console.WriteLine($"\t{c}"));
                    });
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
