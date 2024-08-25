using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class FeatureList
    {
        public ushort FeatureCount { get; }

        public List<FeatureRecord> FeatureRecords { get; }

        public FeatureList(BigEndianReader reader, uint offset)
        {
            reader.Seek(offset);

            FeatureCount = reader.ReadUShort();
            FeatureRecords = new List<FeatureRecord>(FeatureCount);

            for (var i = 0; i < FeatureCount; i++)
            {
                FeatureRecords.Add(new FeatureRecord(reader));
            }
        }
    }
}
