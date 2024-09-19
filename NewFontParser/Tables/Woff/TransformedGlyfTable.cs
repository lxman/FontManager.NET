using NewFontParser.Reader;

namespace NewFontParser.Tables.Woff
{
    public class TransformedGlyfTable
    {
        public ushort Reserved { get; }

        public ushort OptionFlags { get; }

        public ushort GlyphCount { get; }

        public ushort IndexFormat { get; }

        public uint NContourStreamSize { get; }

        public uint NPointsStreamSize { get; }

        public uint FlagStreamSize { get; }

        public uint GlyphStreamSize { get; }

        public uint CompositeStreamSize { get; }

        public uint BBoxStreamSize { get; }

        public uint InstructionStreamSize { get; }

        public ushort[] NContourStream { get; }

        public ushort[] NPointsStream { get; }

        public byte[] FlagStream { get; }

        public byte[] GlyphStream { get; }

        public byte[] CompositeStream { get; }

        public short[] BBoxStream { get; }

        public byte[] InstructionStream { get; }

        public byte[] OverlapSimpleBitmap { get; }

        public TransformedGlyfTable(BigEndianReader reader)
        {
            Reserved = reader.ReadUShort();
            OptionFlags = reader.ReadUShort();
            GlyphCount = reader.ReadUShort();
            IndexFormat = reader.ReadUShort();
            NContourStreamSize = reader.ReadUInt32();
            NPointsStreamSize = reader.ReadUInt32();
            FlagStreamSize = reader.ReadUInt32();
            GlyphStreamSize = reader.ReadUInt32();
            CompositeStreamSize = reader.ReadUInt32();
            BBoxStreamSize = reader.ReadUInt32();
            InstructionStreamSize = reader.ReadUInt32();
            NContourStream = reader.ReadUShortArray(NContourStreamSize);
            NPointsStream = reader.ReadUShortArray(NPointsStreamSize);
            FlagStream = reader.ReadBytes(FlagStreamSize);
            GlyphStream = reader.ReadBytes(GlyphStreamSize);
            CompositeStream = reader.ReadBytes(CompositeStreamSize);
            BBoxStream = reader.ReadShortArray(BBoxStreamSize);
            InstructionStream = reader.ReadBytes(InstructionStreamSize);
            OverlapSimpleBitmap = reader.ReadBytes(GlyphCount);
        }
    }
}