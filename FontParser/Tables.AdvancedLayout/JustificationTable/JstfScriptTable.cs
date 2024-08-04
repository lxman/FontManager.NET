namespace FontParser.Tables.AdvancedLayout.JustificationTable
{
    public class JstfScriptTable
    {
        public ushort[] extenderGlyphs;

        public JstfLangSysRecord defaultLangSys;
        public JstfLangSysRecord[] other;

        public JstfScriptTable()
        {
        }

        public string ScriptTag { get; set; }
#if DEBUG

        public override string ToString()
        {
            return ScriptTag;
        }

#endif
    }
}
