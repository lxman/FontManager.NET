namespace FontParser.Tables.AdvancedLayout.Base
{
    public class BaseValues
    {
        public readonly ushort defaultBaseLineIndex;
        public readonly BaseCoord[] baseCoords;

        public BaseValues(ushort defaultBaseLineIndex, BaseCoord[] baseCoords)
        {
            this.defaultBaseLineIndex = defaultBaseLineIndex;
            this.baseCoords = baseCoords;
        }
    }
}