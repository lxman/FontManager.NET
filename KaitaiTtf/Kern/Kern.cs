using Kaitai;
using KaitaiTtf;
using KaitaiTtf.Kern.Subtable;

namespace KaitaiKern
{
    public class Kern : KaitaiStruct
    {
        public static Kern FromFile(string fileName)
        {
            return new Kern(new KaitaiStream(fileName));
        }

        public Kern(KaitaiStream p__io, DirTableEntry p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _version = m_io.ReadU2be();
            _subtableCount = m_io.ReadU2be();
            _subtables = new List<Subtable>();
            for (var i = 0; i < SubtableCount; i++)
            {
                _subtables.Add(new Subtable(m_io, this, m_root));
            }
        }
        private ushort _version;
        private ushort _subtableCount;
        private List<Subtable> _subtables;
        private Ttf m_root;
        private DirTableEntry m_parent;
        public ushort Version => _version;
        public ushort SubtableCount => _subtableCount;
        public List<Subtable> Subtables => _subtables;
        public Ttf M_Root => m_root;
        public DirTableEntry M_Parent => m_parent;
    }
}
