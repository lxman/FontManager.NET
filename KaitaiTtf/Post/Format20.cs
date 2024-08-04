using System.Collections.Generic;
using Kaitai;

namespace KaitaiTtf.Post
{
    public class Format20 : KaitaiStruct
    {
        public static Format20 FromFile(string fileName)
        {
            return new Format20(new KaitaiStream(fileName));
        }

        public Format20(KaitaiStream p__io, Post p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _numberOfGlyphs = m_io.ReadU2be();
            _glyphNameIndex = new List<ushort>();
            for (var i = 0; i < NumberOfGlyphs; i++)
            {
                _glyphNameIndex.Add(m_io.ReadU2be());
            }
            _glyphNames = new List<PascalString>();
            {
                var i = 0;
                PascalString M_;
                do
                {
                    M_ = new PascalString(m_io, this, m_root);
                    _glyphNames.Add(M_);
                    i++;
                } while (!(((M_.Length == 0) || (M_Io.IsEof))));
            }
        }
        private ushort _numberOfGlyphs;
        private List<ushort> _glyphNameIndex;
        private List<PascalString> _glyphNames;
        private Ttf m_root;
        private Post m_parent;
        public ushort NumberOfGlyphs => _numberOfGlyphs;
        public List<ushort> GlyphNameIndex => _glyphNameIndex;
        public List<PascalString> GlyphNames => _glyphNames;
        public Ttf M_Root => m_root;
        public Post M_Parent => m_parent;
    }
}
