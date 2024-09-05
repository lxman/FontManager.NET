using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkLigPos
{
    public class LigatureAttachTable
    {
        public ushort ComponentCount { get; }

        public List<ComponentRecord> LigatureAnchors { get; } = new List<ComponentRecord>();

        public LigatureAttachTable(BigEndianReader reader, ushort markClassCount)
        {
            ComponentCount = reader.ReadUShort();
            for (var i = 0; i < ComponentCount; i++)
            {
                LigatureAnchors.Add(new ComponentRecord(reader, markClassCount));
            }
        }
    }
}