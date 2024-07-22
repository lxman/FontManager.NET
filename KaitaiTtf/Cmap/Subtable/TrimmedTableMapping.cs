using Kaitai;

namespace KaitaiTtf.Cmap.Subtable
{
    public class TrimmedTableMapping : KaitaiStruct
    {
        public static TrimmedTableMapping FromFile(string fileName)
        {
            return new TrimmedTableMapping(new KaitaiStream(fileName));
        }

        public TrimmedTableMapping(KaitaiStream p__io, Subtable p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _firstCode = m_io.ReadU2be();
            _entryCount = m_io.ReadU2be();
            _glyphIdArray = new List<ushort>();
            for (var i = 0; i < EntryCount; i++)
            {
                _glyphIdArray.Add(m_io.ReadU2be());
            }
        }
        private ushort _firstCode;
        private ushort _entryCount;
        private List<ushort> _glyphIdArray;
        private Ttf m_root;
        private Subtable m_parent;
        public ushort FirstCode => _firstCode;
        public ushort EntryCount => _entryCount;
        public List<ushort> GlyphIdArray => _glyphIdArray;
        public Ttf M_Root => m_root;
        public Subtable M_Parent => m_parent;
    }
}
