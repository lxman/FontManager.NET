namespace FontParser.Tables.AdvancedLayout
{
    //https://docs.microsoft.com/en-us/typography/opentype/spec/featurelist

    public sealed class FeatureInfo
    {
        public readonly string fullname;
        public readonly string shortname;

        public FeatureInfo(string fullname, string shortname)
        {
            this.fullname = fullname;
            this.shortname = shortname;
        }
    }
}