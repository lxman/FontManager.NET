using Kaitai;

namespace KaitaiTtf
{
    public class Maxp : KaitaiStruct
    {
        public static Maxp FromFile(string fileName)
        {
            return new Maxp(new KaitaiStream(fileName));
        }

        public Maxp(KaitaiStream p__io, DirTableEntry p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            f_isVersion10 = false;
            _read();
        }
        private void _read()
        {
            _tableVersionNumber = new Fixed(m_io, this, m_root);
            _numGlyphs = m_io.ReadU2be();
            if (IsVersion10)
            {
                _version10Body = new MaxpVersion10Body(m_io, this, m_root);
            }
        }
        private bool f_isVersion10;
        private bool _isVersion10;
        public bool IsVersion10
        {
            get
            {
                if (f_isVersion10)
                    return _isVersion10;
                _isVersion10 = (TableVersionNumber.Major == 1) && (TableVersionNumber.Minor == 0);
                f_isVersion10 = true;
                return _isVersion10;
            }
        }
        private Fixed _tableVersionNumber;
        private ushort _numGlyphs;
        private MaxpVersion10Body _version10Body;
        private Ttf m_root;
        private DirTableEntry m_parent;

        /// <summary>
        /// 0x00010000 for version 1.0.
        /// </summary>
        public Fixed TableVersionNumber => _tableVersionNumber;

        /// <summary>
        /// The number of glyphs in the font.
        /// </summary>
        public ushort NumGlyphs => _numGlyphs;

        public MaxpVersion10Body Version10Body => _version10Body;
        public Ttf M_Root => m_root;
        public DirTableEntry M_Parent => m_parent;
    }
}
