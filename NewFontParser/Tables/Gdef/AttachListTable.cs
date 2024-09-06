using System;
using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Gdef
{
    public class AttachListTable
    {
        public ICoverageFormat CoverageTable { get; }

        public ushort GlyphCount { get; }

        public List<ushort> AttachPointOffsets { get; } = new List<ushort>();

        public AttachListTable(BigEndianReader reader)
        {
            long position = reader.Position;
            ushort coverageOffset = reader.ReadUShort();
            reader.Seek(position + coverageOffset);
            byte coverageVersion = reader.PeekBytes(2)[1];
            CoverageTable = coverageVersion switch
            {
                1 => new Format1(reader),
                2 => new Format2(reader),
                _ => throw new NotSupportedException($"Unsupported coverage format: {coverageVersion}")
            };
            GlyphCount = reader.ReadUShort();
            for (var i = 0; i < GlyphCount; i++)
            {
                AttachPointOffsets.Add(reader.ReadUShort());
            }
        }
    }
}