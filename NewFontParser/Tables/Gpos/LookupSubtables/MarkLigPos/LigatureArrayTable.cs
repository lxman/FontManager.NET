using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkLigPos
{
    public class LigatureArrayTable
    {
        public ushort LigatureCount { get; }

        public List<LigatureAttachTable> LigatureAttachTables { get; } = new List<LigatureAttachTable>();

        public LigatureArrayTable(BigEndianReader reader, ushort markClassCount)
        {
            long position = reader.Position;

            LigatureCount = reader.ReadUShort();
            ushort[] ligatureAttachOffsets = reader.ReadUShortArray(LigatureCount);
            for (var i = 0; i < ligatureAttachOffsets.Length; i++)
            {
                reader.Seek(position + ligatureAttachOffsets[i]);
                LigatureAttachTables.Add(new LigatureAttachTable(reader, markClassCount));
            }
        }
    }
}