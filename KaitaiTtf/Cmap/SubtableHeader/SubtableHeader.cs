using Kaitai;

namespace KaitaiTtf.Cmap.SubtableHeader
{
    public class SubtableHeader : KaitaiStruct
    {
        public static SubtableHeader FromFile(string fileName)
        {
            return new SubtableHeader(new KaitaiStream(fileName));
        }

        public SubtableHeader(KaitaiStream p__io, Cmap p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            f_table = false;
            _read();
        }
        private void _read()
        {
            _platformId = m_io.ReadU2be();
            _encodingId = m_io.ReadU2be();
            _subtableOffset = m_io.ReadU4be();
        }
        private bool f_table;
        private Subtable.Subtable _table;
        public Subtable.Subtable Table
        {
            get
            {
                if (f_table)
                    return _table;
                KaitaiStream io = M_Parent.M_Io;
                long _pos = io.Pos;
                io.Seek(SubtableOffset);
                _table = new Subtable.Subtable(io, this, m_root);
                io.Seek(_pos);
                f_table = true;
                return _table;
            }
        }
        private ushort _platformId;
        private ushort _encodingId;
        private uint _subtableOffset;
        private Ttf m_root;
        private Cmap m_parent;
        public ushort PlatformId => _platformId;
        public ushort EncodingId => _encodingId;
        public uint SubtableOffset => _subtableOffset;
        public Ttf M_Root => m_root;
        public Cmap M_Parent => m_parent;
    }
}
