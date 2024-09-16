using System;
using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gsub.LookupSubTables.LigatureSubstitution
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public List<LigatureSet> LigatureSets { get; } = new List<LigatureSet>();

        public Format1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            Format = reader.ReadUShort();
            if (Format != 1)
            {
                throw new Exception($"Expected Format 1, but got {Format}");
            }
            ushort coverageOffset = reader.ReadUShort();
            ushort ligSetCount = reader.ReadUShort();
            ushort[] ligSetOffsets = reader.ReadUShortArray(ligSetCount);
            for (var i = 0; i < ligSetCount; i++)
            {
                reader.Seek(startOfTable + ligSetOffsets[i]);
                LigatureSets.Add(new LigatureSet(reader));
            }
        }
    }
}
