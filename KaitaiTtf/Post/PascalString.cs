using Kaitai;

namespace KaitaiTtf.Post
{
    public class PascalString : KaitaiStruct
    {
        public static PascalString FromFile(string fileName)
        {
            return new PascalString(new KaitaiStream(fileName));
        }

        public PascalString(KaitaiStream p__io, Format20 p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _length = m_io.ReadU1();
            if (Length != 0)
            {
                _value = System.Text.Encoding.GetEncoding("ascii").GetString(m_io.ReadBytes(Length));
            }
        }
        private byte _length;
        private string _value;
        private Ttf m_root;
        private Format20 m_parent;
        public byte Length => _length;
        public string Value => _value;
        public Ttf M_Root => m_root;
        public Format20 M_Parent => m_parent;
    }
}
