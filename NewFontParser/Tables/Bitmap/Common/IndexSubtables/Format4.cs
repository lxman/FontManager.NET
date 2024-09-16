using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Common.IndexSubtables
{
    public class Format4 : IIndexSubtable
    {
        public ushort IndexFormat { get; }

        public ushort ImageFormat { get; }

        public uint ImageDataOffset { get; }

        public List<GlyphIdOffsetPair> GlyphIdOffsetPairs { get; } = new List<GlyphIdOffsetPair>();

        public Format4(BigEndianReader reader)
        {
            IndexFormat = reader.ReadUShort();
            ImageFormat = reader.ReadUShort();
            ImageDataOffset = reader.ReadUInt32();
            uint numGlyphs = reader.ReadUInt32();
            for (var i = 0; i < numGlyphs; i++)
            {
                GlyphIdOffsetPairs.Add(new GlyphIdOffsetPair(reader));
            }
        }
    }
}