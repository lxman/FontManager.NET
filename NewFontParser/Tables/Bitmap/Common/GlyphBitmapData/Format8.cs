﻿using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Bitmap.Ebdt;

namespace NewFontParser.Tables.Bitmap.Common.GlyphBitmapData
{
    public class Format8 : IGlyphBitmapDataFormat
    {
        public SmallGlyphMetricsRecord SmallGlyphMetrics { get; }

        public byte Pad { get; }

        public ushort ComponentCount { get; }

        public List<EbdtComponent> EbdtComponents { get; } = new List<EbdtComponent>();

        public Format8(BigEndianReader reader)
        {
            SmallGlyphMetrics = new SmallGlyphMetricsRecord(reader);
            Pad = reader.ReadByte();
            ComponentCount = reader.ReadUShort();
            for (var i = 0; i < ComponentCount; i++)
            {
                EbdtComponents.Add(new EbdtComponent(reader));
            }
        }
    }
}
