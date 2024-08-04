using FontParser.Tables;

namespace FontParser.Typeface
{
    /// <summary>
    /// access to some openfont table directly
    /// </summary>
    public static class TypefaceInternalTypeAccessExtensions
    {
        public static Os2Table GetOs2Table(this Typeface typeface) => typeface.OS2Table;

        public static NameEntry GetNameEntry(this Typeface typeface) => typeface.NameEntry;
    }
}