using NewFontParser;

namespace NewFontParserTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> errors = [];
            const string rootDirectory = @"C:\Users\jorda\source\repos\Typography\Demo\Windows\TestFonts";
            List<string> fonts = Directory.GetFiles(rootDirectory).Where(f => f.EndsWith(".ttf") || f.EndsWith(".otf"))
                .ToList();
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
