using NewFontParser.Reader;

namespace NewFontParser.Tables.TtTables.Glyf
{
    public class CompositeGlyph : IGlyphSpec
    {
        public CompositeGlyphFlags Flags { get; }

        public ushort GlyphIndex { get; }

        public int Argument1 { get; }

        public int Argument2 { get; }

        public CompositeGlyph(BigEndianReader reader, GlyphHeader glyphHeader)
        {
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
    }
}
