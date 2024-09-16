using System.Collections.Generic;
using System.Linq;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Common.IndexSubtables
{
    public class Format3 : IIndexSubtable
    {
        public ushort IndexFormat { get; }

        public ushort ImageFormat { get; }

        public uint ImageDataOffset { get; }

        public List<ushort> BitmapDataOffsets { get; }

        public Format3(BigEndianReader reader, ushort numOffsets)
        {
            IndexFormat = reader.ReadUShort();
            ImageFormat = reader.ReadUShort();
            ImageDataOffset = reader.ReadUInt32();
            BitmapDataOffsets = reader.ReadUShortArray(numOffsets).ToList();
        }
    }
}