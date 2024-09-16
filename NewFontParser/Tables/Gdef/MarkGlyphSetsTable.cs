using System;
using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Gdef
{
    public class MarkGlyphSetsTable
    {
        public readonly ushort Format;

        public readonly List<ICoverageFormat> MarkSetTables = new List<ICoverageFormat>();

        public MarkGlyphSetsTable(BigEndianReader reader)
        {
            long position = reader.Position;

            Format = reader.ReadUShort();
            ushort markSetCount = reader.ReadUShort();
            var markSetOffsets = new uint[markSetCount];
            for (var i = 0; i < markSetCount; i++)
            {
                markSetOffsets[i] = reader.ReadUInt32();
            }
            for (var i = 0; i < markSetCount; i++)
            {
                reader.Seek(position + markSetOffsets[i]);
                byte format = reader.PeekBytes(2)[1];
                MarkSetTables.Add(format switch
                {
                    1 => new CoverageFormat1(reader),
                    2 => new CoverageFormat2(reader),
                    _ => throw new NotSupportedException($"MarkGlyphSetsTable format {Format} is not supported.")
                });
            }
        }
    }
}