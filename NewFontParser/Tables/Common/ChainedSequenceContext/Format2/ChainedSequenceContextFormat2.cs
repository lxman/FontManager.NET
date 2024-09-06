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

        public ushort CoverageOffset { get; }

        public ushort BacktrackClassDefOffset { get; }

        public ushort InputClassDefOffset { get; }

        public ushort LookaheadClassDefOffset { get; }

        public ushort ChainedClassSequenceRuleSetCount { get; }

        public IClassDefinition BacktrackClassDef { get; }

        public IClassDefinition InputClassDef { get; }

        public IClassDefinition LookaheadClassDef { get; }

        public ChainedClassSequenceRuleSet[] ChainedClassSequenceRuleSets { get; }

        public ICoverageFormat Coverage { get; }

        public ChainedSequenceContextFormat2(BigEndianReader reader)
        {
            long position = reader.Position;

            Format = reader.ReadUShort();
            CoverageOffset = reader.ReadUShort();
            BacktrackClassDefOffset = reader.ReadUShort();
            InputClassDefOffset = reader.ReadUShort();
            LookaheadClassDefOffset = reader.ReadUShort();
            ChainedClassSequenceRuleSetCount = reader.ReadUShort();
            ushort[] chainedClassSequenceRuleSetOffsets = reader.ReadUShortArray(ChainedClassSequenceRuleSetCount);
            if (BacktrackClassDefOffset > 0)
            {
                reader.Seek(position + BacktrackClassDefOffset);
                byte backtrackFormat = reader.PeekBytes(2)[1];
                BacktrackClassDef = backtrackFormat switch
                {
                    1 => new ClassDefinition1(reader),
                    2 => new ClassDefinition2(reader),
                    _ => BacktrackClassDef
                };
            }
            if (InputClassDefOffset > 0)
            {
                reader.Seek(position + InputClassDefOffset);
                byte inputFormat = reader.PeekBytes(2)[1];
                InputClassDef = inputFormat switch
                {
                    1 => new ClassDefinition1(reader),
                    2 => new ClassDefinition2(reader),
                    _ => InputClassDef
                };
            }

            if (LookaheadClassDefOffset > 0)
            {
                reader.Seek(position + LookaheadClassDefOffset);
                byte lookaheadFormat = reader.PeekBytes(2)[1];
                LookaheadClassDef = lookaheadFormat switch
                {
                    1 => new ClassDefinition1(reader),
                    2 => new ClassDefinition2(reader),
                    _ => LookaheadClassDef
                };
            }

            ChainedClassSequenceRuleSets = new ChainedClassSequenceRuleSet[ChainedClassSequenceRuleSetCount];
            for (var i = 0; i < ChainedClassSequenceRuleSetCount; i++)
            {
                if (chainedClassSequenceRuleSetOffsets[i] == 0) continue;
                reader.Seek(position + chainedClassSequenceRuleSetOffsets[i]);
                ChainedClassSequenceRuleSets[i] = new ChainedClassSequenceRuleSet(reader);
            }

            if (CoverageOffset == 0) return;
            reader.Seek(position + CoverageOffset);
            byte coverageFormat = reader.PeekBytes(2)[1];
            Coverage = coverageFormat switch
            {
                1 => new CoverageFormat.Format1(reader),
                2 => new CoverageFormat.Format2(reader),
                _ => Coverage
            };
        }
    }
}