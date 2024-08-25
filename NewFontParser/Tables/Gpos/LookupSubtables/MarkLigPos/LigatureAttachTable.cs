using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkLigPos
{
    public class LigatureAttachTable
    {
        public ushort ComponentCount { get; }

        public List<ComponentRecord> LigatureAnchors { get; } = new List<ComponentRecord>();

        public LigatureAttachTable(byte[] data, ushort markClassCount)
        {
            var reader = new BigEndianReader(data);

            ComponentCount = reader.ReadUShort();
            for (var i = 0; i < ComponentCount; i++)
            {
                LigatureAnchors.Add(new ComponentRecord(reader.ReadBytes(2 * markClassCount), markClassCount));
            }
        }
    }
}
