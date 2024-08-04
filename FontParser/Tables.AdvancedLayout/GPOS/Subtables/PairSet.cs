namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public readonly struct PairSet
    {
        public readonly ushort secondGlyph;//GlyphID of second glyph in the pair-first glyph is listed in the Coverage table
        public readonly ValueRecord value1;//Positioning data for the first glyph in the pair
        public readonly ValueRecord value2;//Positioning data for the second glyph in the pair

        public PairSet(ushort secondGlyph, ValueRecord v1, ValueRecord v2)
        {
            this.secondGlyph = secondGlyph;
            value1 = v1;
            value2 = v2;
        }

#if DEBUG

        public override string ToString()
        {
            return "second_glyph:" + secondGlyph;
        }

#endif
    }
}