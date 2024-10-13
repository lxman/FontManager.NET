using System.Collections;
using System.Numerics;

namespace FontParser.RenderFont.Interpreter
{
    public class GraphicsState
    {
        public Vector2 FreedomVector
        {
            get => _freedomVector;
            set
            {
                _freedomVector = value;
                _vectorsDotP = Vector2.Dot(FreedomVector, ProjectionVector);
            }
        }

        private Vector2 _freedomVector = Vector2.UnitX;

        public Vector2 ProjectionVector
        {
            get => _projectionVector;
            set
            {
                _projectionVector = value;
                _vectorsDotP = Vector2.Dot(FreedomVector, ProjectionVector);
            }
        }

        private Vector2 _projectionVector = Vector2.UnitX;

        public float VectorsDotProduct => _vectorsDotP;

        public Vector2 DualProjectionVectors { get; set; }

        public InstructionControlFlags InstructControl { get; set; } = InstructionControlFlags.None;

        public RoundState RoundState { get; set; } = RoundState.Grid;

        public bool AutoFlip { get; set; } = true;

        public bool Debug { get; set; }

        public float ControlValueCutIn { get; set; } = 17f / 16;

        public int DeltaBase { get; set; } = 9;

        public int DeltaShift { get; set; } = 3;

        public BitArray ZonePointers { get; set; } = new BitArray(3, true);

        public int Loop { get; set; } = 1;

        public float MinimumDistance { get; set; } = 1;

        public int ScanControl { get; set; } = 0;

        public float SingleWidthCutIn { get; set; } = 0;

        public float SingleWidthValue { get; set; } = 0;

        public uint[] ReferencePoints { get; set; } = new uint[3];

        private float _vectorsDotP;
    }
}