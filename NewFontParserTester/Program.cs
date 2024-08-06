using NewFontParser;

namespace NewFontParserTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fontReader = new FontReader();
            var fontStructure = fontReader.ReadFile(@"C:\Users\jorda\source\repos\Typography\Demo\Windows\TestFonts\Arimo-Regular.ttf");
        }
    }
}
