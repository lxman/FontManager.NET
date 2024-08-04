using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    /// <summary>
    /// Subtable for unhandled/unimplemented features
    /// </summary>
    public class UnImplementedLookupSubTable : LookupSubTable
    {
        private readonly string _msg;

        public UnImplementedLookupSubTable(string message)
        {
            _msg = message;
            Utils.WarnUnimplemented(message);
        }

        public override string ToString() => _msg;

        public override void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        { }
    }
}