namespace FontParser.Tables.AdvancedLayout.Base
{
    public class FeatureMinMax
    {
        public readonly string featureTableTag;
        public readonly BaseCoord minCoord;
        public readonly BaseCoord maxCoord;

        public FeatureMinMax(string tag, BaseCoord minCoord, BaseCoord maxCoord)
        {
            featureTableTag = tag;
            this.minCoord = minCoord;
            this.maxCoord = maxCoord;
        }
    }
}