using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    /// <summary>
    /// Empty lookup sub table for unimplemented formats
    /// </summary>
    public class UnImplementedLookupSubTable : LookupSubTable
    {
        private readonly string _message;

        public UnImplementedLookupSubTable(string msg)
        {
            _message = msg;
            Utils.WarnUnimplemented(msg);
        }

        public override string ToString() => _message;

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            return false;
        }

        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            Utils.WarnUnimplemented("collect-assoc-sub-glyph: " + ToString());
        }
    }
}