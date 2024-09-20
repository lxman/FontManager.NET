using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class FeatureList
    {
        public List<FeatureRecord> FeatureRecords { get; }

        public FeatureList(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            ushort featureCount = reader.ReadUShort();
            FeatureRecords = new List<FeatureRecord>(featureCount);

            for (var i = 0; i < featureCount; i++)
            {
                FeatureRecords.Add(new FeatureRecord(reader, startOfTable));
            }
        }
    }
}