using System;
using System.Collections;
using System.Numerics;
using FontParser.Extensions;

// ReSharper disable BitwiseOperatorOnEnumWithoutFlags

namespace FontParser.RenderFont.Interpreter
{
    public class GraphicsState
    {
        public TouchState FreedomTouchState
        {
            get
            {
                var ts = TouchState.None;
                if (FreedomVector.X != 0)
                {
                    ts |= TouchState.X;
                }
                if (FreedomVector.Y != 0)
                {
                    ts |= TouchState.Y;
                }
                return ts;
            }
        }

        public TouchState ProjectionTouchState
        {
            get
            {
                var ts = TouchState.None;
                if (ProjectionVector.X != 0)
                {
                    ts |= TouchState.X;
                }
                if (ProjectionVector.Y != 0)
                {
                    ts |= TouchState.Y;
                }
                return ts;
            }
        }

        public Vector2 FreedomVector
        {
            get => _freedomVector;
            set
            {
                _freedomVector = value;
                VectorsDotProduct = Vector2.Dot(FreedomVector, ProjectionVector);
            }
        }

        private Vector2 _freedomVector = Vector2.UnitX;

        public Vector2 ProjectionVector
        {
            get => _projectionVector;
            set
            {
                _projectionVector = value;
                VectorsDotProduct = Vector2.Dot(FreedomVector, ProjectionVector);
            }
        }

        private Vector2 _projectionVector = Vector2.UnitX;

        public float VectorsDotProduct { get; private set; } = 1;

        public Vector2 DualProjectionVectors { get; set; }

        public InstructionControlFlags InstructControl { get; set; } = InstructionControlFlags.None;

        public RoundState RoundState { get; set; } = RoundState.Grid;

        public bool AutoFlip { get; set; } = true;

        public bool Debug { get; set; }

        public float ControlValueCutIn { get; set; } = 17f / 16;

        public int DeltaBase { get; set; } = 9;

        public int DeltaShift { get; set; } = 3;

        /// <summary>
        /// True if the zone pointer is set to the twilight zone.
        /// </summary>
        public BitArray ZonePointers { get; set; } = new BitArray(3, true);

        public int Loop { get; set; } = 1;

        public float MinimumDistance { get; set; } = 1;

        public int ScanControl { get; set; } = 0;

        public float SingleWidthCutIn { get; set; } = 0;

        public float SingleWidthValue { get; set; } = 0;

        public int Ppem { get; set; }

        public float Project(InterpreterPointF p)
        {
            var v = p.ToVector2();
            return Vector2.Dot(v, _projectionVector);
        }

        public float Round(float value)
        {
            switch (RoundState)
            {
                case RoundState.Grid:
                    return value >= 0 ? (float)Math.Round(value) : -(float)Math.Round(-value);
                case RoundState.HalfGrid:
                    return value >= 0 ? (float)Math.Floor(value) + 0.5f : -((float)Math.Floor(-value) + 0.5f);
                case RoundState.DoubleGrid:
                    return value >= 0 ? (float)(Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2) : -(float)(Math.Round(-value * 2, MidpointRounding.AwayFromZero) / 2);
                case RoundState.DownToGrid:
                    return value >= 0 ? (float)Math.Floor(value) : -(float)Math.Floor(-value);
                case RoundState.UpToGrid:
                    return value >= 0 ? (float)Math.Ceiling(value) : -(float)Math.Ceiling(-value);
                case RoundState.Off:
                    return value;
                case RoundState.Super:
                case RoundState.Super45:
                    float result;
                    if (value >= 0)
                    {
                        result = value - _roundPhase + _roundThreshold;
                        result = (float)Math.Truncate(result / _roundPeriod) * _roundPeriod;
                        result += _roundPhase;
                        if (result < 0)
                            result = _roundPhase;
                    }
                    else
                    {
                        result = -value - _roundPhase + _roundThreshold;
                        result = -(float)Math.Truncate(result / _roundPeriod) * _roundPeriod;
                        result -= _roundPhase;
                        if (result > 0)
                            result = -_roundPhase;
                    }
                    return result;
                default:
                    return value;
            }
        }

        public void SetSuperRound(float period, int mode)
        {
            _roundPeriod = (mode & 0xC0) switch
            {
                0 => period / 2,
                0x40 => period,
                0x80 => period * 2,
                _ => throw new ArgumentException("Invalid rounding period multiplier.")
            };

            _roundPhase = (mode & 0x30) switch
            {
                0 => 0,
                0x10 => _roundPeriod / 4,
                0x20 => _roundPeriod / 2,
                0x30 => _roundPeriod * 3 / 4,
                _ => _roundPhase
            };

            if ((mode & 0xF) == 0)
                _roundThreshold = _roundPeriod - 1;
            else
                _roundThreshold = ((mode & 0xF) - 4) * _roundPeriod / 8;
        }

        public uint[] ReferencePoints { get; set; } = new uint[3];

        private float _roundPhase;
        private float _roundThreshold;
        private float _roundPeriod;
    }
}