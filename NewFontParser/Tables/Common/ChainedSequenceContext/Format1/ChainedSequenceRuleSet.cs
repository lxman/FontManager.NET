using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format1
{
    public class ChainedSequenceRuleSet
    {
        public ushort ChainedSequenceRuleCount { get; }

        public ushort[] ChainedSequenceRuleOffsets { get; }

        public ChainedSequenceRule[] ChainedSequenceRules { get; }

        public ChainedSequenceRuleSet(BigEndianReader reader)
        {
            long position = reader.Position;
            ChainedSequenceRuleCount = reader.ReadUShort();
            ChainedSequenceRuleOffsets = new ushort[ChainedSequenceRuleCount];

            reader.Seek(position);

            var tables = new ReadSubTablesFromOffset16Array<ChainedSequenceRule>(reader, ChainedSequenceRuleOffsets);
            ChainedSequenceRules = tables.Tables;
        }
    }
}