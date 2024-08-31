using NewFontParser.Reader;

namespace NewFontParser.Tables.Hhea
{
    public class HheaTable : IInfoTable
    {
        public static string Tag => "hhea";

        public ushort MajorVersion { get; set; }

        public ushort MinorVersion { get; set; }

        public string Version => $"{MajorVersion}.{MinorVersion}";

        public short Ascender { get; set; }

        public short Descender { get; set; }

        public short LineGap { get; set; }

        public ushort AdvanceWidthMax { get; set; }

        public short MinLeftSideBearing { get; set; }

        public short MinRightSideBearing { get; set; }

        public short XMaxExtent { get; set; }

        public short CaretSlopeRise { get; set; }

        public short CaretSlopeRun { get; set; }

        public short CaretOffset { get; set; }

        public short Reserved1 { get; set; }

        public short Reserved2 { get; set; }

        public short Reserved3 { get; set; }

        public short Reserved4 { get; set; }

        public short MetricDataFormat { get; set; }

        public ushort NumberOfHMetrics { get; set; }

        public HheaTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            MajorVersion = reader.ReadUShort();
            MinorVersion = reader.ReadUShort();
            Ascender = reader.ReadShort();
            Descender = reader.ReadShort();
            LineGap = reader.ReadShort();
            AdvanceWidthMax = reader.ReadUShort();
            MinLeftSideBearing = reader.ReadShort();
            MinRightSideBearing = reader.ReadShort();
            XMaxExtent = reader.ReadShort();
            CaretSlopeRise = reader.ReadShort();
            CaretSlopeRun = reader.ReadShort();
            CaretOffset = reader.ReadShort();
            Reserved1 = reader.ReadShort();
            Reserved2 = reader.ReadShort();
            Reserved3 = reader.ReadShort();
            Reserved4 = reader.ReadShort();
            MetricDataFormat = reader.ReadShort();
            NumberOfHMetrics = reader.ReadUShort();
        }
    }
}