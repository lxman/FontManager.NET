using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format1
{
    public class ChainedSequenceContextFormat1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort CoverageOffset { get; }

        public ushort ChainedSequenceRuleSetCount { get; }

        public ushort[] ChainedSequenceRuleSetOffsets { get; }

        public ChainedSequenceRuleSet[] ChainedSequenceRuleSets { get; }

        public ChainedSequenceContextFormat1(BigEndianReader reader)
        {
            long position = reader.Position;
            Format = reader.ReadUShort();
            CoverageOffset = reader.ReadUShort();
            ChainedSequenceRuleSetCount = reader.ReadUShort();
            ChainedSequenceRuleSetOffsets = new ushort[ChainedSequenceRuleSetCount];
            for (var i = 0; i < ChainedSequenceRuleSetCount; i++)
            {
                ChainedSequenceRuleSetOffsets[i] = reader.ReadUShort();
            }

            reader.Seek(position);
            var tables = new ReadSubTablesFromOffset16Array<ChainedSequenceRuleSet>(reader, ChainedSequenceRuleSetOffsets);
            ChainedSequenceRuleSets = tables.Tables;
        }
    }
}
