using Kaitai;
using KaitaiTtf;
using KaitaiTtf.Name;

namespace KaitaiName
{
    /// <summary>
    /// Name table is meant to include human-readable string metadata
    /// that describes font: name of the font, its styles, copyright &amp;
    /// trademark notices, vendor and designer info, etc.
    /// 
    /// The table includes a list of &quot;name records&quot;, each of which
    /// corresponds to a single metadata entry.
    /// </summary>
    /// <remarks>
    /// Reference: <a href="https://developer.apple.com/fonts/TrueType-Reference-Manual/RM06/Chap6name.html">Source</a>
    /// </remarks>
    public class Name : KaitaiStruct
    {
        public static Name FromFile(string fileName)
        {
            return new Name(new KaitaiStream(fileName));
        }


        public Name(KaitaiStream p__io, DirTableEntry p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _formatSelector = m_io.ReadU2be();
            _numNameRecords = m_io.ReadU2be();
            _ofsStrings = m_io.ReadU2be();
            _nameRecords = new List<NameRecord>();
            for (var i = 0; i < NumNameRecords; i++)
            {
                _nameRecords.Add(new NameRecord(m_io, this, m_root));
            }
        }
        private ushort _formatSelector;
        private ushort _numNameRecords;
        private ushort _ofsStrings;
        private List<NameRecord> _nameRecords;
        private Ttf m_root;
        private DirTableEntry m_parent;
        public ushort FormatSelector => _formatSelector;
        public ushort NumNameRecords => _numNameRecords;
        public ushort OfsStrings => _ofsStrings;
        public List<NameRecord> NameRecords => _nameRecords;
        public Ttf M_Root => m_root;
        public DirTableEntry M_Parent => m_parent;
    }
}
