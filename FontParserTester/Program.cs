using FontParser;

namespace FontParserTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var stream = new MemoryStream(File.ReadAllBytes(@"C:\Users\jorda\source\repos\Typography\Demo\Windows\TestFonts\NotoSansDevanagari-Regular.ttf"));
            var reader = new OpenFontReader();
            var typeFace = reader.Read(stream);
        }
    }
}
