using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Gsub.LookupSubTables.AlternateSubstitution
{
    public class AlternateSubstitutionFormat1 : ILookupSubTable
    {
        public ICoverageFormat Coverage { get; }

        public List<AlternateSet> AlternateSets { get; } = new List<AlternateSet>();

        public AlternateSubstitutionFormat1(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            _ = reader.ReadUShort();
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