using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Colr
{
    public class BaseGlyphList
    {
        public List<BaseGlyphPaintRecord> BaseGlyphPaintRecords { get; } = new List<BaseGlyphPaintRecord>();

        public BaseGlyphList(BigEndianReader reader)
        {
            ushort baseGlyphCount = reader.ReadUShort();
            for (var i = 0; i < baseGlyphCount; i++)
            {
                BaseGlyphPaintRecords.Add(new BaseGlyphPaintRecord(reader));
            }
        }
    }
}
