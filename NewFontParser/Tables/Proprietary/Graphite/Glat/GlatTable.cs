namespace NewFontParser.Tables.Proprietary.Graphite.Glat
{
    public class GlatTable : IInfoTable
    {
        public static string Tag => "Glat";

        public GlatTable(byte[] data)
        {
            // TODO: Implement GLAT table parsing
            // GLAT table is a Graphite table that contains glyph attributes
            // I have been unable to find any documentation on this table
        }
    }
}