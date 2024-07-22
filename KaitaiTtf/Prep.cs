using Kaitai;

namespace KaitaiTtf
{
    public class Prep : KaitaiStruct
    {
        public static Prep FromFile(string fileName)
        {
            return new Prep(new KaitaiStream(fileName));
        }

        public Prep(KaitaiStream p__io, DirTableEntry p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _instructions = m_io.ReadBytesFull();
        }
        private byte[] _instructions;
        private Ttf m_root;
        private DirTableEntry m_parent;
        public byte[] Instructions => _instructions;
        public Ttf M_Root => m_root;
        public DirTableEntry M_Parent => m_parent;
    }
}
