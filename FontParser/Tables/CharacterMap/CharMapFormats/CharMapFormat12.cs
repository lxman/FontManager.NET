using System;
using System.Collections.Generic;

namespace FontParser.Tables.CharacterMap.CharMapFormats
{
    internal class CharMapFormat12 : CharacterMap
    {
        public override ushort Format => 12;

        private readonly uint[] _startCharCodes;
        private readonly uint[] _endCharCodes;
        private readonly uint[] _startGlyphIds;

        internal CharMapFormat12(uint[] startCharCodes, uint[] endCharCodes, uint[] startGlyphIds)
        {
            _startCharCodes = startCharCodes;
            _endCharCodes = endCharCodes;
            _startGlyphIds = startGlyphIds;
        }

        public override ushort GetGlyphIndex(int codepoint)
        {
            // https://www.microsoft.com/typography/otspec/cmap.htm#format12
            // "Groups must be sorted by increasing startCharCode."
            // -> binary search is valid here
            int i = Array.BinarySearch(_startCharCodes, (uint)codepoint);
            i = i < 0 ? ~i - 1 : i;

            if (i >= 0 && codepoint <= _endCharCodes[i])
            {
                return (ushort)(_startGlyphIds[i] + codepoint - _startCharCodes[i]);
            }
            return 0;
        }

        public override void CollectUnicodeChars(List<uint> unicodes)
        {
            for (int i = 0; i < _startCharCodes.Length; ++i)
            {
                uint start = _startCharCodes[i];
                uint stop = _endCharCodes[i];
                for (uint u = start; u <= stop; ++u)
                {
                    unicodes.Add(u);
                }
            }
        }
    }
}