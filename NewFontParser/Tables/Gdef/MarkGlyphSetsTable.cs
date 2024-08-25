using System;
using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.GlyphClassDef;

namespace NewFontParser.Tables.Gdef
{
    public class MarkGlyphSetsTable
    {
        public readonly ushort Format;

        public readonly ushort MarkSetCount;

        public uint[] MarkSetOffsets;

        public readonly List<IClassDefinition> MarkSetTables = new List<IClassDefinition>();

        public MarkGlyphSetsTable(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            MarkSetCount = reader.ReadUShort();
            MarkSetOffsets = new uint[MarkSetCount];
            for (var i = 0; i < MarkSetCount; i++)
            {
                MarkSetOffsets[i] = reader.ReadUInt32();
            }
            for (var i = 0; i < MarkSetCount; i++)
            {
                reader.Seek(MarkSetOffsets[i]);
                MarkSetTables.Add(Format switch
                {
                    1 => new ClassDefinition1(reader),
                    2 => new ClassDefinition2(reader),
                    _ => throw new NotSupportedException($"MarkGlyphSetsTable format {Format} is not supported.")
                });
            }
        }
    }
}
