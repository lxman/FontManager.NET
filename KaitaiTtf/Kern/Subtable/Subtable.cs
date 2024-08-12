using Kaitai;

namespace KaitaiTtf.Kern.Subtable
{
    public class Subtable : KaitaiStruct
    {
        public static Subtable FromFile(string fileName)
        {
            return new Subtable(new KaitaiStream(fileName));
        }

        public Subtable(KaitaiStream p__io, Kern p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _version = m_io.ReadU2be();
            _length = m_io.ReadU2be();
            _format = m_io.ReadU1();
            _reserved = m_io.ReadBitsIntBe(4);
            _isOverride = m_io.ReadBitsIntBe(1) != 0;
            _isCrossStream = m_io.ReadBitsIntBe(1) != 0;
            _isMinimum = m_io.ReadBitsIntBe(1) != 0;
            _isHorizontal = m_io.ReadBitsIntBe(1) != 0;
            m_io.AlignToByte();
            if (Format == 0)
            {
                _format0 = new Format0.Format0(m_io, this, m_root);
            }
        }
        private ushort _version;
        private ushort _length;
        private byte _format;
        private ulong _reserved;
        private bool _isOverride;
        private bool _isCrossStream;
        private bool _isMinimum;
        private bool _isHorizontal;
        private Format0.Format0 _format0;
        private Ttf m_root;
        private Kern m_parent;
        public ushort Version => _version;
        public ushort Length => _length;
        public byte Format => _format;
        public ulong Reserved => _reserved;
        public bool IsOverride => _isOverride;
        public bool IsCrossStream => _isCrossStream;
        public bool IsMinimum => _isMinimum;
        public bool IsHorizontal => _isHorizontal;
        public Format0.Format0 Format0 => _format0;
        public Ttf M_Root => m_root;
        public Kern M_Parent => m_parent;
    }
}
