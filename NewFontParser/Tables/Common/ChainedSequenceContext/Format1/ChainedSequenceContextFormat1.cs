using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format1
{
    public class ChainedSequenceContextFormat1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ChainedSequenceRuleSet[] ChainedSequenceRuleSets { get; }

        public ChainedSequenceContextFormat1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            Format = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ushort chainedSequenceRuleSetCount = reader.ReadUShort();
            ushort[] chainedSequenceRuleSetOffsets = reader.ReadUShortArray(chainedSequenceRuleSetCount);
            ChainedSequenceRuleSets = new ChainedSequenceRuleSet[chainedSequenceRuleSetCount];
            for (var i = 0; i < chainedSequenceRuleSetCount; i++)
            {
                reader.Seek(startOfTable + chainedSequenceRuleSetOffsets[i]);
                ChainedSequenceRuleSets[i] = new ChainedSequenceRuleSet(reader);
            }
            reader.Seek(startOfTable + coverageOffset);
            CoverageTable.Retrieve(reader);
        }
    }
}