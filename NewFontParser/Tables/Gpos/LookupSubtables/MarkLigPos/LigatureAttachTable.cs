using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkLigPos
{
    public class LigatureAttachTable
    {
        public List<ComponentRecord> LigatureAnchors { get; } = new List<ComponentRecord>();

        public LigatureAttachTable(BigEndianReader reader, ushort markClassCount)
        {
            ushort componentCount = reader.ReadUShort();
            for (var i = 0; i < componentCount; i++)
            {
                LigatureAnchors.Add(new ComponentRecord(reader, markClassCount));
            }
        }
    }
}