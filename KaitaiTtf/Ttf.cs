using KaitaiTtf;

namespace Kaitai
{

    /// <summary>
    /// A TrueType font file contains data, in table format, that comprises
    /// an outline font.
    /// </summary>
    /// <remarks>
    /// Reference: <a href="https://web.archive.org/web/20160410081432/https://www.microsoft.com/typography/tt/ttf_spec/ttch02.doc">Source</a>
    /// </remarks>
    public class Ttf : KaitaiStruct
    {
        public static Ttf FromFile(string fileName)
        {
            return new Ttf(new KaitaiStream(fileName));
        }

        public Ttf(KaitaiStream p__io, KaitaiStruct p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _offsetTable = new OffsetTable(m_io, this, m_root);
            _directoryTable = new List<DirTableEntry>();
            for (var i = 0; i < OffsetTable.NumTables; i++)
            {
                _directoryTable.Add(new DirTableEntry(m_io, this, m_root));
            }
        }

        private OffsetTable _offsetTable;
        private List<DirTableEntry> _directoryTable;
        private Ttf m_root;
        private KaitaiStruct m_parent;
        public OffsetTable OffsetTable => _offsetTable;
        public List<DirTableEntry> DirectoryTable => _directoryTable;
        public Ttf M_Root => m_root;
        public KaitaiStruct M_Parent => m_parent;
    }
}
