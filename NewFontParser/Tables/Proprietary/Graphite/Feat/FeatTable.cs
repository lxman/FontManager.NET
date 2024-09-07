using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Graphite.Feat
{
    public class FeatTable : IInfoTable
    {
        public static string Tag => "Feat";

        public uint Version { get; }

        public ushort FeatureCount { get; }

        public List<FeatureSpec> FeatureSpecs { get; } = new List<FeatureSpec>();

        public List<Settings> Settings { get; } = new List<Settings>();

        public FeatTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Version = reader.ReadUInt32();
            FeatureCount = reader.ReadUShort();
            _ = reader.ReadUShort(); // Reserved
            _ = reader.ReadUInt32(); // Reserved
            for (var i = 0; i < FeatureCount; i++)
            {
                FeatureSpecs.Add(new FeatureSpec(reader));
            }
            for (var i = 0; i < FeatureCount; i++)
            {
                Settings.Add(new Settings(reader));
            }
            // TODO: Implement the rest of the Feat table
            // https://graphite.sil.org/specification/feat
        }
    }
}