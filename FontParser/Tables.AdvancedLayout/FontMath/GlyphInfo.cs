namespace FontParser.Tables.AdvancedLayout.FontMath
{
    public class GlyphInfo
    {
        public readonly ushort GlyphIndex;

        public GlyphInfo(ushort glyphIndex)
        {
            GlyphIndex = glyphIndex;
        }

        public ValueRecord? ItalicCorrection { get; internal set; }
        public ValueRecord? TopAccentAttachment { get; internal set; }
        public bool IsShapeExtensible { get; internal set; }

        //optional
        public Kern TopLeftKern => _kernRec.TopLeft;

        public Kern TopRightKern => _kernRec.TopRight;
        public Kern BottomLeftKern => _kernRec.BottomLeft;
        public Kern BottomRightKern => _kernRec.BottomRight;
        public bool HasSomeMathKern { get; private set; }

        //
        private KernInfoRecord _kernRec;

        internal void SetMathKerns(KernInfoRecord kernRec)
        {
            _kernRec = kernRec;
            HasSomeMathKern = true;
        }

        /// <summary>
        /// vertical glyph construction
        /// </summary>
        public GlyphConstruction VertGlyphConstruction;

        /// <summary>
        /// horizontal glyph construction
        /// </summary>
        public GlyphConstruction HoriGlyphConstruction;
    }
}
