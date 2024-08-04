using System.Collections.Generic;

namespace FontParser.Tables.CharacterMap
{
    public abstract class CharacterMap
    {
        //https://www.microsoft.com/typography/otspec/cmap.htm
        public abstract ushort Format { get; }

        public ushort PlatformId { get; set; }
        public ushort EncodingId { get; set; }

        public ushort CharacterToGlyphIndex(int codepoint)
        {
            return GetGlyphIndex(codepoint);
        }

        public abstract ushort GetGlyphIndex(int codepoint);

        public abstract void CollectUnicodeChars(List<uint> unicodes);

        public override string ToString()
        {
            return $"fmt:{Format}, plat:{PlatformId}, enc:{EncodingId}";
        }
    }
}