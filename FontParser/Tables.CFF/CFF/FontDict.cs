using System.Collections.Generic;

namespace FontParser.Tables.CFF.CFF
{
    public class FontDict
    {
        public int FontName;
        public int PrivateDicSize;
        public int PrivateDicOffset;
        public List<byte[]> LocalSubr;

        public FontDict(int dictSize, int dictOffset)
        {
            PrivateDicSize = dictSize;
            PrivateDicOffset = dictOffset;
        }
    }
}
