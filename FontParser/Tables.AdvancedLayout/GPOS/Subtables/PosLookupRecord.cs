using System.IO;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public readonly struct PosLookupRecord
    {
        //PosLookupRecord
        //Value 	Type 	Description
        //USHORT 	SequenceIndex 	Index to input glyph sequence-first glyph = 0
        //USHORT 	LookupListIndex 	Lookup to apply to that position-zero-based

        public readonly ushort seqIndex;
        public readonly ushort lookupListIndex;

        public PosLookupRecord(ushort seqIndex, ushort lookupListIndex)
        {
            this.seqIndex = seqIndex;
            this.lookupListIndex = lookupListIndex;
        }

        public static PosLookupRecord CreateFrom(BinaryReader reader)
        {
            return new PosLookupRecord(reader.ReadUInt16(), reader.ReadUInt16());
        }
    }
}