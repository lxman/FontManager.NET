using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkLigPos
{
    public class ComponentRecord
    {
        public ushort[] LigatureAnchorOffsets { get; }

        public ComponentRecord(BigEndianReader reader, ushort markClassCount)
        {
            LigatureAnchorOffsets = reader.ReadUShortArray(markClassCount);
        }
    }
}