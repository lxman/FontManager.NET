using System;
using System.Collections.Generic;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    /// <summary>
    /// 5.3 Context Substitution Format 3: Coverage-based Glyph Contexts
    /// </summary>
    public class LkSubTableT5Fmt3 : LookupSubTable
    {
        public override void CollectAssociatedSubstitutionGlyphs(List<ushort> outputAssocGlyphs)
        {
            throw new NotImplementedException();
        }

        public override bool DoSubstitutionAt(IGlyphIndexList glyphIndices, int pos, int len)
        {
            throw new NotImplementedException();
        }
    }
}