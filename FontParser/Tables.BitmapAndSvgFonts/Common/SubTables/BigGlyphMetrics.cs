using System.IO;

namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    //BigGlyphMetrics
    //Type    Name
    //uint8   height
    //uint8   width
    //int8    horiBearingX
    //int8    horiBearingY
    //uint8   horiAdvance
    //int8    vertBearingX
    //int8    vertBearingY
    //uint8   vertAdvance

    public class BigGlyphMetrics
    {
        public byte height;
        public byte width;

        public sbyte horiBearingX;
        public sbyte horiBearingY;
        public byte horiAdvance;

        public sbyte vertBearingX;
        public sbyte vertBearingY;
        public byte vertAdvance;

        public const int SIZE = 8; //size of BigGlyphMetrics

        public static void ReadBigGlyphMetric(BinaryReader reader, ref BigGlyphMetrics output)
        {
            output.height = reader.ReadByte();
            output.width = reader.ReadByte();

            output.horiBearingX = (sbyte)reader.ReadByte();
            output.horiBearingY = (sbyte)reader.ReadByte();
            output.horiAdvance = reader.ReadByte();

            output.vertBearingX = (sbyte)reader.ReadByte();
            output.vertBearingY = (sbyte)reader.ReadByte();
            output.vertAdvance = reader.ReadByte();
        }
    }
}
