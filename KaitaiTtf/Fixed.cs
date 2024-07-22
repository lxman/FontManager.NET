using Kaitai;

namespace KaitaiTtf
{
    public class Fixed : KaitaiStruct
    {
        public static Fixed FromFile(string fileName)
        {
            return new Fixed(new KaitaiStream(fileName));
        }

        public Fixed(KaitaiStream p__io, KaitaiStruct p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _major = m_io.ReadU2be();
            _minor = m_io.ReadU2be();
        }
        private ushort _major;
        private ushort _minor;
        private Ttf m_root;
        private KaitaiStruct m_parent;
        public ushort Major => _major;
        public ushort Minor => _minor;
        public Ttf M_Root => m_root;
        public KaitaiStruct M_Parent => m_parent;
    }
}
