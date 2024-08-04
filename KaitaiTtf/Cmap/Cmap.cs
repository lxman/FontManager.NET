using System.Collections.Generic;
using Kaitai;
using KaitaiTtf.Cmap.SubtableHeader;

namespace KaitaiCmap
{
    /// <summary>
    /// cmap - Character To Glyph Index Mapping Table This table defines the mapping of character codes to the glyph index values used in the font.
    /// </summary>
    public class Cmap : KaitaiStruct
    {
        public static Cmap FromFile(string fileName)
        {
            return new Cmap(new KaitaiStream(fileName));
        }

        public Cmap(KaitaiStream p__io, KaitaiTtf.DirTableEntry p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _versionNumber = m_io.ReadU2be();
            _numberOfEncodingTables = m_io.ReadU2be();
            _tables = new List<SubtableHeader>();
            for (var i = 0; i < NumberOfEncodingTables; i++)
            {
                _tables.Add(new SubtableHeader(m_io, this, m_root));
            }
        }
        private ushort _versionNumber;
        private ushort _numberOfEncodingTables;
        private List<SubtableHeader> _tables;
        private Ttf m_root;
        private KaitaiTtf.DirTableEntry m_parent;
        public ushort VersionNumber => _versionNumber;
        public ushort NumberOfEncodingTables => _numberOfEncodingTables;
        public List<SubtableHeader> Tables => _tables;
        public Ttf M_Root => m_root;
        public KaitaiTtf.DirTableEntry M_Parent => m_parent;
    }
}
