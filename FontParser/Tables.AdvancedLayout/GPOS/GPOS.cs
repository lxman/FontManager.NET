using System.Collections.Generic;
using System.IO;
using FontParser.Tables.AdvancedLayout.GPOS.Subtables;
using FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable;

namespace FontParser.Tables.AdvancedLayout.GPOS
{
    //https://docs.microsoft.com/en-us/typography/opentype/spec/gpos

    public class GPOS : GlyphShapingTableEntry
    {
        public const string _N = "GPOS";
        public override string Name => _N;

        /// <summary>
        /// heuristic lookback optimization,
        /// some layout-context may need=> eg. **Emoji**, some complex script
        /// some layout-context may not need.
        /// </summary>
        public bool EnableLongLookBack { get; set; }

#if DEBUG

        public GPOS()
        { }

#endif

        public static PosRuleSetTable[] CreateMultiplePosRuleSetTables(long initPos, ushort[] offsets, BinaryReader reader)
        {
            int j = offsets.Length;
            PosRuleSetTable[] results = new PosRuleSetTable[j];
            for (int i = 0; i < j; ++i)
            {
                results[i] = PosRuleSetTable.CreateFrom(reader, initPos + offsets[i]);
            }
            return results;
        }

        public static PosLookupRecord[] CreateMultiplePosLookupRecords(BinaryReader reader, int count)
        {
            PosLookupRecord[] results = new PosLookupRecord[count];
            for (int n = 0; n < count; ++n)
            {
                results[n] = PosLookupRecord.CreateFrom(reader);
            }
            return results;
        }

        protected override void ReadLookupTable(BinaryReader reader, long lookupTablePos,
                                                ushort lookupType, ushort lookupFlags,
                                                ushort[] subTableOffsets, ushort markFilteringSet)
        {
            LookupTable lookupTable = new LookupTable(lookupFlags, markFilteringSet);
            var subTables = new LookupSubTable[subTableOffsets.Length];
            lookupTable.SubTables = subTables;

            for (int i = 0; i < subTableOffsets.Length; ++i)
            {
                LookupSubTable subTable = LookupTable.ReadSubTable(lookupType, reader, lookupTablePos + subTableOffsets[i]);
                subTable.OwnerGPos = this;
                subTables[i] = subTable;

                if (lookupType == 9)
                {
                    //temp fix
                    // (eg. Emoji) => enable long look back
                    EnableLongLookBack = true;
                }
            }

#if DEBUG
            lookupTable.dbugLkIndex = LookupList.Count;
#endif

            LookupList.Add(lookupTable);
        }

        protected override void ReadFeatureVariations(BinaryReader reader, long featureVariationsBeginAt)
        {
            Utils.WarnUnimplemented("GPOS feature variations");
        }

        private readonly List<LookupTable> _lookupList = new List<LookupTable>();

        public IList<LookupTable> LookupList => _lookupList;
    }
}