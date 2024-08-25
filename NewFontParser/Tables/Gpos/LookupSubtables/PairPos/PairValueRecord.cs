using NewFontParser.Reader;

namespace NewFontParser.Tables.Gpos.LookupSubtables.PairPos
{
    public class PairValueRecord
    {
        public ushort SecondGlyph { get; }

        public short Value1 { get; }

        public short Value2 { get; }

        public PairValueRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);

            SecondGlyph = reader.ReadUShort();
            Value1 = reader.ReadShort();
            Value2 = reader.ReadShort();
        }
    }
}
