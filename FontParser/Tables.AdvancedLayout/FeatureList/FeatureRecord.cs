namespace FontParser.Tables.AdvancedLayout.FeatureList
{
    public class FeatureRecord
    {
        public readonly uint featureTag;//4-byte ScriptTag identifier
        public readonly ushort offset; //Script Offset to Script table-from beginning of ScriptList

        public FeatureRecord(uint featureTag, ushort offset)
        {
            this.featureTag = featureTag;
            this.offset = offset;
        }

        public string FeatureName => Utils.TagToString(featureTag);
#if DEBUG

        public override string ToString()
        {
            return FeatureName + "," + offset;
        }

#endif
    }
}
