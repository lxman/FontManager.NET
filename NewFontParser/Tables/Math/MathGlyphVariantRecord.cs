using NewFontParser.Reader;

namespace NewFontParser.Tables.Math
{
    public class MathGlyphVariantRecord
    {
        public ushort VariantGlyph { get; }

        public ushort AdvanceMeasurement { get; }

        public MathGlyphVariantRecord(BigEndianReader reader)
        {
            VariantGlyph = reader.ReadUShort();
            AdvanceMeasurement = reader.ReadUShort();
        }
    }
}
