using System;
using System.Collections.Generic;

namespace FontParser.Tables.CharacterMap.CharMapFormats
{
    internal class CharMapFormat4 : CharacterMap
    {
        public override ushort Format => 4;

        internal readonly ushort[] _startCode; //Starting character code for each segment
        internal readonly ushort[] _endCode;//Ending character code for each segment, last = 0xFFFF.
        internal readonly ushort[] _idDelta; //Delta for all character codes in segment
        internal readonly ushort[] _idRangeOffset; //Offset in bytes to glyph indexArray, or 0 (not offset in bytes unit)
        internal readonly ushort[] _glyphIdArray;

        public CharMapFormat4(ushort[] startCode, ushort[] endCode, ushort[] idDelta, ushort[] idRangeOffset, ushort[] glyphIdArray)
        {
            _startCode = startCode;
            _endCode = endCode;
            _idDelta = idDelta;
            _idRangeOffset = idRangeOffset;
            _glyphIdArray = glyphIdArray;
        }

        public override ushort GetGlyphIndex(int codepoint)
        {
            // This lookup table only supports 16-bit codepoints
            if (codepoint > ushort.MaxValue)
            {
                return 0;
            }

            // https://www.microsoft.com/typography/otspec/cmap.htm#format4
            // "You search for the first endCode that is greater than or equal to the character code you want to map"
            // "The segments are sorted in order of increasing endCode values"
            // -> binary search is valid here
            int i = Array.BinarySearch(_endCode, (ushort)codepoint);
            i = i < 0 ? ~i : i;

            // https://www.microsoft.com/typography/otspec/cmap.htm#format4
            // "If the corresponding startCode is [not] less than or equal to the character code,
            // then [...] the missingGlyph is returned"
            // Index i should never be out of range, because the list ends with a
            // 0xFFFF value. However, we also use this charmap for format 0, which
            // does not have that final endcode, so there is a chance to overflow.
            if (i >= _endCode.Length || _startCode[i] > codepoint)
            {
                return 0;
            }

            if (_idRangeOffset[i] == 0)
            {
                //TODO: review 65536 => use bitflags
                return (ushort)((codepoint + _idDelta[i]) % 65536);
            }
            //If the idRangeOffset value for the segment is not 0,
            //the mapping of character codes relies on glyphIdArray.
            //The character code offset from startCode is added to the idRangeOffset value.
            //This sum is used as an offset from the current location within idRangeOffset itself to index out the correct glyphIdArray value.
            //This obscure indexing trick works because glyphIdArray immediately follows idRangeOffset in the font file.
            //The C expression that yields the glyph index is:

            //*(idRangeOffset[i]/2
            //+ (c - startCount[i])
            //+ &idRangeOffset[i])

            int offset = _idRangeOffset[i] / 2 + (codepoint - _startCode[i]);
            // I want to thank Microsoft for this clever pointer trick
            // TODO: What if the value fetched is inside the _idRangeOffset table?
            // TODO: e.g. (offset - _idRangeOffset.Length + i < 0)
            return _glyphIdArray[offset - _idRangeOffset.Length + i];
        }

        public override void CollectUnicodeChars(List<uint> unicodes)
        {
            for (var i = 0; i < _startCode.Length; ++i)
            {
                uint start = _startCode[i];
                uint stop = _endCode[i];
                for (uint u = start; u <= stop; ++u)
                {
                    unicodes.Add(u);
                }
            }
        }
    }
}