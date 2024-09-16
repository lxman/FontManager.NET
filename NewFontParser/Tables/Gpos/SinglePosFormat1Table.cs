using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Gpos
{
    public class SinglePosFormat1Table : ISinglePosFormatTable
    {
        public ushort PosFormat { get; }

        public ICoverageFormat Coverage { get; }

        public ValueFormat ValueFormat { get; }

        public ValueRecord ValueRecord { get; }

        public SinglePosFormat1Table(byte[] data)
        {
            var reader = new BigEndianReader(data);

            PosFormat = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ValueFormat = (ValueFormat)reader.ReadUShort();
            ValueRecord = new ValueRecord(ValueFormat, reader);
            reader.Seek(coverageOffset);
            Coverage = CoverageTable.Retrieve(reader);
        }
    }
}