using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Gsub.LookupSubTables.ReverseChainSingleSubstitution
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ICoverageFormat Coverage { get; }

        public List<ICoverageFormat> BacktrackCoverages { get; } = new List<ICoverageFormat>();

        public List<ICoverageFormat> LookaheadCoverages { get; } = new List<ICoverageFormat>();

        public ushort[] SubstituteGlyphIds { get; }

        public Format1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            Format = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ushort backtrackGlyphCount = reader.ReadUShort();
            ushort[] backtrackOffsets = reader.ReadUShortArray(backtrackGlyphCount);
            ushort lookaheadGlyphCount = reader.ReadUShort();
            ushort[] lookaheadOffsets = reader.ReadUShortArray(lookaheadGlyphCount);
            ushort substCount = reader.ReadUShort();
            SubstituteGlyphIds = reader.ReadUShortArray(substCount);
            for (var i = 0; i < backtrackGlyphCount; i++)
            {
                reader.Seek(startOfTable + backtrackOffsets[i]);
                BacktrackCoverages.Add(CoverageTable.Retrieve(reader));
            }
            for (var i = 0; i < lookaheadGlyphCount; i++)
            {
                reader.Seek(startOfTable + lookaheadOffsets[i]);
                LookaheadCoverages.Add(CoverageTable.Retrieve(reader));
            }
            reader.Seek(startOfTable + coverageOffset);
            Coverage = CoverageTable.Retrieve(reader);
        }
    }
}