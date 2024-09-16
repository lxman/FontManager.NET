using NewFontParser.Reader;
using NewFontParser.Tables.Common.CoverageFormat;
using NewFontParser.Tables.Common.GlyphClassDef;
using NewFontParser.Tables.Gdef;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.

namespace NewFontParser.Tables.Common.ChainedSequenceContext.Format2
{
    public class ChainedSequenceContextFormat2 : ILookupSubTable
    {
        public ushort Format { get; }

        public IClassDefinition BacktrackClassDef { get; }

        public IClassDefinition InputClassDef { get; }

        public IClassDefinition LookaheadClassDef { get; }

        public ChainedClassSequenceRuleSet[] ChainedClassSequenceRuleSets { get; }

        public ICoverageFormat Coverage { get; }

        public ChainedSequenceContextFormat2(BigEndianReader reader)
        {
            long startOfTable = reader.Position;

            Format = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ushort backtrackClassDefOffset = reader.ReadUShort();
            ushort inputClassDefOffset = reader.ReadUShort();
            ushort lookaheadClassDefOffset = reader.ReadUShort();
            ushort chainedClassSequenceRuleSetCount = reader.ReadUShort();
            ushort[] chainedClassSequenceRuleSetOffsets = reader.ReadUShortArray(chainedClassSequenceRuleSetCount);
            if (backtrackClassDefOffset > 0)
            {
                reader.Seek(startOfTable + backtrackClassDefOffset);
                byte backtrackFormat = reader.PeekBytes(2)[1];
                BacktrackClassDef = backtrackFormat switch
                {
                    1 => new ClassDefinition1(reader),
                    2 => new ClassDefinition2(reader),
                    _ => BacktrackClassDef
                };
            }
            if (inputClassDefOffset > 0)
            {
                reader.Seek(startOfTable + inputClassDefOffset);
                byte inputFormat = reader.PeekBytes(2)[1];
                InputClassDef = inputFormat switch
                {
                    1 => new ClassDefinition1(reader),
                    2 => new ClassDefinition2(reader),
                    _ => InputClassDef
                };
            }

            if (lookaheadClassDefOffset > 0)
            {
                reader.Seek(startOfTable + lookaheadClassDefOffset);
                byte lookaheadFormat = reader.PeekBytes(2)[1];
                LookaheadClassDef = lookaheadFormat switch
                {
                    1 => new ClassDefinition1(reader),
                    2 => new ClassDefinition2(reader),
                    _ => LookaheadClassDef
                };
            }

            ChainedClassSequenceRuleSets = new ChainedClassSequenceRuleSet[chainedClassSequenceRuleSetCount];
            for (var i = 0; i < chainedClassSequenceRuleSetCount; i++)
            {
                if (chainedClassSequenceRuleSetOffsets[i] == 0) continue;
                reader.Seek(startOfTable + chainedClassSequenceRuleSetOffsets[i]);
                ChainedClassSequenceRuleSets[i] = new ChainedClassSequenceRuleSet(reader);
            }

            if (coverageOffset == 0) return;
            reader.Seek(startOfTable + coverageOffset);
            Coverage = CoverageTable.Retrieve(reader);
        }
    }
}