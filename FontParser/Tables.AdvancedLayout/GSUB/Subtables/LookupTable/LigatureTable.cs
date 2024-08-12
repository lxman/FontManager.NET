using System.IO;
using System.Text;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class LigatureTable
    {
        //uint16 	LigGlyph 	GlyphID of ligature to substitute
        //uint16 	CompCount 	Number of components in the ligature
        //uint16 	Component[CompCount - 1] 	Array of component GlyphIDs-start with the second component-ordered in writing direction
        /// <summary>
        /// output glyph
        /// </summary>
        public readonly ushort GlyphId;

        /// <summary>
        /// ligature component start with second ordered glyph
        /// </summary>
        public readonly ushort[] ComponentGlyphs;

        public LigatureTable(ushort glyphId, ushort[] componentGlyphs)
        {
            GlyphId = glyphId;
            ComponentGlyphs = componentGlyphs;
        }

        public static LigatureTable CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);

            //
            ushort glyphIndex = reader.ReadUInt16();
            ushort compCount = reader.ReadUInt16();
            return new LigatureTable(glyphIndex, reader.ReadUInt16Array(compCount - 1));
        }

#if DEBUG

        public override string ToString()
        {
            var stbuilder = new StringBuilder();
            int j = ComponentGlyphs.Length;
            stbuilder.Append("output:" + GlyphId + ",{");

            for (var i = 0; i < j; ++i)
            {
                if (i > 0)
                {
                    stbuilder.Append(',');
                }
                stbuilder.Append(ComponentGlyphs[i]);
            }
            stbuilder.Append("}");
            return stbuilder.ToString();
        }

#endif
    }
}