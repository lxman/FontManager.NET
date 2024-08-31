using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class GlyphTable : IInfoTable
    {
        public static string Tag => "glyf";

        public List<GlyphData> Glyphs { get; } = new List<GlyphData>();

        private readonly BigEndianReader _reader;

        public GlyphTable(byte[] data)
        {
            _reader = new BigEndianReader(data);
        }

        // numGlyphs from maxp table
        // offsets from loca table
        public void Process(int numGlyphs, LocaTable offsets)
        {
            for (var i = 0; i < numGlyphs; i++)
            {
                _reader.Seek(offsets.Offsets[i]);

                var glyphHeader = new GlyphHeader(_reader.ReadBytes(GlyphHeader.RecordSize));
                IGlyphSpec spec;

                if (glyphHeader.NumberOfContours >= 0)
                {
                    spec = new SimpleGlyph(_reader, glyphHeader);
                }
                else
                {
                    spec = new CompositeGlyph(_reader, glyphHeader);
                }
                Glyphs.Add(new GlyphData(glyphHeader, spec));
            }
        }
    }
}
