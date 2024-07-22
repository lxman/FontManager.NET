using Kaitai;

namespace KaitaiTtf.Cmap.Subtable
{
    public class ByteEncodingTable : KaitaiStruct
    {
        public static ByteEncodingTable FromFile(string fileName)
        {
            return new ByteEncodingTable(new KaitaiStream(fileName));
        }

        public ByteEncodingTable(KaitaiStream p__io, Subtable p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _glyphIdArray = m_io.ReadBytes(256);
        }
        private byte[] _glyphIdArray;
        private Ttf m_root;
        private Subtable m_parent;
        public byte[] GlyphIdArray => _glyphIdArray;
        public Ttf M_Root => m_root;
        public Subtable M_Parent => m_parent;
    }
}
