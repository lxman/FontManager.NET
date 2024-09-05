using NewFontParser.Extensions;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos
{
    public class SinglePosFormat1Table : ISinglePosFormatTable
    {
        public ushort PosFormat { get; }

        public ushort CoverageOffset { get; }

        public ValueFormat ValueFormat { get; }

        public ValueRecord ValueRecord { get; }

        public SinglePosFormat1Table(byte[] data)
        {
            var reader = new BigEndianReader(data);

            PosFormat = reader.ReadUShort();
            CoverageOffset = reader.ReadUShort();
            ValueFormat = (ValueFormat)reader.ReadUShort();
            ValueRecord = new ValueRecord(ValueFormat.GetFlags(), reader);
        }
    }
}