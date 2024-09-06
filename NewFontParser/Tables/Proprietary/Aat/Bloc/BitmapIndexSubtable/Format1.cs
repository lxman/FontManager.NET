using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Aat.Bloc.BitmapIndexSubtable
{
    public class Format1
    {
        public IndexFormat IndexFormat { get; }

        public ImageFormat ImageFormat { get; }

        public Format1(
            BigEndianReader reader,
            ushort firstGlyphIndex,
            ushort lastGlyphIndex)
        {
            IndexFormat = (IndexFormat)reader.ReadUShort();
            ImageFormat = (ImageFormat)reader.ReadUShort();
            uint imageDataOffset = reader.ReadUInt32();
            //List<uint> offsets =
        }
    }
}