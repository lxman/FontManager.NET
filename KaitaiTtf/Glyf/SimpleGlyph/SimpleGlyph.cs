using System.Collections.Generic;
using System.Linq;
using Kaitai;

namespace KaitaiTtf.Glyf.SimpleGlyph
{
    public class SimpleGlyph : KaitaiStruct
    {
        public static SimpleGlyph FromFile(string fileName)
        {
            return new SimpleGlyph(new KaitaiStream(fileName));
        }

        public SimpleGlyph(KaitaiStream p__io, Glyf p__parent = null, Ttf p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            f_pointCount = false;
            _read();
        }
        private void _read()
        {
            _endPtsOfContours = new List<ushort>();
            for (var i = 0; i < M_Parent.NumberOfContours; i++)
            {
                _endPtsOfContours.Add(m_io.ReadU2be());
            }
            _instructionLength = m_io.ReadU2be();
            _instructions = m_io.ReadBytes(InstructionLength);
            _flags = new List<Flag>();
            for (var i = 0; i < PointCount; i++)
            {
                _flags.Add(new Flag(m_io, this, m_root));
            }
        }
        private bool f_pointCount;
        private int _pointCount;
        public int PointCount
        {
            get
            {
                if (f_pointCount)
                    return _pointCount;
                _pointCount = EndPtsOfContours.Max() + 1;
                f_pointCount = true;
                return _pointCount;
            }
        }
        private List<ushort> _endPtsOfContours;
        private ushort _instructionLength;
        private byte[] _instructions;
        private List<Flag> _flags;
        private Ttf m_root;
        private Glyf m_parent;
        public List<ushort> EndPtsOfContours => _endPtsOfContours;
        public ushort InstructionLength => _instructionLength;
        public byte[] Instructions => _instructions;
        public List<Flag> Flags => _flags;
        public Ttf M_Root => m_root;
        public Glyf M_Parent => m_parent;
    }
}
