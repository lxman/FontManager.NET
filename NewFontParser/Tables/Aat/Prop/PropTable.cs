namespace NewFontParser.Tables.Aat.Prop
{
    public class PropTable : IInfoTable
    {
        public static string Tag => "prop";

        public PropTable(byte[] data)
        {
            // https://developer.apple.com/fonts/TrueType-Reference-Manual/RM06/Chap6prop.html
            // TODO: Implement this
            // Proprietary Apple AAT table
        }
    }
}