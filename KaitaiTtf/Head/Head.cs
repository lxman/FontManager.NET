using Kaitai;
using KaitaiTtf.Enums;

namespace KaitaiTtf.Head
{
    public class Head : KaitaiStruct
    {
        public static Head FromFile(string fileName)
        {
            return new Head(new KaitaiStream(fileName));
        }

        public Head(KaitaiStream p__io, DirTableEntry p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _version = new Fixed(m_io, this, m_root);
            _fontRevision = new Fixed(m_io, this, m_root);
            _checksumAdjustment = m_io.ReadU4be();
            _magicNumber = m_io.ReadBytes(4);
            if (KaitaiStream.ByteArrayCompare(MagicNumber, new byte[] { 95, 15, 60, 245 }) != 0)
            {
                throw new ValidationNotEqualError(new byte[] { 95, 15, 60, 245 }, MagicNumber, M_Io, "/types/head/seq/3");
            }
            _flags = ((Flags)m_io.ReadU2be());
            _unitsPerEm = m_io.ReadU2be();
            _created = m_io.ReadU8be();
            _modified = m_io.ReadU8be();
            _xMin = m_io.ReadS2be();
            _yMin = m_io.ReadS2be();
            _xMax = m_io.ReadS2be();
            _yMax = m_io.ReadS2be();
            _macStyle = m_io.ReadU2be();
            _lowestRecPpem = m_io.ReadU2be();
            _fontDirectionHint = ((FontDirectionHint)m_io.ReadS2be());
            _indexToLocFormat = m_io.ReadS2be();
            _glyphDataFormat = m_io.ReadS2be();
        }
        private Fixed _version;
        private Fixed _fontRevision;
        private uint _checksumAdjustment;
        private byte[] _magicNumber;
        private Flags _flags;
        private ushort _unitsPerEm;
        private ulong _created;
        private ulong _modified;
        private short _xMin;
        private short _yMin;
        private short _xMax;
        private short _yMax;
        private ushort _macStyle;
        private ushort _lowestRecPpem;
        private FontDirectionHint _fontDirectionHint;
        private short _indexToLocFormat;
        private short _glyphDataFormat;
        private Ttf m_root;
        private DirTableEntry m_parent;
        public Fixed Version => _version;
        public Fixed FontRevision => _fontRevision;
        public uint ChecksumAdjustment => _checksumAdjustment;
        public byte[] MagicNumber => _magicNumber;
        public Flags Flags => _flags;
        public ushort UnitsPerEm => _unitsPerEm;
        public ulong Created => _created;
        public ulong Modified => _modified;
        public short XMin => _xMin;
        public short YMin => _yMin;
        public short XMax => _xMax;
        public short YMax => _yMax;
        public ushort MacStyle => _macStyle;
        public ushort LowestRecPpem => _lowestRecPpem;
        public FontDirectionHint FontDirectionHint => _fontDirectionHint;
        public short IndexToLocFormat => _indexToLocFormat;
        public short GlyphDataFormat => _glyphDataFormat;
        public Ttf M_Root => m_root;
        public DirTableEntry M_Parent => m_parent;
    }
}
