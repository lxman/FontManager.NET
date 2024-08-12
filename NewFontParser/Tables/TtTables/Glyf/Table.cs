using System.Collections.Generic;
using NewFontParser.Reader;
using Serilog;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class Table : IInfoTable
    {
        public List<GlyphData> Glyphs { get; } = new List<GlyphData>();

        public Table(byte[] data, int numGlyphs, LocaTable offsets)
        {
            var reader = new BigEndianReader(data);
            for (var i = 0; i < numGlyphs; i++)
            {
                var glyphHeader = new GlyphHeader(reader.ReadBytes(GlyphHeader.RecordSize));
                IGlyphSpec spec;

                if (glyphHeader.NumberOfContours >= 0)
                {
                    Log.Debug("Adding a simple table");
                    spec = new SimpleGlyph(reader, glyphHeader);
                    Log.Debug("Success adding a simple table");
                }
                else
                {
                    Log.Debug("Adding a composite table");
                    spec = new CompositeGlyph(reader, glyphHeader);
                    Log.Debug("Success adding a composite table");
                }
                Glyphs.Add(new GlyphData(glyphHeader, spec));
            }
        }
    }
}
