namespace FontParser.Tables.AdvancedLayout.FontMath
{
    internal class ItalicsCorrectionInfoTable
    {
        //MathItalicsCorrectionInfo Table
        //The MathItalicsCorrectionInfo table contains italics correction values for slanted glyphs used in math typesetting.The table consists of the following parts:

        //    Coverage of glyphs for which the italics correction values are provided.It is assumed to be zero for all other glyphs.
        //    Count of covered glyphs.
        //    Array of italic correction values for each covered glyph, in order of coverage.The italics correction is the measurement of how slanted the glyph is, and how much its top part protrudes to the right. For example, taller letters tend to have larger italics correction, and a V will probably have larger italics correction than an L.

        //Italics correction can be used in the following situations:

        //    When a run of slanted characters is followed by a straight character (such as an operator or a delimiter), the italics correction of the last glyph is added to its advance width.
        //    When positioning limits on an N-ary operator (e.g., integral sign), the horizontal position of the upper limit is moved to the right by ½ of the italics correction, while the position of the lower limit is moved to the left by the same distance.
        //    When positioning superscripts and subscripts, their default horizontal positions are also different by the amount of the italics correction of the preceding glyph.

        public ValueRecord[] ItalicCorrections;
        public CoverageTable.CoverageTable CoverageTable;
    }
}
