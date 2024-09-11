using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Bitmap.Ebdt;

namespace NewFontParser.Tables.Bitmap.Common.GlyphBitmapData
{
    public class Format9 : IGlyphBitmapData
    {
        public BigGlyphMetricsRecord BigMetrics { get; }

        public ushort ComponentCount { get; }

        public List<EbdtComponent> EbdtComponents { get; } = new List<EbdtComponent>();

        public Format9(BigEndianReader reader)
        {
            BigMetrics = new BigGlyphMetricsRecord(reader);
            ComponentCount = reader.ReadUShort();
            for (var i = 0; i < ComponentCount; i++)
            {
                EbdtComponents.Add(new EbdtComponent(reader));
            }
        }
    }
}