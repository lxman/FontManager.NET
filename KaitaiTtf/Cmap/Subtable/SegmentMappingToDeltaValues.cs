using Kaitai;

namespace KaitaiTtf.Cmap.Subtable
{
    public class SegmentMappingToDeltaValues : KaitaiStruct
    {
        public static SegmentMappingToDeltaValues FromFile(string fileName)
        {
            return new SegmentMappingToDeltaValues(new KaitaiStream(fileName));
        }

        public SegmentMappingToDeltaValues(KaitaiStream p__io, Subtable p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            f_segCount = false;
            _read();
        }
        private void _read()
        {
            _segCountX2 = m_io.ReadU2be();
            _searchRange = m_io.ReadU2be();
            _entrySelector = m_io.ReadU2be();
            _rangeShift = m_io.ReadU2be();
            _endCount = new List<ushort>();
            for (var i = 0; i < SegCount; i++)
            {
                _endCount.Add(m_io.ReadU2be());
            }
            _reservedPad = m_io.ReadU2be();
            _startCount = new List<ushort>();
            for (var i = 0; i < SegCount; i++)
            {
                _startCount.Add(m_io.ReadU2be());
            }
            _idDelta = new List<ushort>();
            for (var i = 0; i < SegCount; i++)
            {
                _idDelta.Add(m_io.ReadU2be());
            }
            _idRangeOffset = new List<ushort>();
            for (var i = 0; i < SegCount; i++)
            {
                _idRangeOffset.Add(m_io.ReadU2be());
            }
            _glyphIdArray = new List<ushort>();
            {
                var i = 0;
                while (!m_io.IsEof)
                {
                    _glyphIdArray.Add(m_io.ReadU2be());
                    i++;
                }
            }
        }
        private bool f_segCount;
        private int _segCount;
        public int SegCount
        {
            get
            {
                if (f_segCount)
                    return _segCount;
                _segCount = (int)((SegCountX2 / 2));
                f_segCount = true;
                return _segCount;
            }
        }
        private ushort _segCountX2;
        private ushort _searchRange;
        private ushort _entrySelector;
        private ushort _rangeShift;
        private List<ushort> _endCount;
        private ushort _reservedPad;
        private List<ushort> _startCount;
        private List<ushort> _idDelta;
        private List<ushort> _idRangeOffset;
        private List<ushort> _glyphIdArray;
        private Ttf m_root;
        private Subtable m_parent;
        public ushort SegCountX2 => _segCountX2;
        public ushort SearchRange => _searchRange;
        public ushort EntrySelector => _entrySelector;
        public ushort RangeShift => _rangeShift;
        public List<ushort> EndCount => _endCount;
        public ushort ReservedPad => _reservedPad;
        public List<ushort> StartCount => _startCount;
        public List<ushort> IdDelta => _idDelta;
        public List<ushort> IdRangeOffset => _idRangeOffset;
        public List<ushort> GlyphIdArray => _glyphIdArray;
        public Ttf M_Root => m_root;
        public Subtable M_Parent => m_parent;
    }
}
