using System.IO;

namespace FontParser.Tables.AdvancedLayout.FeatureList
{
    //Feature Table

    //A Feature table defines a feature with one or more lookups.
    //The client uses the lookups to substitute or position glyphs.

    //Feature tables defined within the GSUB table contain references to glyph substitution lookups,
    //and feature tables defined within the GPOS table contain references to glyph positioning lookups.
    //If a text-processing operation requires both glyph substitution and positioning,
    //then both the GSUB and GPOS tables must each define a Feature table,
    //and the tables must use the same FeatureTags.

    //A Feature table consists of an offset to a Feature Parameters (FeatureParams) table
    //(if one has been defined for this feature - see note in the following paragraph),
    //a count of the lookups listed for the feature (LookupCount),
    //and an arbitrarily ordered array of indices into a LookupList (LookupListIndex).
    //The LookupList indices are references into an array of offsets to Lookup tables.

    //The format of the Feature Parameters table is specific to a particular feature,
    //and must be specified in the feature's entry in the Feature Tags section of the OpenType Layout Tag Registry.
    //The length of the Feature Parameters table must be implicitly or explicitly specified in the Feature Parameters table itself.
    //The FeatureParams field in the Feature Table records the offset relative to the beginning of the Feature Table.
    //If a Feature Parameters table is not needed, the FeatureParams field must be set to NULL.

    //To identify the features in a GSUB or GPOS table,
    //a text-processing client reads the FeatureTag of each FeatureRecord referenced in a given LangSys table.
    //Then the client selects the features it wants to implement and uses the LookupList to retrieve the Lookup indices of the chosen features.
    //Next, the client arranges the indices in the LookupList order.
    //Finally, the client applies the lookup data to substitute or position glyphs.

    //Example 3 at the end of this chapter shows the FeatureList and Feature tables used to substitute ligatures in two languages.
    //
    public class FeatureTable
    {
        public static FeatureTable CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            ushort featureParams = reader.ReadUInt16();
            ushort lookupCount = reader.ReadUInt16();

            var featureTable = new FeatureTable
            {
                LookupListIndices = reader.ReadUInt16Array(lookupCount)
            };
            return featureTable;
        }

        public ushort[] LookupListIndices { get; private set; }
        public uint FeatureTag { get; set; }
        public string TagName => Utils.TagToString(FeatureTag);
#if DEBUG

        public override string ToString()
        {
            return TagName;
        }

#endif
    }
}
