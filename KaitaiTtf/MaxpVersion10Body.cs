using Kaitai;

namespace KaitaiTtf
{
    public class MaxpVersion10Body : KaitaiStruct
    {
        public static MaxpVersion10Body FromFile(string fileName)
        {
            return new MaxpVersion10Body(new KaitaiStream(fileName));
        }

        public MaxpVersion10Body(KaitaiStream p__io, Maxp p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _maxPoints = m_io.ReadU2be();
            _maxContours = m_io.ReadU2be();
            _maxCompositePoints = m_io.ReadU2be();
            _maxCompositeContours = m_io.ReadU2be();
            _maxZones = m_io.ReadU2be();
            _maxTwilightPoints = m_io.ReadU2be();
            _maxStorage = m_io.ReadU2be();
            _maxFunctionDefs = m_io.ReadU2be();
            _maxInstructionDefs = m_io.ReadU2be();
            _maxStackElements = m_io.ReadU2be();
            _maxSizeOfInstructions = m_io.ReadU2be();
            _maxComponentElements = m_io.ReadU2be();
            _maxComponentDepth = m_io.ReadU2be();
        }
        private ushort _maxPoints;
        private ushort _maxContours;
        private ushort _maxCompositePoints;
        private ushort _maxCompositeContours;
        private ushort _maxZones;
        private ushort _maxTwilightPoints;
        private ushort _maxStorage;
        private ushort _maxFunctionDefs;
        private ushort _maxInstructionDefs;
        private ushort _maxStackElements;
        private ushort _maxSizeOfInstructions;
        private ushort _maxComponentElements;
        private ushort _maxComponentDepth;
        private Ttf m_root;
        private Maxp m_parent;

        /// <summary>
        /// Maximum points in a non-composite glyph.
        /// </summary>
        public ushort MaxPoints => _maxPoints;

        /// <summary>
        /// Maximum contours in a non-composite glyph.
        /// </summary>
        public ushort MaxContours => _maxContours;

        /// <summary>
        /// Maximum points in a composite glyph.
        /// </summary>
        public ushort MaxCompositePoints => _maxCompositePoints;

        /// <summary>
        /// Maximum contours in a composite glyph.
        /// </summary>
        public ushort MaxCompositeContours => _maxCompositeContours;

        /// <summary>
        /// 1 if instructions do not use the twilight zone (Z0), or 2 if instructions do use Z0; should be set to 2 in most cases.
        /// </summary>
        public ushort MaxZones => _maxZones;

        /// <summary>
        /// Maximum points used in Z0.
        /// </summary>
        public ushort MaxTwilightPoints => _maxTwilightPoints;

        /// <summary>
        /// Number of Storage Area locations.
        /// </summary>
        public ushort MaxStorage => _maxStorage;

        /// <summary>
        /// Number of FDEFs.
        /// </summary>
        public ushort MaxFunctionDefs => _maxFunctionDefs;

        /// <summary>
        /// Number of IDEFs.
        /// </summary>
        public ushort MaxInstructionDefs => _maxInstructionDefs;

        /// <summary>
        /// Maximum stack depth.
        /// </summary>
        public ushort MaxStackElements => _maxStackElements;

        /// <summary>
        /// Maximum byte count for glyph instructions.
        /// </summary>
        public ushort MaxSizeOfInstructions => _maxSizeOfInstructions;

        /// <summary>
        /// Maximum number of components referenced at &quot;top level&quot; for any composite glyph.
        /// </summary>
        public ushort MaxComponentElements => _maxComponentElements;

        /// <summary>
        /// Maximum levels of recursion; 1 for simple components.
        /// </summary>
        public ushort MaxComponentDepth => _maxComponentDepth;

        public Ttf M_Root => m_root;
        public Maxp M_Parent => m_parent;
    }
}
