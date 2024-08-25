using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkLigPos
{
    public class ComponentRecord
    {
        public ushort[] LigatureAnchorOffsets { get; }

        public ComponentRecord(byte[] data, ushort markClassCount)
        {
            var reader = new BigEndianReader(data);

            LigatureAnchorOffsets = new ushort[markClassCount];
            for (var i = 0; i < markClassCount; i++)
            {
                LigatureAnchorOffsets[i] = reader.ReadUShort();
            }
        }
    }
}
