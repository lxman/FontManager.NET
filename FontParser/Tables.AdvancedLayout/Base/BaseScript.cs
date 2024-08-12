namespace FontParser.Tables.AdvancedLayout.Base
{
    public class BaseScript
    {
        public string ScriptIdenTag;
        public BaseValues baseValues;
        public BaseLangSysRecord[] baseLangSysRecords;
        public MinMax MinMax;

#if DEBUG

        public override string ToString()
        {
            return ScriptIdenTag;
        }

#endif
    }
}