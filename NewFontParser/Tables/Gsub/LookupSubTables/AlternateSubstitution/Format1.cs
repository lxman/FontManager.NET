using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Gsub.LookupSubTables.AlternateSubstitution
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ICoverageFormat Coverage { get; }

        public List<AlternateSet> AlternateSets { get; } = new List<AlternateSet>();

        public Format1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            Format = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ushort alternateSetCount = reader.ReadUShort();
            ushort[] alternateSetOffsets = reader.ReadUShortArray(alternateSetCount);
            for (var i = 0; i < alternateSetCount; i++)
            {
                reader.Seek(startOfTable + alternateSetOffsets[i]);
                AlternateSets.Add(new AlternateSet(reader));
            }
            reader.Seek(startOfTable + coverageOffset);
            Coverage = CoverageTable.Retrieve(reader);
        }
    }
}