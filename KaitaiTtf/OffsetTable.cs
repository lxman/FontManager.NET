using Kaitai;

namespace KaitaiTtf
{
    public class OffsetTable : KaitaiStruct
    {
        public static OffsetTable FromFile(string fileName)
        {
            return new OffsetTable(new KaitaiStream(fileName));
        }

        public OffsetTable(KaitaiStream p__io, Ttf p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _sfntVersion = new Fixed(m_io, this, m_root);
            _numTables = m_io.ReadU2be();
            _searchRange = m_io.ReadU2be();
            _entrySelector = m_io.ReadU2be();
            _rangeShift = m_io.ReadU2be();
        }
        private Fixed _sfntVersion;
        private ushort _numTables;
        private ushort _searchRange;
        private ushort _entrySelector;
        private ushort _rangeShift;
        private Ttf m_root;
        private Ttf m_parent;
        public Fixed SfntVersion => _sfntVersion;
        public ushort NumTables => _numTables;
        public ushort SearchRange => _searchRange;
        public ushort EntrySelector => _entrySelector;
        public ushort RangeShift => _rangeShift;
        public Ttf M_Root => m_root;
        public Ttf M_Parent => m_parent;
    }
}
