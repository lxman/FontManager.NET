using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gsub
{
    public class GsubTable : IInfoTable
    {
        public static string Tag => "GSUB";

        public GsubHeader Header { get; }

        public ScriptList ScriptList { get; }

        public FeatureList FeatureList { get; }

        public GposLookupList GposLookupList { get; }

        public FeatureVariationsTable FeatureVariationsTable { get; }

        public GsubTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Header = new GsubHeader(reader);
            reader.Seek(0);
            ScriptList = new ScriptList(reader, Header.ScriptListOffset);
            FeatureList = new FeatureList(reader, Header.FeatureListOffset);
            //GposLookupList = new GposLookupList(reader, Header.LookupListOffset);
            //if (Header.FeatureVariationsOffset.HasValue)
            //{
            //    reader.Seek(Header.FeatureVariationsOffset ?? 0);
            //    FeatureVariationsTable = new FeatureVariationsTable(reader);
            //}
        }
    }
}