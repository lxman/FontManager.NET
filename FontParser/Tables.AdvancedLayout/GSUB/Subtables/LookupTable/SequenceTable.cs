namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class SequenceTable
    {
        public readonly ushort[] substituteGlyphs;

        public SequenceTable(ushort[] substituteGlyphs)
        {
            this.substituteGlyphs = substituteGlyphs;
        }
    }
}