using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos
{
    public class GposTable : IFontTable
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

            reader.Seek(Header.ScriptListOffset);
            ScriptList = new ScriptList(reader);

            reader.Seek(Header.FeatureListOffset);
            FeatureList = new FeatureList(reader);

            reader.Seek(Header.LookupListOffset);
            GposLookupList = new GposLookupList(reader);
        }
    }
}