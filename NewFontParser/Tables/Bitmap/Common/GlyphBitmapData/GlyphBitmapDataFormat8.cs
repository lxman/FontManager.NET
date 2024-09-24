﻿using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Bitmap.Ebdt;

namespace NewFontParser.Tables.Bitmap.Common.GlyphBitmapData
{
    public class GlyphBitmapDataFormat8 : IGlyphBitmapData
    {
        public SmallGlyphMetricsRecord SmallGlyphMetrics { get; }

        public byte Pad { get; }

        public List<EbdtComponent> EbdtComponents { get; } = new List<EbdtComponent>();

        public GlyphBitmapDataFormat8(BigEndianReader reader)
        {
            SmallGlyphMetrics = new SmallGlyphMetricsRecord(reader);
            Pad = reader.ReadByte();
            ushort componentCount = reader.ReadUShort();
            for (var i = 0; i < componentCount; i++)
            {
                EbdtComponents.Add(new EbdtComponent(reader));
            }
        }
    }
}