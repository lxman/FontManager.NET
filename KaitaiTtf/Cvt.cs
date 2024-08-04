using System.Collections.Generic;
using Kaitai;

namespace KaitaiTtf
{
    /// <summary>
    /// cvt  - Control Value Table This table contains a list of values that can be referenced by instructions. They can be used, among other things, to control characteristics for different glyphs.
    /// </summary>
    public class Cvt : KaitaiStruct
    {
        public static Cvt FromFile(string fileName)
        {
            return new Cvt(new KaitaiStream(fileName));
        }

        public Cvt(KaitaiStream p__io, DirTableEntry p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _fwords = new List<short>();
            {
                var i = 0;
                while (!m_io.IsEof)
                {
                    _fwords.Add(m_io.ReadS2be());
                    i++;
                }
            }
        }
        private List<short> _fwords;
        private Ttf m_root;
        private DirTableEntry m_parent;
        public List<short> Fwords => _fwords;
        public Ttf M_Root => m_root;
        public DirTableEntry M_Parent => m_parent;
    }
}
