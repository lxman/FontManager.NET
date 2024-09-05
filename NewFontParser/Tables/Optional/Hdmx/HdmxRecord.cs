using NewFontParser.Reader;

namespace NewFontParser.Tables.Optional.Hdmx
{
    public class HdmxRecord
    {
        public byte PixelSize { get; }

        public byte MaxWidth { get; }

        public byte[] Widths { get; }

        public HdmxRecord(BigEndianReader reader, ushort numGlyphs)
        {
            PixelSize = reader.ReadByte();
            MaxWidth = reader.ReadByte();

            Widths = new byte[numGlyphs];
            for (var i = 0; i < numGlyphs; i++)
            {
                Widths[i] = reader.ReadByte();
            }
        }
    }
}