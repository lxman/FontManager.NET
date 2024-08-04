namespace FontParser.Tables.BitmapAndSvgFonts
{
    public class SvgDocumentEntry
    {
        public ushort startGlyphID;
        public ushort endGlyphID;
        public uint svgDocOffset;
        public uint svgDocLength;

        public byte[] svgBuffer;
        public bool compressed;

#if DEBUG

        public override string ToString()
        {
            return "startGlyphID:" + startGlyphID + "," +
                   "endGlyphID:" + endGlyphID + "," +
                   "svgDocOffset:" + svgDocOffset + "," +
                   "svgDocLength:" + svgDocLength;
        }

#endif
    }
}
