namespace FontParser.Tables.AdvancedLayout.Base
{
    public class BaseLangSysRecord
    {
        public readonly string baseScriptTag;
        public readonly ushort baseScriptOffset;

        public BaseLangSysRecord(string scriptTag, ushort offset)
        {
            baseScriptTag = scriptTag;
            baseScriptOffset = offset;
        }
    }
}