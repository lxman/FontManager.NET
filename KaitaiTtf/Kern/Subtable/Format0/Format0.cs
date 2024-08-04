using System.Collections.Generic;
using Kaitai;

namespace KaitaiTtf.Kern.Subtable.Format0
{
    public class Format0 : KaitaiStruct
    {
        public static Format0 FromFile(string fileName)
        {
            return new Format0(new KaitaiStream(fileName));
        }

        public Format0(KaitaiStream p__io, Subtable p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _pairCount = m_io.ReadU2be();
            _searchRange = m_io.ReadU2be();
            _entrySelector = m_io.ReadU2be();
            _rangeShift = m_io.ReadU2be();
            _kerningPairs = new List<KerningPair.KerningPair>();
            for (var i = 0; i < PairCount; i++)
            {
                _kerningPairs.Add(new KerningPair.KerningPair(m_io, this, m_root));
            }
        }
        private ushort _pairCount;
        private ushort _searchRange;
        private ushort _entrySelector;
        private ushort _rangeShift;
        private List<KerningPair.KerningPair> _kerningPairs;
        private Ttf m_root;
        private Subtable m_parent;
        public ushort PairCount => _pairCount;
        public ushort SearchRange => _searchRange;
        public ushort EntrySelector => _entrySelector;
        public ushort RangeShift => _rangeShift;
        public List<KerningPair.KerningPair> KerningPairs => _kerningPairs;
        public Ttf M_Root => m_root;
        public Subtable M_Parent => m_parent;
    }
}
