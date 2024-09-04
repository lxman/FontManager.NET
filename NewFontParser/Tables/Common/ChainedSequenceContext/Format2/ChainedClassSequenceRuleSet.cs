using NewFontParser.Reader;
using Serilog;

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format2
{
    public class ChainedClassSequenceRuleSet
    {
        public ushort ChainedClassSequenceRuleCount { get; }

        public ChainedClassSequenceRule[] ChainedClassSequenceRules { get; }

        public ChainedClassSequenceRuleSet(BigEndianReader reader)
        {
            long position = reader.Position;

            ChainedClassSequenceRuleCount = reader.ReadUShort();
            ushort[] chainedClassSequenceRuleOffsets = reader.ReadUShortArray(ChainedClassSequenceRuleCount);
            ChainedClassSequenceRules = new ChainedClassSequenceRule[ChainedClassSequenceRuleCount];
            for (var i = 0; i < ChainedClassSequenceRuleCount; i++)
            {
                reader.Seek(position + chainedClassSequenceRuleOffsets[i]);
                Log.Debug("Creating ChainedClassSequenceRule");
                ChainedClassSequenceRules[i] = new ChainedClassSequenceRule(reader);
                Log.Debug("Creating ChainedClassSequenceRule success!");
            }
        }
    }
}
