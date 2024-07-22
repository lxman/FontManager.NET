using Kaitai;

namespace KaitaiTtf
{
    public class Hhea : KaitaiStruct
    {
        public static Hhea FromFile(string fileName)
        {
            return new Hhea(new KaitaiStream(fileName));
        }

        public Hhea(KaitaiStream p__io, DirTableEntry p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _version = new Fixed(m_io, this, m_root);
            _ascender = m_io.ReadS2be();
            _descender = m_io.ReadS2be();
            _lineGap = m_io.ReadS2be();
            _advanceWidthMax = m_io.ReadU2be();
            _minLeftSideBearing = m_io.ReadS2be();
            _minRightSideBearing = m_io.ReadS2be();
            _xMaxExtend = m_io.ReadS2be();
            _caretSlopeRise = m_io.ReadS2be();
            _caretSlopeRun = m_io.ReadS2be();
            _reserved = m_io.ReadBytes(10);
            if (KaitaiStream.ByteArrayCompare(Reserved, [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]) != 0)
            {
                throw new ValidationNotEqualError([0, 0, 0, 0, 0, 0, 0, 0, 0, 0], Reserved, M_Io, "/types/hhea/seq/10");
            }
            _metricDataFormat = m_io.ReadS2be();
            _numberOfHmetrics = m_io.ReadU2be();
        }
        private Fixed _version;
        private short _ascender;
        private short _descender;
        private short _lineGap;
        private ushort _advanceWidthMax;
        private short _minLeftSideBearing;
        private short _minRightSideBearing;
        private short _xMaxExtend;
        private short _caretSlopeRise;
        private short _caretSlopeRun;
        private byte[] _reserved;
        private short _metricDataFormat;
        private ushort _numberOfHmetrics;
        private Ttf m_root;
        private DirTableEntry m_parent;
        public Fixed Version => _version;

        /// <summary>
        /// Typographic ascent
        /// </summary>
        public short Ascender => _ascender;

        /// <summary>
        /// Typographic descent
        /// </summary>
        public short Descender => _descender;

        /// <summary>
        /// Typographic line gap. Negative LineGap values are treated as zero in Windows 3.1, System 6, and System 7.
        /// </summary>
        public short LineGap => _lineGap;

        /// <summary>
        /// Maximum advance width value in `hmtx` table.
        /// </summary>
        public ushort AdvanceWidthMax => _advanceWidthMax;

        /// <summary>
        /// Minimum left sidebearing value in `hmtx` table.
        /// </summary>
        public short MinLeftSideBearing => _minLeftSideBearing;

        /// <summary>
        /// Minimum right sidebearing value; calculated as Min(aw - lsb - (xMax - xMin)).
        /// </summary>
        public short MinRightSideBearing => _minRightSideBearing;

        /// <summary>
        /// Max(lsb + (xMax - xMin)).
        /// </summary>
        public short XMaxExtend => _xMaxExtend;

        public short CaretSlopeRise => _caretSlopeRise;
        public short CaretSlopeRun => _caretSlopeRun;
        public byte[] Reserved => _reserved;
        public short MetricDataFormat => _metricDataFormat;
        public ushort NumberOfHmetrics => _numberOfHmetrics;
        public Ttf M_Root => m_root;
        public DirTableEntry M_Parent => m_parent;
    }
}
