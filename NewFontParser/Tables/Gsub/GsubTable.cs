using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gsub
{
    public class GsubTable : IFontTable
    {
        public static string Tag => "GSUB";

        public GsubHeader Header { get; }

        public ScriptList ScriptList { get; }

        public FeatureList FeatureList { get; }

        public GsubLookupList GsubLookupList { get; }

        public FeatureVariationsTable FeatureVariationsTable { get; }

        public GsubTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Header = new GsubHeader(reader);
            reader.Seek(Header.ScriptListOffset);
            ScriptList = new ScriptList(reader);
            reader.Seek(Header.FeatureListOffset);
            FeatureList = new FeatureList(reader);
            reader.Seek(Header.FeatureListOffset);

            // TODO: Come back and fix this
            reader.Seek(Header.LookupListOffset);
            GsubLookupList = new GsubLookupList(reader);
        }
    }
}