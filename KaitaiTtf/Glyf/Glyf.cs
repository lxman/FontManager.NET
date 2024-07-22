using Kaitai;

namespace KaitaiTtf.Glyf
{
    public class Glyf : KaitaiStruct
    {
        public static Glyf FromFile(string fileName)
        {
            return new Glyf(new KaitaiStream(fileName));
        }

        public Glyf(KaitaiStream p__io, DirTableEntry p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _numberOfContours = m_io.ReadS2be();
            _xMin = m_io.ReadS2be();
            _yMin = m_io.ReadS2be();
            _xMax = m_io.ReadS2be();
            _yMax = m_io.ReadS2be();
            if (NumberOfContours > 0)
            {
                _value = new SimpleGlyph.SimpleGlyph(m_io, this, m_root);
            }
        }
        private short _numberOfContours;
        private short _xMin;
        private short _yMin;
        private short _xMax;
        private short _yMax;
        private SimpleGlyph.SimpleGlyph _value;
        private Ttf m_root;
        private DirTableEntry m_parent;
        public short NumberOfContours => _numberOfContours;
        public short XMin => _xMin;
        public short YMin => _yMin;
        public short XMax => _xMax;
        public short YMax => _yMax;
        public SimpleGlyph.SimpleGlyph Value => _value;
        public Ttf M_Root => m_root;
        public DirTableEntry M_Parent => m_parent;
    }
}
