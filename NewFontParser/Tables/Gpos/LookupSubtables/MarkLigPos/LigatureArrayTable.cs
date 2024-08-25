using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkLigPos
{
    public class LigatureArrayTable
    {
        public ushort LigatureCount { get; }

        public List<LigatureAttachTable> LigatureAttachTables { get; } = new List<LigatureAttachTable>();

        public LigatureArrayTable(byte[] data, ushort markClassCount)
        {
            var reader = new BigEndianReader(data);

            LigatureCount = reader.ReadUShort();
            for (var i = 0; i < LigatureCount; i++)
            {
                LigatureAttachTables.Add(new LigatureAttachTable(reader.ReadBytes(2 + (2 * markClassCount)), markClassCount));
            }
        }
    }
}
