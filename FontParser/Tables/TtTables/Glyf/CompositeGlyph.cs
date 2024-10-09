using FontParser.Reader;
using FontParser.Tables.WOFF2.GlyfReconstruct;

namespace FontParser.Tables.TtTables.Glyf
{
    public class CompositeGlyph : IGlyphSpec
    {
        public CompositeGlyphFlags Flags { get; }

        public ushort GlyphIndex { get; }

        public int Argument1 { get; }

        public int Argument2 { get; }

        public CompositeGlyph(
            BigEndianReader reader,
            GlyphHeader glyphHeader,
            bool woff2Reconstruct = false)
        {
            if (woff2Reconstruct) return;
            Flags = (CompositeGlyphFlags)reader.ReadUShort();
            GlyphIndex = reader.ReadUShort();
            if (Flags.HasFlag(CompositeGlyphFlags.ArgsAreXyValues))
            {
                Argument1 = reader.ReadShort();
                Argument2 = reader.ReadShort();
            }
            else
            {
                Argument1 = 0;
                Argument2 = 0;
            }
        }

        public void Woff2Reconstruct(CompositeGlyphInfo compositeGlyphInfo)
        {
            // Transfer information from CompositeGlyphInfo to this instance
        }
    }
}