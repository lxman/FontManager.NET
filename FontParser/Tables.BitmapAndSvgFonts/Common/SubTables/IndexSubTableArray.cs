namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    public readonly struct IndexSubTableArray
    {
        public readonly ushort firstGlyphIndex;
        public readonly ushort lastGlyphIndex;
        public readonly uint additionalOffsetToIndexSubtable;

        public IndexSubTableArray(ushort firstGlyphIndex, ushort lastGlyphIndex, uint additionalOffsetToIndexSubtable)
        {
            this.firstGlyphIndex = firstGlyphIndex;
            this.lastGlyphIndex = lastGlyphIndex;
            this.additionalOffsetToIndexSubtable = additionalOffsetToIndexSubtable;
        }

#if DEBUG

        public override string ToString()
        {
            return "[" + firstGlyphIndex + "-" + lastGlyphIndex + "]";
        }

#endif
    }
}
