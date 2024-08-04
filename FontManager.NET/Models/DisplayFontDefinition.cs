using System.Windows.Media;

namespace FontManager.NET.Models
{
    public class DisplayFontDefinition(
        Typeface typeface,
        double size)
    {
        public Typeface Typeface { get; set; } = typeface;

        public double Size { get; set; } = size;
    }
}