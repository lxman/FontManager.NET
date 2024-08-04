using System.IO;
using FontParser.Exceptions;

namespace FontParser.Tables.CFF.CFF
{
    internal static class CFFBinaryReaderExtension
    {
        public static int ReadOffset(this BinaryReader reader, int offsetSize)
        {
            switch (offsetSize)
            {
                default: throw new OpenFontNotSupportedException();
                case 1:
                    return reader.ReadByte();

                case 2:
                    return (reader.ReadByte() << 8) | (reader.ReadByte() << 0);

                case 3:
                    return (reader.ReadByte() << 16) | (reader.ReadByte() << 8) | (reader.ReadByte() << 0);

                case 4:
                    return (reader.ReadByte() << 24) | (reader.ReadByte() << 16) | (reader.ReadByte() << 8) | (reader.ReadByte() << 0);
            }
        }
    }
}
