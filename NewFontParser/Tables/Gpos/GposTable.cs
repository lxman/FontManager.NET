﻿using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.FeatureParametersTable;

namespace NewFontParser.Tables.Gpos
{
    public class GposTable : IFontTable
    {
        public static string Tag => "GPOS";

        public GposLookupList GposLookupList { get; }

        public ScriptList ScriptList { get; }

        public FeatureList FeatureList { get; }

        public GposTable(byte[] data)
        {
            using var reader = new BigEndianReader(data);

            var header = new GposHeader(reader);

            reader.Seek(header.ScriptListOffset);
            ScriptList = new ScriptList(reader);

            reader.Seek(header.FeatureListOffset);
            FeatureList = new FeatureList(reader);

            reader.Seek(header.LookupListOffset);
            GposLookupList = new GposLookupList(reader);
        }
    }
}