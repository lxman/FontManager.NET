using System.Collections.Generic;
using FontParser.Exceptions;
using FontParser.Typeface;

namespace FontParser.Tables.CFF.CFF
{
    public class Cff1Font
    {
        internal string FontName { get; set; }
        internal Glyph[] _glyphs;

        internal List<byte[]> _localSubrRawBufferList;
        internal List<byte[]> _globalSubrRawBufferList;

        internal int _defaultWidthX;
        internal int _nominalWidthX;
        internal List<FontDict> _cidFontDict;

        public string Version { get; set; } //CFF SID
        public string Notice { get; set; }//CFF SID
        public string CopyRight { get; set; }//CFF SID
        public string FullName { get; set; }//CFF SID
        public string FamilyName { get; set; }//CFF SID
        public string Weight { get; set; }//CFF SID
        public double UnderlinePosition { get; set; }
        public double UnderlineThickness { get; set; }
        public double[] FontBBox { get; set; }
#if DEBUG

        public Cff1Font()
        {
        }

#endif

        internal IEnumerable<GlyphNameMap> GetGlyphNameIter()
        {
            int j = _glyphs.Length;
#if DEBUG
            if (j > ushort.MaxValue) { throw new OpenFontNotSupportedException(); }
#endif
            for (var i = 0; i < j; ++i)
            {
                Glyph cff1Glyph = _glyphs[i];
                yield return new GlyphNameMap((ushort)i, cff1Glyph._cff1GlyphData.Name);
            }
        }
    }
}
