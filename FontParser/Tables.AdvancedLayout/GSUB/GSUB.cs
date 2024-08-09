using System.Collections.Generic;
using System.IO;
using FontParser.Tables.AdvancedLayout.GSUB.Subtables;
using FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable;

namespace FontParser.Tables.AdvancedLayout.GSUB
{
    //https://docs.microsoft.com/en-us/typography/opentype/spec/gsub

    ////////////////////////////////////////////////////////////////
    //GSUB Table
    //The GSUB table contains substitution lookups that map GIDs to GIDs and associate these mappings with particular OpenType Layout features. The OpenType specification currently supports six different GSUB lookup types:

    //    1. Single        Replaces one glyph with one glyph.
    //    2. Multiple      Replaces one glyph with more than one glyph.
    //    3. Alternate     Replaces one glyph with one of many glyphs.
    //    4. Ligature      Replaces multiple glyphs with one glyph.
    //    5. Context       Replaces one or more glyphs in context.
    //    6. Chaining Context   Replaces one or more glyphs in chained context.

    //Although these lookups are defined by the font developer,
    //it is important for application developers to understand that some features require relatively complex UI support.
    //In particular, OTL features using type 3 lookups may require the application to present options
    //to the user (an example of this is provided in the discussion of OTLS in Part One).
    //In addition, some registered features allow more than one lookup type to be employed,
    //so application developers cannot rely on supporting only some lookup types.
    //Similarly, features may have both GSUB and GPOS solutions—e.g. the 'Case-Sensitive Forms' feature—so applications
    //that want to support these features should avoid limiting their support to only one of these tables.
    //In setting priorities for feature support,
    //it is important to consider the possible interaction of features and to provide users with powerful sets of typographic tools that work together.

    ////////////////////////////////////////////////////////////////

    public class GSUB : GlyphShapingTableEntry
    {
        public const string _N = "GSUB";
        public override string Name => _N;

#if DEBUG

        public GSUB()
        { }

#endif

        //
        protected override void ReadLookupTable(BinaryReader reader, long lookupTablePos,
                                                ushort lookupType, ushort lookupFlags,
                                                ushort[] subTableOffsets, ushort markFilteringSet)
        {
            var lookupTable = new LookupTable(lookupFlags, markFilteringSet);
            LookupSubTable[] subTables = new LookupSubTable[subTableOffsets.Length];
            lookupTable.SubTables = subTables;

            for (var i = 0; i < subTableOffsets.Length; ++i)
            {
                LookupSubTable subTable = LookupTable.ReadSubTable(lookupType, reader, lookupTablePos + subTableOffsets[i]);
                subTable.OwnerGSub = this;
                subTables[i] = subTable;
            }

#if DEBUG
            lookupTable.dbugLkIndex = LookupList.Count;
#endif
            LookupList.Add(lookupTable);
        }

        protected override void ReadFeatureVariations(BinaryReader reader, long featureVariationsBeginAt)
        {
            Utils.WarnUnimplemented("GSUB feature variations");
        }

        private List<LookupTable> _lookupList = new List<LookupTable>();

        public IList<LookupTable> LookupList => _lookupList;
    }
}