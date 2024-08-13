namespace NewFontParser.Tables.TtTables
{
    public class PrepTable : IInfoTable
    {
        public byte[] Instructions { get; }

        public PrepTable(byte[] data)
        {
            Instructions = data;
        }
    }
}
