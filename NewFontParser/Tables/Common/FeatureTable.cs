using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class FeatureTable
    {
        public ushort[] LookupListIndexes { get; }

        public FeatureParametersTable? FeatureParametersTable { get; }

        public FeatureTable(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            ushort featureParamsOffset = reader.ReadUShort();
            ushort lookupIndexCount = reader.ReadUShort();
            LookupListIndexes = reader.ReadUShortArray(lookupIndexCount);
            if (featureParamsOffset <= 0) return;
            long before = reader.Position;
            reader.Seek(startOfTable + featureParamsOffset);
            FeatureParametersTable = new FeatureParametersTable(reader);
            reader.Seek(before);
        }
    }
}
