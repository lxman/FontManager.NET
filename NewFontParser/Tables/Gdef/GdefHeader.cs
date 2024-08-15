using NewFontParser.Reader;

namespace NewFontParser.Tables.Gdef
{
    public class GdefHeader : IInfoTable
    {
        public ushort MajorVersion { get; }

        public ushort MinorVersion { get; }

        public ushort? GlyphClassDefOffset { get; }

        public ushort? AttachListOffset { get; }

        public ushort? LigCaretListOffset { get; }

        public ushort? MarkAttachClassDefOffset { get; }

        public ushort? MarkGlyphSetsDefOffset { get; }

        public ushort? ItemVarStoreOffset { get; }

        public GdefHeader(byte[] data)
        {
            var reader = new BigEndianReader(data);
            MajorVersion = reader.ReadUShort();
            MinorVersion = reader.ReadUShort();
            GlyphClassDefOffset = reader.ReadUShort();
            AttachListOffset = reader.ReadUShort();
            LigCaretListOffset = reader.ReadUShort();
            MarkAttachClassDefOffset = reader.ReadUShort();
            if (MinorVersion > 1)
            {
                MarkGlyphSetsDefOffset = reader.ReadUShort();
            }
            if (MinorVersion > 2)
            {
                ItemVarStoreOffset = reader.ReadUShort();
            }
        }
    }
}
