using System;
using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gdef
{
    public class MarkGlyphSetsTable
    {
        public readonly ushort Format;

        public readonly ushort MarkSetCount;

        public readonly List<IClassDefinition> MarkSetTables = new List<IClassDefinition>();

        public MarkGlyphSetsTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Format = reader.ReadUShort();
            MarkSetCount = reader.ReadUShort();
            for (var i = 0; i < MarkSetCount; i++)
            {
                ushort offset = reader.ReadUShort();
                byte[] markSetTableData = data[offset..];
                byte[] formatBytes = reader.PeekBytes(2);
                var format = (ushort)(formatBytes[0] << 8 | formatBytes[1]);
                MarkSetTables.Add(format switch
                {
                    1 => new ClassDefinition1(markSetTableData),
                    2 => new ClassDefinition2(markSetTableData),
                    _ => throw new NotSupportedException($"MarkSetTable format {format} is not supported.")
                });
            }
        }
    }
}
