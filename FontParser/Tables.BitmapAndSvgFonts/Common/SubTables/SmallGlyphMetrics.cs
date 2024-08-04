using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    //SmallGlyphMetrics
    //Type    Name
    //uint8   height
    //uint8   width
    //int8    bearingX
    //int8    bearingY
    //uint8   advance
    public struct SmallGlyphMetrics
    {
        public byte height;
        public byte width;
        public sbyte bearingX;
        public sbyte bearingY;
        public byte advance;

        public const int SIZE = 5; //size of SmallGlyphMetrics

        public static void ReadSmallGlyphMetric(BinaryReader reader, out SmallGlyphMetrics output)
        {
            output = new SmallGlyphMetrics
            {
                height = reader.ReadByte(),
                width = reader.ReadByte(),
                bearingX = (sbyte)reader.ReadByte(),
                bearingY = (sbyte)reader.ReadByte(),
                advance = reader.ReadByte()
            };
        }
    }
}
