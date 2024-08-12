using Kaitai;
using KaitaiTtf.Enums;

namespace KaitaiTtf.Os2
{
    public class Panose : KaitaiStruct
    {
        public static Panose FromFile(string fileName)
        {
            return new Panose(new KaitaiStream(fileName));
        }

        public Panose(KaitaiStream p__io, Os2 p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _familyType = ((FamilyKind)m_io.ReadU1());
            _serifStyle = ((SerifStyle)m_io.ReadU1());
            _weight = ((Weight)m_io.ReadU1());
            _proportion = ((Proportion)m_io.ReadU1());
            _contrast = ((Contrast)m_io.ReadU1());
            _strokeVariation = ((StrokeVariation)m_io.ReadU1());
            _armStyle = ((ArmStyle)m_io.ReadU1());
            _letterForm = ((LetterForm)m_io.ReadU1());
            _midline = ((Midline)m_io.ReadU1());
            _xHeight = ((XHeight)m_io.ReadU1());
        }
        private FamilyKind _familyType;
        private SerifStyle _serifStyle;
        private Weight _weight;
        private Proportion _proportion;
        private Contrast _contrast;
        private StrokeVariation _strokeVariation;
        private ArmStyle _armStyle;
        private LetterForm _letterForm;
        private Midline _midline;
        private XHeight _xHeight;
        private Ttf m_root;
        private Os2 m_parent;
        public FamilyKind FamilyType => _familyType;
        public SerifStyle SerifStyle => _serifStyle;
        public Weight Weight => _weight;
        public Proportion Proportion => _proportion;
        public Contrast Contrast => _contrast;
        public StrokeVariation StrokeVariation => _strokeVariation;
        public ArmStyle ArmStyle => _armStyle;
        public LetterForm LetterForm => _letterForm;
        public Midline Midline => _midline;
        public XHeight XHeight => _xHeight;
        public Ttf M_Root => m_root;
        public Os2 M_Parent => m_parent;
    }
}
