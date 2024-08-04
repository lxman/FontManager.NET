using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables
{
    /// <summary>
    /// base class of lookup sub table
    /// </summary>
    public abstract class LookupSubTable
    {
        public GSUB OwnerGSub;

        public abstract bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len);

        //collect all substitution glyphs
        //
        //if we look up glyph index from the unicode char
        // (e.g. building pre-built glyph texture)
        //we may miss some glyph that is needed for substitution process.
        //
        //so, we collect it here, based on current script lang.
        public abstract void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs);
    }
}