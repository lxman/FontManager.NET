using System.Collections.Generic;

namespace FontParser.Tables.CharacterMap.CharMapFormats
{
    /// <summary>
    /// An empty character map that maps all characters to glyph 0
    /// </summary>
    internal class NullCharMap : CharacterMap
    {
        public override ushort Format => 0;

        public override ushort GetGlyphIndex(int character) => 0;

        public override void CollectUnicodeChars(List<uint> unicodes)
        {  /*nothing*/}
    }
}