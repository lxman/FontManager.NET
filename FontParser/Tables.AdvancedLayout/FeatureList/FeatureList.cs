using System.IO;

namespace FontParser.Tables.AdvancedLayout.FeatureList
{
    //https://docs.microsoft.com/en-us/typography/opentype/spec/featurelist
    //The order for applying standard features encoded in OpenType fonts:

    //Feature   	Feature function 	                                Layout operation 	Required
    //---------------------
    //Language based forms:
    //---------------------
    //ccmp 	        Character composition/decomposition substitution 	GSUB
    //---------------------
    //Typographical forms:
    //---------------------
    //liga 	        Standard ligature substitution 	                    GSUB
    //clig 	        Contextual ligature substitution 	                GSUB
    //Positioning features:
    //kern 	        Pair kerning 	                                    GPOS
    //mark 	        Mark to base positioning 	                        GPOS 	X
    //mkmk 	        Mark to mark positioning 	                        GPOS 	X

    //[GSUB = glyph substitution, GPOS = glyph positioning]

    public class FeatureList
    {
        public FeatureTable[] featureTables;

        public static FeatureList CreateFrom(BinaryReader reader, long beginAt)
        {
            //https://docs.microsoft.com/en-us/typography/opentype/spec/chapter2

            //------------------
            //FeatureList table
            //------------------
            //Type 	    Name 	        Description
            //uint16 	FeatureCount 	Number of FeatureRecords in this table
            //struct 	FeatureRecord[FeatureCount] 	Array of FeatureRecords-zero-based (first feature has FeatureIndex = 0)-listed alphabetically by FeatureTag
            //------------------
            //FeatureRecord
            //------------------
            //Type 	    Name 	        Description
            //Tag 	    FeatureTag 	    4-byte feature identification tag
            //Offset16 	Feature 	    Offset to Feature table-from beginning of FeatureList
            //----------------------------------------------------
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            var featureList = new FeatureList();
            ushort featureCount = reader.ReadUInt16();
            FeatureRecord[] featureRecords = new FeatureRecord[featureCount];
            for (var i = 0; i < featureCount; ++i)
            {
                //read script record
                featureRecords[i] = new FeatureRecord(
                    reader.ReadUInt32(), //feature tag
                    reader.ReadUInt16()); //Offset16
            }
            //read each feature table
            FeatureTable[] featureTables = featureList.featureTables = new FeatureTable[featureCount];
            for (var i = 0; i < featureCount; ++i)
            {
                FeatureRecord frecord = featureRecords[i];
                (featureTables[i] = FeatureTable.CreateFrom(reader, beginAt + frecord.offset)).FeatureTag = frecord.featureTag;
            }
            return featureList;
        }
    }
}