using Kaitai;

namespace KaitaiTtf.Name
{
    public class NameRecord : KaitaiStruct
    {
        public static NameRecord FromFile(string fileName)
        {
            return new NameRecord(new KaitaiStream(fileName));
        }

        public NameRecord(KaitaiStream p__io, KaitaiName.Name p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            f_asciiValue = false;
            f_unicodeValue = false;
            _read();
        }
        private void _read()
        {
            _platformId = ((Platforms)m_io.ReadU2be());
            _encodingId = m_io.ReadU2be();
            _languageId = m_io.ReadU2be();
            _nameId = ((Names)m_io.ReadU2be());
            _lenStr = m_io.ReadU2be();
            _ofsStr = m_io.ReadU2be();
        }
        private bool f_asciiValue;
        private string _asciiValue;
        public string AsciiValue
        {
            get
            {
                if (f_asciiValue)
                    return _asciiValue;
                KaitaiStream io = M_Parent.M_Io;
                long _pos = io.Pos;
                io.Seek((M_Parent.OfsStrings + OfsStr));
                _asciiValue = System.Text.Encoding.GetEncoding("ascii").GetString(io.ReadBytes(LenStr));
                io.Seek(_pos);
                f_asciiValue = true;
                return _asciiValue;
            }
        }
        private bool f_unicodeValue;
        private string _unicodeValue;
        public string UnicodeValue
        {
            get
            {
                if (f_unicodeValue)
                    return _unicodeValue;
                KaitaiStream io = M_Parent.M_Io;
                long _pos = io.Pos;
                io.Seek((M_Parent.OfsStrings + OfsStr));
                _unicodeValue = System.Text.Encoding.GetEncoding("utf-16be").GetString(io.ReadBytes(LenStr));
                io.Seek(_pos);
                f_unicodeValue = true;
                return _unicodeValue;
            }
        }
        private Platforms _platformId;
        private ushort _encodingId;
        private ushort _languageId;
        private Names _nameId;
        private ushort _lenStr;
        private ushort _ofsStr;
        private Ttf m_root;
        private KaitaiName.Name m_parent;
        public Platforms PlatformId => _platformId;
        public ushort EncodingId => _encodingId;
        public ushort LanguageId => _languageId;
        public Names NameId => _nameId;
        public ushort LenStr => _lenStr;
        public ushort OfsStr => _ofsStr;
        public Ttf M_Root => m_root;
        public KaitaiName.Name M_Parent => m_parent;
    }
}
