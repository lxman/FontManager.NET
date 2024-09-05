namespace NewFontParser.Tables.TtTables
{
    public class PrepTable : IInfoTable
    {
        public static string Tag => "prep";

        public byte[] Instructions { get; }

        public PrepTable(byte[] data)
        {
            Instructions = data;
        }
    }
}