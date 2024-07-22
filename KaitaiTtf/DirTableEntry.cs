using Kaitai;

namespace KaitaiTtf
{
    public class DirTableEntry : KaitaiStruct
    {
        public static DirTableEntry FromFile(string fileName)
        {
            return new DirTableEntry(new KaitaiStream(fileName));
        }

        public DirTableEntry(KaitaiStream p__io, Ttf p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            f_value = false;
            _read();
        }
        private void _read()
        {
            _tag = System.Text.Encoding.GetEncoding("ascii").GetString(m_io.ReadBytes(4));
            _checksum = m_io.ReadU4be();
            _offset = m_io.ReadU4be();
            _length = m_io.ReadU4be();
        }
        private bool f_value;
        private object _value;
        public object Value
        {
            get
            {
                if (f_value)
                    return _value;
                KaitaiStream io = M_Root.M_Io;
                long _pos = io.Pos;
                io.Seek(Offset);
                switch (Tag)
                {
                    case "head":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new Head.Head(io___raw_value, this, m_root);
                            break;
                        }
                    case "cvt ":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new Cvt(io___raw_value, this, m_root);
                            break;
                        }
                    case "prep":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new Prep(io___raw_value, this, m_root);
                            break;
                        }
                    case "kern":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new KaitaiKern.Kern(io___raw_value, this, m_root);
                            break;
                        }
                    case "hhea":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new Hhea(io___raw_value, this, m_root);
                            break;
                        }
                    case "post":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new Post.Post(io___raw_value, this, m_root);
                            break;
                        }
                    case "OS/2":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new Os2.Os2(io___raw_value, this, m_root);
                            break;
                        }
                    case "name":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new KaitaiName.Name(io___raw_value, this, m_root);
                            break;
                        }
                    case "maxp":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new Maxp(io___raw_value, this, m_root);
                            break;
                        }
                    case "glyf":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new Glyf.Glyf(io___raw_value, this, m_root);
                            break;
                        }
                    case "fpgm":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new Fpgm(io___raw_value, this, m_root);
                            break;
                        }
                    case "cmap":
                        {
                            __raw_value = io.ReadBytes(Length);
                            var io___raw_value = new KaitaiStream(__raw_value);
                            _value = new KaitaiCmap.Cmap(io___raw_value, this, m_root);
                            break;
                        }
                    default:
                        {
                            _value = io.ReadBytes(Length);
                            break;
                        }
                }
                io.Seek(_pos);
                f_value = true;
                return _value;
            }
        }
        private string _tag;
        private uint _checksum;
        private uint _offset;
        private uint _length;
        private Ttf m_root;
        private Ttf m_parent;
        private byte[] __raw_value;
        public string Tag => _tag;
        public uint Checksum => _checksum;
        public uint Offset => _offset;
        public uint Length => _length;
        public Ttf M_Root => m_root;
        public Ttf M_Parent => m_parent;
        public byte[] M_RawValue => __raw_value;
    }
}
