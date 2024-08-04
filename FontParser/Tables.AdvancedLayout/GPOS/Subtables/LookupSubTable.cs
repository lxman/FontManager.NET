using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public abstract class LookupSubTable
    {
        public GPOS OwnerGPos;

        public abstract void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len);
    }
}