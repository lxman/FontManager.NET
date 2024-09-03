namespace NewFontParser.Tables.Proprietary.Tex
{
    // A proprietary obsoleted table created by the FF people. Replaced now by the 'MATH' table.
    public class TexTable : IInfoTable
    {
        public static string Tag => "TeX ";

        public TexTable(byte[] data)
        {
            // This table is obsoleted and not used anymore.
            // It was used to store information about the TeX font.
            // The 'MATH' table is now used for this purpose.
        }
    }
}
