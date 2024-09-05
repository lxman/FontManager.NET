using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.GlyphClassDef
{
    public class ClassRangeRecord
    {
        public ushort StartGlyphId { get; }

        public ushort EndGlyphId { get; }

        public GlyphClassType GlyphClass { get; }

        public ClassRangeRecord(BigEndianReader reader)
        {
            StartGlyphId = reader.ReadUShort();
            EndGlyphId = reader.ReadUShort();
            GlyphClass = (GlyphClassType)reader.ReadUShort();
        }
    }
}