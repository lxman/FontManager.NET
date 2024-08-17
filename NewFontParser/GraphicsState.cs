using System.Collections;
using System.Drawing;

namespace NewFontParser
{
    public class GraphicsState
    {
        public bool AutoFlip { get; set; }

        public bool Debug { get; set; }

        public uint ControlValueCutIn { get; set; }

        public int DeltaBase { get; set; }

        public int DeltaShift { get; set; }

        public int? DualProjectionVectors { get; set; }

        public PointF FreedomVector { get; set; }

        public BitArray ZonePointers { get; set; } = new BitArray(3, true);

        public int InstructControl { get; set; }

        public int Loop { get; set; } = 1;

        public uint MinimumDistance { get; set; }

        public PointF ProjectionVector { get; set; }

        public int RoundState { get; set; }

        public int ScanControl { get; set; }

        public uint SingleWidthCutIn { get; set; }

        public uint SingleWidthValue { get; set; }

        public uint[] ReferencePoints { get; set; } = new uint[3];
    }
}
