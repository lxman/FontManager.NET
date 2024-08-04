namespace FontParser.Tables.AdvancedLayout.Base
{
    public class BaseScriptRecord
    {
        public readonly string baseScriptTag;
        public readonly ushort baseScriptOffset;

        public BaseScriptRecord(string scriptTag, ushort offset)
        {
            baseScriptTag = scriptTag;
            baseScriptOffset = offset;
        }
    }
}