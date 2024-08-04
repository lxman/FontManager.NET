namespace FontParser.Tables.AdvancedLayout.JustificationTable
{
    public class JstfScriptRecord
    {
        public readonly string jstfScriptTag;
        public readonly ushort jstfScriptOffset;

        public JstfScriptRecord(string jstfScriptTag, ushort jstfScriptOffset)
        {
            this.jstfScriptTag = jstfScriptTag;
            this.jstfScriptOffset = jstfScriptOffset;
        }
    }
}
