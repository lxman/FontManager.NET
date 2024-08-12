using System.Text;
using Kaitai;
using KaitaiTtf.Enums;

namespace KaitaiTtf.Os2
{
    /// <summary>
    /// The OS/2 table consists of a set of metrics that are required by Windows and OS/2.
    /// </summary>
    public class Os2 : KaitaiStruct
    {
        public static Os2 FromFile(string fileName)
        {
            return new Os2(new KaitaiStream(fileName));
        }

        public Os2(KaitaiStream p__io, DirTableEntry p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _version = m_io.ReadU2be();
            _xAvgCharWidth = m_io.ReadS2be();
            _weightClass = ((WeightClass)m_io.ReadU2be());
            _widthClass = ((WidthClass)m_io.ReadU2be());
            _fsType = ((FsType)m_io.ReadS2be());
            _ySubscriptXSize = m_io.ReadS2be();
            _ySubscriptYSize = m_io.ReadS2be();
            _ySubscriptXOffset = m_io.ReadS2be();
            _ySubscriptYOffset = m_io.ReadS2be();
            _ySuperscriptXSize = m_io.ReadS2be();
            _ySuperscriptYSize = m_io.ReadS2be();
            _ySuperscriptXOffset = m_io.ReadS2be();
            _ySuperscriptYOffset = m_io.ReadS2be();
            _yStrikeoutSize = m_io.ReadS2be();
            _yStrikeoutPosition = m_io.ReadS2be();
            _sFamilyClass = m_io.ReadS2be();
            _panose = new Panose(m_io, this, m_root);
            _unicodeRange = new UnicodeRange(m_io, this, m_root);
            _achVendId = Encoding.GetEncoding("ascii").GetString(m_io.ReadBytes(4));
            _selection = ((FsSelection)m_io.ReadU2be());
            _firstCharIndex = m_io.ReadU2be();
            _lastCharIndex = m_io.ReadU2be();
            _typoAscender = m_io.ReadS2be();
            _typoDescender = m_io.ReadS2be();
            _typoLineGap = m_io.ReadS2be();
            _winAscent = m_io.ReadU2be();
            _winDescent = m_io.ReadU2be();
            _codePageRange = new CodePageRange(m_io, this, m_root);
        }
        private ushort _version;
        private short _xAvgCharWidth;
        private WeightClass _weightClass;
        private WidthClass _widthClass;
        private FsType _fsType;
        private short _ySubscriptXSize;
        private short _ySubscriptYSize;
        private short _ySubscriptXOffset;
        private short _ySubscriptYOffset;
        private short _ySuperscriptXSize;
        private short _ySuperscriptYSize;
        private short _ySuperscriptXOffset;
        private short _ySuperscriptYOffset;
        private short _yStrikeoutSize;
        private short _yStrikeoutPosition;
        private short _sFamilyClass;
        private Panose _panose;
        private UnicodeRange _unicodeRange;
        private string _achVendId;
        private FsSelection _selection;
        private ushort _firstCharIndex;
        private ushort _lastCharIndex;
        private short _typoAscender;
        private short _typoDescender;
        private short _typoLineGap;
        private ushort _winAscent;
        private ushort _winDescent;
        private CodePageRange _codePageRange;
        private Ttf m_root;
        private DirTableEntry m_parent;

        /// <summary>
        /// The version number for this OS/2 table.
        /// </summary>
        public ushort Version => _version;

        /// <summary>
        /// The Average Character Width parameter specifies the arithmetic average of the escapement (width) of all of the 26 lowercase letters a through z of the Latin alphabet and the space character. If any of the 26 lowercase letters are not present, this parameter should equal the weighted average of all glyphs in the font. For non-UGL (platform 3, encoding 0) fonts, use the unweighted average.
        /// </summary>
        public short XAvgCharWidth => _xAvgCharWidth;

        /// <summary>
        /// Indicates the visual weight (degree of blackness or thickness of strokes) of the characters in the font.
        /// </summary>
        public WeightClass WeightClass => _weightClass;

        /// <summary>
        /// Indicates a relative change from the normal aspect ratio (width to height ratio) as specified by a font designer for the glyphs in a font.
        /// </summary>
        public WidthClass WidthClass => _widthClass;

