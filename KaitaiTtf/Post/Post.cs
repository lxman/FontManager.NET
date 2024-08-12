using Kaitai;

//using static Kaitai.Ttf;

namespace KaitaiTtf.Post
{
    public class Post : KaitaiStruct
    {
        public static Post FromFile(string fileName)
        {
            return new Post(new KaitaiStream(fileName));
        }

        public Post(KaitaiStream p__io, DirTableEntry p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _format = new Fixed(m_io, this, m_root);
            _italicAngle = new Fixed(m_io, this, m_root);
            _underlinePosition = m_io.ReadS2be();
            _underlineThichness = m_io.ReadS2be();
            _isFixedPitch = m_io.ReadU4be();
            _minMemType42 = m_io.ReadU4be();
            _maxMemType42 = m_io.ReadU4be();
            _minMemType1 = m_io.ReadU4be();
            _maxMemType1 = m_io.ReadU4be();
            if (((Format.Major == 2) && (Format.Minor == 0)))
            {
                _format20 = new Format20(m_io, this, m_root);
            }
        }
        private Fixed _format;
        private Fixed _italicAngle;
        private short _underlinePosition;
        private short _underlineThichness;
        private uint _isFixedPitch;
        private uint _minMemType42;
        private uint _maxMemType42;
        private uint _minMemType1;
        private uint _maxMemType1;
        private Format20 _format20;
        private Ttf m_root;
        private DirTableEntry m_parent;
        public Fixed Format => _format;
        public Fixed ItalicAngle => _italicAngle;
        public short UnderlinePosition => _underlinePosition;
        public short UnderlineThichness => _underlineThichness;
        public uint IsFixedPitch => _isFixedPitch;
        public uint MinMemType42 => _minMemType42;
        public uint MaxMemType42 => _maxMemType42;
        public uint MinMemType1 => _minMemType1;
        public uint MaxMemType1 => _maxMemType1;
        public Format20 Format20 => _format20;
        public Ttf M_Root => m_root;
        public DirTableEntry M_Parent => m_parent;
    }
}
