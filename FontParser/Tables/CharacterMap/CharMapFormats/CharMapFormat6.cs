using System.Collections.Generic;

namespace FontParser.Tables.CharacterMap.CharMapFormats
{
    internal class CharMapFormat6 : CharacterMap
    {
        public override ushort Format => 6;

        internal CharMapFormat6(ushort startCode, ushort[] glyphIdArray)
        {
            _glyphIdArray = glyphIdArray;
            _startCode = startCode;
        }

        public override ushort GetGlyphIndex(int codepoint)
        {
            // The firstCode and entryCount values specify a subrange (beginning at firstCode,
            // length = entryCount) within the range of possible character codes.
            // Codes outside of this subrange are mapped to glyph index 0.
            // The offset of the code (from the first code) within this subrange is used as
            // index to the glyphIdArray, which provides the glyph index value.
            int i = codepoint - _startCode;
            return i >= 0 && i < _glyphIdArray.Length ? _glyphIdArray[i] : (ushort)0;
        }

        internal readonly ushort _startCode;
        internal readonly ushort[] _glyphIdArray;

        public override void CollectUnicodeChars(List<uint> unicodes)
        {
            ushort u = _startCode;
            for (uint i = 0; i < _glyphIdArray.Length; ++i)
            {
                unicodes.Add(u + i);
            }
        }
    }
}