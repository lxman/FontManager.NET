namespace FontParser.Tables.AdvancedLayout.FontMath
{
    public readonly struct GlyphVariantRecord
    {
        //    MathGlyphVariantRecord Table
        //Type      Name                Description
        //uint16    VariantGlyph        Glyph ID for the variant.
        //uint16    AdvanceMeasurement  Advance width/height, in design units, of the variant, in the direction of requested glyph extension.
        public readonly ushort VariantGlyph;

        public readonly ushort AdvanceMeasurement;

        public GlyphVariantRecord(ushort variantGlyph, ushort advanceMeasurement)
        {
            VariantGlyph = variantGlyph;
            AdvanceMeasurement = advanceMeasurement;
        }

#if DEBUG

        public override string ToString()
        {
            return "variant_glyph_id:" + VariantGlyph + ", adv:" + AdvanceMeasurement;
        }

#endif
    }
}
