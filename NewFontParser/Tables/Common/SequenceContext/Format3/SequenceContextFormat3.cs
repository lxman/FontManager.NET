using NewFontParser.Reader;
using NewFontParser.Tables.Common.SequenceContext.Format1;

namespace NewFontParser.Tables.Common.SequenceContext.Format3
{
    public class SequenceContextFormat3 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort GlyphCount { get; }

        public ushort SeqLookupCount { get; }

        public ushort[] CoverageOffsets { get; }

        public SequenceLookup[] SequenceLookups { get; }

        public SequenceContextFormat3(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            GlyphCount = reader.ReadUShort();
            SeqLookupCount = reader.ReadUShort();

            CoverageOffsets = new ushort[GlyphCount];
            for (var i = 0; i < GlyphCount; i++)
            {
                CoverageOffsets[i] = reader.ReadUShort();
            }

            SequenceLookups = new SequenceLookup[SeqLookupCount];
            for (var i = 0; i < SeqLookupCount; i++)
            {
                SequenceLookups[i] = new SequenceLookup(reader.ReadBytes(4));
            }
        }
    }
}