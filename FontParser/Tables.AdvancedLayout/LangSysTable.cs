using System.IO;

namespace FontParser.Tables.AdvancedLayout
{
    public class LangSysTable
    {
        //The Language System table (LangSys) identifies language-system features
        //used to render the glyphs in a script. (The LookupOrder offset is reserved for future use.)
        //
        public uint langSysTagIden { get; private set; }

        internal readonly ushort offset;

        //
        public ushort[] featureIndexList { get; private set; }

        public ushort RequiredFeatureIndex { get; private set; }

        public LangSysTable(uint langSysTagIden, ushort offset)
        {
            this.offset = offset;
            this.langSysTagIden = langSysTagIden;
        }

        public void ReadFrom(BinaryReader reader)
        {
            //---------------------
            //LangSys table
            //Type 	    Name 	                    Description
            //Offset16 	lookupOrder 	            = NULL (reserved for an offset to a reordering table)
            //uint16 	requiredFeatureIndex        Index of a feature required for this language system- if no required features = 0xFFFF
            //uint16 	featureIndexCount 	            Number of FeatureIndex values for this language system-excludes the required feature
            //uint16 	featureIndices[featureIndexCount] 	Array of indices into the FeatureList-in arbitrary order
            //---------------------

            ushort lookupOrder = reader.ReadUInt16();//reserve
            RequiredFeatureIndex = reader.ReadUInt16();
            ushort featureIndexCount = reader.ReadUInt16();
            featureIndexList = reader.ReadUInt16Array(featureIndexCount);
        }

        public bool HasRequireFeature => RequiredFeatureIndex != 0xFFFF;
        public string LangSysTagIdenString => (langSysTagIden == 0) ? "" : Utils.TagToString(langSysTagIden);
#if DEBUG

        public override string ToString() => LangSysTagIdenString;

#endif
    }
}
