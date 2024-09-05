using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos
{
    public class GposTable : IInfoTable
    {
        public static string Tag => "GPOS";

        public GposHeader Header { get; }

        public GposLookupList GposLookupList { get; }

        public ScriptList ScriptList { get; }

        public FeatureList FeatureList { get; }

        public GposTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Header = new GposHeader(reader);

            reader.Seek(0);
            ScriptList = new ScriptList(reader, Header.ScriptListOffset);

            reader.Seek(0);
            FeatureList = new FeatureList(reader, Header.FeatureListOffset);

            reader.Seek(0);
            GposLookupList = new GposLookupList(reader, Header.LookupListOffset);
        }
    }
}