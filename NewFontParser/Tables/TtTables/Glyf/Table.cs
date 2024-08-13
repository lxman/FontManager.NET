using System.Collections.Generic;
using NewFontParser.Reader;
using Serilog;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class Table : IInfoTable
    {
        public List<GlyphData> Glyphs { get; } = new List<GlyphData>();

        private static int _iteration;

        public Table(byte[] data, int numGlyphs, LocaTable offsets)
        {
            var reader = new BigEndianReader(data);
            for (var i = 0; i < numGlyphs; i++)
            {
                Log.Debug($"Reading glyph {i + 1}/{numGlyphs}");
                reader.Seek(offsets.Offsets[i]);
                bool complete;

                var glyphHeader = new GlyphHeader(reader.ReadBytes(GlyphHeader.RecordSize));
                IGlyphSpec spec;

                Log.Debug(glyphHeader.ToString());

                if (glyphHeader.NumberOfContours >= 0)
                {
                    Log.Debug("Adding a SimpleGlyph");
                    complete = false;
                    spec = new SimpleGlyph(reader, glyphHeader);
                    Log.Debug("Succeeded adding a SimpleGlyph");
                    complete = true;
                }
                else
                {
                    Log.Debug("Adding a CompositeGlyph");
                    complete = false;
                    spec = new CompositeGlyph(reader, glyphHeader);
                    Log.Debug("Succeeded adding a CompositeGlyph");
                    complete = true;
                }
                if (!complete) Log.Debug("****************************************************BANG*********************************************************");
                Glyphs.Add(new GlyphData(glyphHeader, spec));
            }

        }
    }
}
