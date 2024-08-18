using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.CoverageFormat;

namespace NewFontParser.Tables.Gdef
{
    public class AttachListTable
    {
        public ICoverageFormat CoverageTable { get; }

        public ushort GlyphCount { get; }

        public List<ushort> AttachPointOffsets { get; } = new List<ushort>();

        public AttachListTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            ushort coverageOffset = reader.ReadUShort();
            byte[] coverageData = data[coverageOffset..];
            ushort coverageVersion = BinaryPrimitives.ReadUInt16BigEndian(coverageData);
            CoverageTable = coverageVersion switch
            {
                1 => new Format1(coverageData),
                2 => new Format2(coverageData),
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
