using Kaitai;

namespace KaitaiTtf.Cmap.Subtable
{
    public class HighByteMappingThroughTable : KaitaiStruct
    {
        public static HighByteMappingThroughTable FromFile(string fileName)
        {
            return new HighByteMappingThroughTable(new KaitaiStream(fileName));
        }

        public HighByteMappingThroughTable(KaitaiStream p__io, Subtable p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _subHeaderKeys = new List<ushort>();
            for (var i = 0; i < 256; i++)
            {
                _subHeaderKeys.Add(m_io.ReadU2be());
            }
        }
        private List<ushort> _subHeaderKeys;
        private Ttf m_root;
        private Subtable m_parent;
        public List<ushort> SubHeaderKeys => _subHeaderKeys;
        public Ttf M_Root => m_root;
        public Subtable M_Parent => m_parent;
    }
}
