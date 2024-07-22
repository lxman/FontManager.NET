using Kaitai;

namespace KaitaiTtf.Kern.Subtable.Format0.KerningPair
{
    public class KerningPair : KaitaiStruct
    {
        public static KerningPair FromFile(string fileName)
        {
            return new KerningPair(new KaitaiStream(fileName));
        }

        public KerningPair(KaitaiStream p__io, Format0 p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _left = m_io.ReadU2be();
            _right = m_io.ReadU2be();
            _value = m_io.ReadS2be();
        }
        private ushort _left;
        private ushort _right;
        private short _value;
        private Ttf m_root;
        private Format0 m_parent;
        public ushort Left => _left;
        public ushort Right => _right;
        public short Value => _value;
        public Ttf M_Root => m_root;
        public Format0 M_Parent => m_parent;
    }
}