        /// <summary>
        /// Indicates font embedding licensing rights for the font. Embeddable fonts may be stored in a document. When a document with embedded fonts is opened on a system that does not have the font installed (the remote system), the embedded font may be loaded for temporary (and in some cases, permanent) use on that system by an embedding-aware application. Embedding licensing rights are granted by the vendor of the font.
        /// </summary>
        public FsType FsType => _fsType;

        /// <summary>
        /// The recommended horizontal size in font design units for subscripts for this font.
        /// </summary>
        public short YSubscriptXSize => _ySubscriptXSize;

        /// <summary>
        /// The recommended vertical size in font design units for subscripts for this font.
        /// </summary>
        public short YSubscriptYSize => _ySubscriptYSize;

        /// <summary>
        /// The recommended horizontal offset in font design untis for subscripts for this font.
        /// </summary>
        public short YSubscriptXOffset => _ySubscriptXOffset;

        /// <summary>
        /// The recommended vertical offset in font design units from the baseline for subscripts for this font.
        /// </summary>
        public short YSubscriptYOffset => _ySubscriptYOffset;

        /// <summary>
        /// The recommended horizontal size in font design units for superscripts for this font.
        /// </summary>
        public short YSuperscriptXSize => _ySuperscriptXSize;

        /// <summary>
        /// The recommended vertical size in font design units for superscripts for this font.
        /// </summary>
        public short YSuperscriptYSize => _ySuperscriptYSize;

        /// <summary>
        /// The recommended horizontal offset in font design units for superscripts for this font.
        /// </summary>
        public short YSuperscriptXOffset => _ySuperscriptXOffset;

        /// <summary>
        /// The recommended vertical offset in font design units from the baseline for superscripts for this font.
        /// </summary>
        public short YSuperscriptYOffset => _ySuperscriptYOffset;

        /// <summary>
        /// Width of the strikeout stroke in font design units.
        /// </summary>
        public short YStrikeoutSize => _yStrikeoutSize;

        /// <summary>
        /// The position of the strikeout stroke relative to the baseline in font design units.
        /// </summary>
        public short YStrikeoutPosition => _yStrikeoutPosition;

        /// <summary>
        /// This parameter is a classification of font-family design.
        /// </summary>
        public short SFamilyClass => _sFamilyClass;

        public Panose Panose => _panose;
        public UnicodeRange UnicodeRange => _unicodeRange;

        /// <summary>
        /// The four character identifier for the vendor of the given type face.
        /// </summary>
        public string AchVendId => _achVendId;

        /// <summary>
        /// Contains information concerning the nature of the font patterns
        /// </summary>
        public FsSelection Selection => _selection;

        /// <summary>
        /// The minimum Unicode index (character code) in this font, according to the cmap subtable for platform ID 3 and encoding ID 0 or 1.
        /// </summary>
        public ushort FirstCharIndex => _firstCharIndex;

        /// <summary>
        /// The maximum Unicode index (character code) in this font, according to the cmap subtable for platform ID 3 and encoding ID 0 or 1.
        /// </summary>
        public ushort LastCharIndex => _lastCharIndex;

        /// <summary>
        /// The typographic ascender for this font.
        /// </summary>
        public short TypoAscender => _typoAscender;

        /// <summary>
        /// The typographic descender for this font.
        /// </summary>
        public short TypoDescender => _typoDescender;

        /// <summary>
        /// The typographic line gap for this font.
        /// </summary>
        public short TypoLineGap => _typoLineGap;

        /// <summary>
        /// The ascender metric for Windows.
        /// </summary>
        public ushort WinAscent => _winAscent;

        /// <summary>
        /// The descender metric for Windows.
        /// </summary>
        public ushort WinDescent => _winDescent;

        /// <summary>
        /// This field is used to specify the code pages encompassed by the font file in the `cmap` subtable for platform 3, encoding ID 1 (Microsoft platform).
        /// </summary>
        public CodePageRange CodePageRange => _codePageRange;

        public Ttf M_Root => m_root;
        public DirTableEntry M_Parent => m_parent;
    }
}
