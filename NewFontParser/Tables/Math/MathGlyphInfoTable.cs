using NewFontParser.Reader;
using NewFontParser.Tables.CoverageFormat;

namespace NewFontParser.Tables.Math
{
    public class MathGlyphInfoTable
    {
        public MathItalicsCorrectionInfo ItalicsCorrectionInfo { get; }

        public MathTopAccentAttachment TopAccentAttachment { get; }

        public ICoverageFormat ExtendedShapeCoverage { get; }

        public MathKernInfoTable KernInfo { get; }

        public MathGlyphInfoTable(BigEndianReader reader)
        {
            var position = reader.Position;

            ushort italicsCorrectionInfoOffset = reader.ReadUShort();
            ushort topAccentAttachmentOffset = reader.ReadUShort();
            ushort extendedShapeCoverageOffset = reader.ReadUShort();
            ushort kernInfoOffset = reader.ReadUShort();

            reader.Seek(position + italicsCorrectionInfoOffset);
            ItalicsCorrectionInfo = new MathItalicsCorrectionInfo(reader);

            reader.Seek(position + topAccentAttachmentOffset);
            TopAccentAttachment = new MathTopAccentAttachment(reader);

            reader.Seek(position + extendedShapeCoverageOffset);
            ExtendedShapeCoverage = new Format1(reader);

            reader.Seek(position + kernInfoOffset);
            KernInfo = new MathKernInfoTable(reader);
        }
    }
}
