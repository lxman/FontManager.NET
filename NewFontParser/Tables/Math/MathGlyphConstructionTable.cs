using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Math
{
    public class MathGlyphConstructionTable
    {
        public GlyphAssemblyTable GlyphAssembly { get; }

        public List<MathGlyphVariantRecord> GlyphVariantRecords { get; } = new List<MathGlyphVariantRecord>();

        public MathGlyphConstructionTable(BigEndianReader reader)
        {
            var position = reader.Position;

            ushort glyphAssemblyOffset = reader.ReadUShort();

            ushort variantCount = reader.ReadUShort();

            for (var i = 0; i < variantCount; i++)
            {
                GlyphVariantRecords.Add(new MathGlyphVariantRecord(reader));
            }

            reader.Seek(position + glyphAssemblyOffset);

            GlyphAssembly = new GlyphAssemblyTable(reader);
        }
    }
}
