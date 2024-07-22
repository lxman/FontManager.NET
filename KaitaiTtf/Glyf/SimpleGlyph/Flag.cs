using Kaitai;

namespace KaitaiTtf.Glyf.SimpleGlyph
{
    public class Flag : KaitaiStruct
    {
        public static Flag FromFile(string fileName)
        {
            return new Flag(new KaitaiStream(fileName));
        }

        public Flag(KaitaiStream p__io, SimpleGlyph p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _reserved = m_io.ReadBitsIntBe(2);
            _yIsSame = m_io.ReadBitsIntBe(1) != 0;
            _xIsSame = m_io.ReadBitsIntBe(1) != 0;
            _repeat = m_io.ReadBitsIntBe(1) != 0;
            _yShortVector = m_io.ReadBitsIntBe(1) != 0;
            _xShortVector = m_io.ReadBitsIntBe(1) != 0;
            _onCurve = m_io.ReadBitsIntBe(1) != 0;
            m_io.AlignToByte();
            if (Repeat)
            {
                _repeatValue = m_io.ReadU1();
            }
        }
        private ulong _reserved;
        private bool _yIsSame;
        private bool _xIsSame;
        private bool _repeat;
        private bool _yShortVector;
        private bool _xShortVector;
        private bool _onCurve;
        private byte? _repeatValue;
        private Ttf m_root;
        private SimpleGlyph m_parent;
        public ulong Reserved => _reserved;
        public bool YIsSame => _yIsSame;
        public bool XIsSame => _xIsSame;
        public bool Repeat => _repeat;
        public bool YShortVector => _yShortVector;
        public bool XShortVector => _xShortVector;
        public bool OnCurve => _onCurve;
        public byte? RepeatValue => _repeatValue;
        public Ttf M_Root => m_root;
        public SimpleGlyph M_Parent => m_parent;
    }
}
