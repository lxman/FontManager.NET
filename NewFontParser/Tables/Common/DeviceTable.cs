using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class DeviceTable
    {
        public ushort StartSize { get; }
        public ushort EndSize { get; }
        public DeltaFormat DeltaFormat { get; }
        public ushort[] DeltaValues { get; }

        public DeviceTable(BigEndianReader reader)
        {
            StartSize = reader.ReadUShort();
            EndSize = reader.ReadUShort();
            DeltaFormat = (DeltaFormat)reader.ReadUShort();

            int deltaCount = EndSize - StartSize;
            DeltaValues = new ushort[deltaCount];
            for (var i = 0; i < deltaCount; i++)
            {
                DeltaValues[i] = reader.ReadUShort();
            }
        }
    }
}