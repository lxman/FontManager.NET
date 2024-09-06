using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Aat.Bloc.BitmapIndexSubtable
{
    public class Format3
    {
        public IndexFormat IndexFormat { get; }

        public ImageFormat ImageFormat { get; }

        public List<ushort> OffsetArray { get; } = new List<ushort>();

        public Format3(BigEndianReader reader)
        {
            IndexFormat = (IndexFormat)reader.ReadUShort();
            ImageFormat = (ImageFormat)reader.ReadUShort();
            ushort offsetArrayCount = reader.ReadUShort();
            for (var i = 0; i < offsetArrayCount; i++)
            {
                OffsetArray.Add(reader.ReadUShort());
            }
        }
    }
}