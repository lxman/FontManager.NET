namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    public class IndexSubHeader
    {
        public readonly ushort indexFormat;
        public readonly ushort imageFormat;
        public readonly uint imageDataOffset;

        public IndexSubHeader(ushort indexFormat,
            ushort imageFormat, uint imageDataOffset)
        {
            this.indexFormat = indexFormat;
            this.imageFormat = imageFormat;
            this.imageDataOffset = imageDataOffset;
        }

#if DEBUG

        public override string ToString()
        {
            return indexFormat + "," + imageFormat;
        }

#endif
    }
}
