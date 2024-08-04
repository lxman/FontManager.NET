using System.IO;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class SubstLookupRecord
    {
        public readonly ushort sequenceIndex;
        public readonly ushort lookupListIndex;

        public SubstLookupRecord(ushort seqIndex, ushort lookupListIndex)
        {
            sequenceIndex = seqIndex;
            this.lookupListIndex = lookupListIndex;
        }

        public static SubstLookupRecord[] CreateSubstLookupRecords(BinaryReader reader, ushort ncount)
        {
            SubstLookupRecord[] results = new SubstLookupRecord[ncount];
            for (int i = 0; i < ncount; ++i)
            {
                results[i] = new SubstLookupRecord(reader.ReadUInt16(), reader.ReadUInt16());
            }
            return results;
        }
    }
}