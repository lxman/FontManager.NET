using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Common
{
    public class BitmapSize
    {
        public uint IndexSubtableListOffset { get; }

        public uint IndexSubtableListSize { get; }

        public uint IndexSubtableCount { get; }

        public uint ColorRef { get; }

        public SbitLineMetrics HorizontalMetrics { get; }

        public SbitLineMetrics VerticalMetrics { get; }

        public ushort StartGlyphIndex { get; }

        public ushort EndGlyphIndex { get; }

        public byte PpemX { get; }

        public byte PpemY { get; }

        public BitmapDepth BitDepth { get; }

        public BitmapFlags Flags { get; }

        public BitmapSize(BigEndianReader reader)
        {
            IndexSubtableListOffset = reader.ReadUInt32();
            IndexSubtableListSize = reader.ReadUInt32();
            IndexSubtableCount = reader.ReadUInt32();
            ColorRef = reader.ReadUInt32();
            HorizontalMetrics = new SbitLineMetrics(reader);
            VerticalMetrics = new SbitLineMetrics(reader);
            StartGlyphIndex = reader.ReadUShort();
            EndGlyphIndex = reader.ReadUShort();
            PpemX = reader.ReadByte();
            PpemY = reader.ReadByte();
            BitDepth = (BitmapDepth)reader.ReadByte();
            Flags = (BitmapFlags)reader.ReadByte();
        }
    }
}
