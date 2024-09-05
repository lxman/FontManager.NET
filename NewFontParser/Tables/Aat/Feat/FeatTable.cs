using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Aat.Feat
{
    public class FeatTable : IInfoTable
    {
        public static string Tag => "feat";

        public Header Header { get; }

        public List<FeatureName> Names { get; } = new List<FeatureName>();

        public FeatTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Header = new Header(reader);
            for (var i = 0; i < Header.FeatureCount; i++)
            {
                Names.Add(new FeatureName(reader));
            }
            Names.ForEach(n => n.ReadSettings(reader));
        }
    }
}