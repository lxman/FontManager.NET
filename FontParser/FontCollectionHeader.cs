namespace FontParser
{
    public class FontCollectionHeader
    {
        public ushort majorVersion;
        public ushort minorVersion;
        public uint numFonts;
        public int[] offsetTables;

        //
        //if version 2
        public uint dsigTag;

        public uint dsigLength;
        public uint dsigOffset;
    }
}