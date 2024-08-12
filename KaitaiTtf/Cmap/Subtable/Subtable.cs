using Kaitai;
using KaitaiTtf.Enums;

namespace KaitaiTtf.Cmap.Subtable
{
    public class Subtable : KaitaiStruct
    {
        public static Subtable FromFile(string fileName)
        {
            return new Subtable(new KaitaiStream(fileName));
        }

        public Subtable(KaitaiStream p__io, SubtableHeader.SubtableHeader p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _format = ((SubtableFormat)m_io.ReadU2be());
            _length = m_io.ReadU2be();
            _version = m_io.ReadU2be();
            switch (Format)
            {
                case SubtableFormat.ByteEncodingTable:
                    {
                        __raw_value = m_io.ReadBytes((Length - 6));
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new ByteEncodingTable(io___raw_value, this, m_root);
                        break;
                    }
                case SubtableFormat.SegmentMappingToDeltaValues:
                    {
                        __raw_value = m_io.ReadBytes((Length - 6));
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new SegmentMappingToDeltaValues(io___raw_value, this, m_root);
                        break;
                    }
                case SubtableFormat.HighByteMappingThroughTable:
                    {
                        __raw_value = m_io.ReadBytes((Length - 6));
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new HighByteMappingThroughTable(io___raw_value, this, m_root);
                        break;
                    }
                case SubtableFormat.TrimmedTableMapping:
                    {
                        __raw_value = m_io.ReadBytes((Length - 6));
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new TrimmedTableMapping(io___raw_value, this, m_root);
                        break;
                    }
                default:
                    {
                        _value = m_io.ReadBytes((Length - 6));
                        break;
                    }
            }
        }
        private SubtableFormat _format;
        private ushort _length;
        private ushort _version;
        private object _value;
        private Ttf m_root;
        private SubtableHeader.SubtableHeader m_parent;
        private byte[] __raw_value;
        public SubtableFormat Format => _format;
        public ushort Length => _length;
        public ushort Version => _version;
        public object Value => _value;
        public Ttf M_Root => m_root;
        public SubtableHeader.SubtableHeader M_Parent => m_parent;
        public byte[] M_RawValue => __raw_value;
    }
}
