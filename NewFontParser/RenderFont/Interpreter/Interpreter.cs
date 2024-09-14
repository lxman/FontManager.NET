using System;
using System.Collections.Generic;
using System.Numerics;
using NewFontParser.Extensions;
using NewFontParser.Tables;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

// ReSharper disable EqualExpressionComparison

namespace NewFontParser.RenderFont.Interpreter
{
    public class Interpreter
    {
        public GraphicsState GraphicsState { get; }

        public Dictionary<int, byte[]> Functions { get; } = new Dictionary<int, byte[]>();

        private readonly InstructionStream _reader;
        private readonly StorageArea _storageArea;
        private readonly CvtTable _cvtTable;
        private readonly GlyphTable _glyphTable;
        private readonly GlyphData _glyphData;
        private readonly MaxPTable _maxPTable;
        private readonly Stack<int> _stack;

        public Interpreter(
            GlyphTable glyphTable,
            CvtTable cvtTable,
            byte[] instructions,
            MaxPTable maxPTable
        )
        {
            _reader = new InstructionStream(instructions);
            _cvtTable = cvtTable;
            _glyphTable = glyphTable;
            GraphicsState = new GraphicsState();
            _stack = new Stack<int>();
            _storageArea = new StorageArea(maxPTable.MaxStorage);
            _maxPTable = maxPTable;
        }

        public Interpreter(
            GlyphTable glyphTable,
            GlyphData glyphData,
            CvtTable cvtTable,
            MaxPTable maxPTable,
            GraphicsState graphicsState,
            Dictionary<int, byte[]> functions,
            byte[] instructions
        )
        {
            _reader = new InstructionStream(instructions);
            _storageArea = new StorageArea(maxPTable.MaxStorage);
            _cvtTable = cvtTable;
            _glyphTable = glyphTable;
            _glyphData = glyphData;
            GraphicsState = graphicsState;
            _stack = new Stack<int>();
            _maxPTable = maxPTable;
            for (var i = 0; i < maxPTable.MaxFunctionDefs; i++)
            {
                Functions.Add(i, Array.Empty<byte>());
            }
            foreach (KeyValuePair<int, byte[]> keyValuePair in functions)
            {
                Functions[keyValuePair.Key] = keyValuePair.Value;
            }
        }

        public void Execute()
        {
            while (!_reader.EndOfProgram)
            {
                byte instruction = _reader.ReadByte();
                switch (instruction)
                {
                    // SVTCA[0]
                    case 0x00:
                        GraphicsState.ProjectionVector = Vector2.UnitX;
                        break;
                    // SVTCA[1]
                    case 0x01:
                        GraphicsState.ProjectionVector = Vector2.UnitY;
                        break;
                    // SPVTCA[0]
                    case 0x02:
                        GraphicsState.ProjectionVector = Vector2.UnitX;
                        break;
                    // SPVTCA[1]
                    case 0x03:
                        GraphicsState.ProjectionVector = Vector2.UnitY;
                        break;
                    // SFVTCA[0]
                    case 0x04:
                        GraphicsState.FreedomVector = Vector2.UnitX;
                        break;
                    // SFVTCA[1]
                    case 0x05:
                        GraphicsState.FreedomVector = Vector2.UnitY;
                        break;
                    // SPVTL[0]
                    case 0x06:
                        // TODO: Implement SPVTL[0]
                        break;
                    // SPVTL[1]
                    case 0x07:
                        // TODO: Implement SPVTL[1]
                        break;
                    // SFVTL[0]
                    case 0x08:
                        // TODO: Implement SFVTL[0]
                        break;
                    // SFVTL[1]
                    case 0x09:
                        // TODO: Implement SFVTL[1]
                        break;
                    // SPVFS
                    case 0x0A:
                        int y = _stack.Pop();
                        int x = _stack.Pop();
                        float y214 = y / 16384f;
                        float x214 = x / 16384f;
                        GraphicsState.ProjectionVector = new Vector2(x214, y214);
                        break;
                    // SFVFS
                    case 0x0B:
                        y = _stack.Pop();
                        x = _stack.Pop();
                        y214 = y / 16384f;
                        x214 = x / 16384f;
                        GraphicsState.FreedomVector = new Vector2(x214, y214);
                        break;
                    // GPV
                    case 0x0C:
                        y214 = GraphicsState.ProjectionVector.Y;
                        x214 = GraphicsState.ProjectionVector.X;
                        _stack.Push((int)(x214 * 16384));
                        _stack.Push((int)(y214 * 16384));
                        break;
                    // GFV
                    case 0x0D:
                        x214 = GraphicsState.FreedomVector.X;
                        y214 = GraphicsState.FreedomVector.Y;
                        _stack.Push((int)(x214 * 16384));
                        _stack.Push((int)(y214 * 16384));
                        break;
                    // SFVTPV
                    case 0x0E:
                        GraphicsState.FreedomVector = GraphicsState.ProjectionVector;
                        break;
                    // ISECT
                    case 0x0F:
                        var b1 = Convert.ToUInt32(_stack.Pop());
                        var b0 = Convert.ToUInt32(_stack.Pop());
                        var a1 = Convert.ToUInt32(_stack.Pop());
                        var a0 = Convert.ToUInt32(_stack.Pop());
                        // TODO: Implement ISECT
                        break;
                    // SRP0
                    case 0x10:
                        GraphicsState.ReferencePoints[0] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SRP1
                    case 0x11:
                        GraphicsState.ReferencePoints[1] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SRP2
                    case 0x12:
                        GraphicsState.ReferencePoints[2] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SZP0
                    case 0x13:
                        GraphicsState.ReferencePoints[3] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SZP1
                    case 0x14:
                        GraphicsState.ReferencePoints[4] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SZP2
                    case 0x15:
                        GraphicsState.ReferencePoints[5] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SZPS
                    case 0x16:
                        GraphicsState.ReferencePoints[6] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SLOOP
                    case 0x17:
                        GraphicsState.Loop = _stack.Pop();
                        break;
                    // RTG
                    case 0x18:
                        GraphicsState.RoundState = RoundState.Grid;
                        break;
                    // RTHG
                    case 0x19:
                        GraphicsState.RoundState = RoundState.HalfGrid;
                        break;
                    // SMD
                    case 0x1A:
                        GraphicsState.MinimumDistance = Convert.ToUInt32(_stack.Pop());
                        break;
                    // ELSE
                    case 0x1B:
                        // TODO: Implement ELSE
                        break;
                    // JMPR
                    case 0x1C:
                        // TODO: Implement JMPR
                        break;
                    // SCVTCI
                    case 0x1D:
                        GraphicsState.ControlValueCutIn = Convert.ToUInt32(_stack.Pop()).ToF26Dot6();
                        break;
                    // SSWCI
                    case 0x1E:
                        GraphicsState.SingleWidthCutIn = Convert.ToUInt32(_stack.Pop()).ToF26Dot6();
                        break;
                    // SSW
                    case 0x1F:
                        GraphicsState.SingleWidthValue = Convert.ToUInt32(_stack.Pop()).ToF26Dot6();
                        break;
                    // DUP
                    case 0x20:
                        _stack.Push(_stack.Peek());
                        break;
                    // POP
                    case 0x21:
                        _stack.Pop();
                        break;
                    // CLEAR
                    case 0x22:
                        _stack.Clear();
                        break;
                    // SWAP
                    case 0x23:
                        int a = _stack.Pop();
                        int b = _stack.Pop();
                        _stack.Push(a);
                        _stack.Push(b);
                        break;
                    // DEPTH
                    case 0x24:
                        _stack.Push(_stack.Count);
                        break;
                    // CINDEX
                    case 0x25:
                        int index = _stack.Pop();
                        int indexedValue = _stack.ToArray()[index];
                        _stack.Push(indexedValue);
                        break;
                    // MINDEX[]
                    case 0x26:
                        index = _stack.Pop();
                        var values = new int[index + 1];
                        for (var i = 0; i <= index; i++)
                        {
                            values[i] = _stack.Pop();
                        }
                        indexedValue = values[index];
                        values[index] = values[0];
                        values[0] = indexedValue;
                        for (int i = index; i >= 0; i--)
                        {
                            _stack.Push(values[i]);
                        }
                        break;
                    // ALIGNPTS[]
                    case 0x27:
                        // TODO: Implement ALIGNPTS[]
                        break;
                    // ???
                    case 0x28:
                        // TODO: Implement ???
                        break;
                    // UTP[]
                    case 0x29:
                        // TODO: Implement UTP[]
                        break;
                    // LOOPCALL[]
                    case 0x2A:
                        // TODO: Implement LOOPCALL[]
                        break;
                    // CALL[]
                    case 0x2B:
                        int funcId = _stack.Pop();
                        // TODO: Implement CALL[]
                        break;
                    // FDEF[]
                    case 0x2C:
                        var function = new List<byte>();
                        int funcIndex = _stack.Pop();
                        var currInstruction = 0;
                        while (currInstruction != 0x2D)
                        {
                            currInstruction = _reader.ReadByte();
                            if (currInstruction != 0x2D)
                            {
                                function.Add(_reader.ReadByte());
                            }
                        }
                        Functions[funcIndex] = function.ToArray();
                        break;
                    // MDAP
                    case 0x2E:
                    case 0x2F:
                        bool round = Convert.ToBoolean(instruction - 0x2E);
                        // TODO: Implement MDAP[1]
                        break;
                    // IUP[0]
                    case 0x30:
                        // TODO: Implement IUP[0]
                        break;
                    // IUP[1]
                    case 0x31:
                        // TODO: Implement IUP[1]
                        break;
                    // SHP[0]
                    case 0x32:
                        // TODO: Implement SHP[0]
                        break;
                    // SHP[1]
                    case 0x33:
                        // TODO: Implement SHP[1]
                        break;
                    // SHC[0]
                    case 0x34:
                        // TODO: Implement SHC[0]
                        break;
                    // SHC[1]
                    case 0x35:
                        // TODO: Implement SHC[1]
                        break;
                    // SHZ[0]
                    case 0x36:
                        // TODO: Implement SHZ[0]
                        break;
                    // SHZ[1]
                    case 0x37:
                        // TODO: Implement SHZ[1]
                        break;
                    // SHPIX
                    case 0x38:
                        // TODO: Implement SHPIX
                        break;
                    // IP
                    case 0x39:
                        // TODO: Implement IP
                        break;
                    // MSIRP[0]
                    case 0x3A:
                        // TODO: Implement MSIRP[0]
                        break;
                    // MSIRP[1]
                    case 0x3B:
                        // TODO: Implement MSIRP[1]
                        break;
                    // AlignRP
                    case 0x3C:
                        // TODO: Implement AlignRP
                        break;
                    // RTDG
                    case 0x3D:
                        // TODO: Implement RTDG
                        break;
                    // MIAP[0]
                    case 0x3E:
                        // TODO: Implement MIAP[0]
                        break;
                    // MIAP[1]
                    case 0x3F:
                        // TODO: Implement MIAP[1]
                        int cvtEntryNumber = _stack.Pop();
                        int pointNumber = _stack.Pop();
                        float? cvtValue = _cvtTable.GetCvtValue(cvtEntryNumber);
                        var point = ((SimpleGlyph)_glyphData.GlyphSpec).Coordinates[pointNumber];
                        break;
                    // Push Bytes
                    case 0x40:
                    case 0xB0:
                    case 0xB1:
                    case 0xB2:
                    case 0xB3:
                    case 0xB4:
                    case 0xB5:
                    case 0xB6:
                    case 0xB7:
                        PushBytes(instruction);
                        break;
                    // Push Words
                    case 0x41:
                    case 0xB8:
                    case 0xB9:
                    case 0xBA:
                    case 0xBB:
                    case 0xBC:
                    case 0xBD:
                    case 0xBE:
                    case 0xBF:
                        PushWords(instruction);
                        break;
                    // Storage Area
                    case 0x42:
                    case 0x43:
                        ReadWriteStorage(instruction);
                        break;
                    // WCVTP
                    case 0x44:
                        float value = Convert.ToUInt32(_stack.Pop()).ToF26Dot6();
                        _cvtTable.WriteCvtValue(_stack.Pop(), value);
                        break;
                    // RCVT
                    case 0x45:
                        _stack.Push(Convert.ToInt32(_cvtTable.GetCvtValues(_stack.Pop(), 1)![0].FromF26Dot6()));
                        break;
                    // GC[0]
                    case 0x46:
                        GraphicsState.ControlValueCutIn = Convert.ToUInt32(_stack.Pop()).ToF26Dot6();
                        break;
                    // GC[1]
                    case 0x47:
                        GraphicsState.SingleWidthValue = Convert.ToUInt32(_stack.Pop()).ToF26Dot6();
                        break;
                    // SCFS
                    case 0x48:
                        GraphicsState.SingleWidthCutIn = Convert.ToUInt32(_stack.Pop()).ToF26Dot6();
                        break;
                    // MD[0]
                    case 0x49:
                        GraphicsState.MinimumDistance = Convert.ToUInt32(_stack.Pop()).ToF26Dot6();
                        break;
                    // MD[1]
                    case 0x4A:
                        GraphicsState.DeltaBase = _stack.Pop();
                        break;
                    // MPPEM
                    case 0x4B:
                        // TODO: Implement MPPEM
                        _stack.Push(0);
                        break;
                    // MPS
                    case 0x4C:
                        // TODO: Implement MPS
                        _stack.Push(0);
                        break;
                    // FLIPON
                    case 0x4D:
                        GraphicsState.AutoFlip = true;
                        break;
                    // FLIPOFF
                    case 0x4E:
                        GraphicsState.AutoFlip = false;
                        break;
                    // DEBUG
                    case 0x4F:
                        GraphicsState.Debug = true;
                        break;
                    // LT
                    case 0x50:
                        _stack.Push(_stack.Pop() < _stack.Pop() ? 1 : 0);
                        break;
                    // LTEQ
                    case 0x51:
                        _stack.Push(_stack.Pop() <= _stack.Pop() ? 1 : 0);
                        break;
                    // GT
                    case 0x52:
                        _stack.Push(_stack.Pop() > _stack.Pop() ? 1 : 0);
                        break;
                    // GTEQ
                    case 0x53:
                        _stack.Push(_stack.Pop() >= _stack.Pop() ? 1 : 0);
                        break;
                    // EQ
                    case 0x54:
                        _stack.Push(_stack.Pop() == _stack.Pop() ? 1 : 0);
                        break;
                    // NEQ
                    case 0x55:
                        _stack.Push(_stack.Pop() != _stack.Pop() ? 1 : 0);
                        break;
                    // ODD
                    case 0x56:
                        _stack.Push(_stack.Pop() % 2 == 1 ? 1 : 0);
                        break;
                    // EVEN
                    case 0x57:
                        _stack.Push(_stack.Pop() % 2 == 0 ? 1 : 0);
                        break;
                    // IF
                    case 0x58:
                        // TODO: Implement IF
                        break;
                    // EIF
                    case 0x59:
                        // TODO: Implement EIF
                        break;
                    // AND
                    case 0x5A:
                        _stack.Push(_stack.Pop() & _stack.Pop());
                        break;
                    // OR
                    case 0x5B:
                        _stack.Push(_stack.Pop() | _stack.Pop());
                        break;
                    // NOT
                    case 0x5C:
                        _stack.Push(~_stack.Pop());
                        break;
                    // DELTAP1
                    case 0x5D:
                        _stack.Push(_stack.Pop());
                        break;
                    // SDB
                    case 0x5E:
                        GraphicsState.DeltaBase = _stack.Pop();
                        break;
                    // SDS
                    case 0x5F:
                        GraphicsState.DeltaShift = _stack.Pop();
                        break;
                    // ADD
                    case 0x60:
                        _stack.Push(_stack.Pop() + _stack.Pop());
                        break;
                    // SUB
                    case 0x61:
                        _stack.Push(_stack.Pop() - _stack.Pop());
                        break;
                    // DIV
                    case 0x62:
                        _stack.Push(_stack.Pop() / _stack.Pop());
                        break;
                    // MUL
                    case 0x63:
                        _stack.Push(_stack.Pop() * _stack.Pop());
                        break;
                    // ABS
                    case 0x64:
                        _stack.Push(Math.Abs(_stack.Pop()));
                        break;
                    // NEG
                    case 0x65:
                        _stack.Push(-_stack.Pop());
                        break;
                    // FLOOR
                    case 0x66:
                        _stack.Push((int)Math.Floor(Convert.ToUInt32(_stack.Pop()).ToF26Dot6()));
                        break;
                    // CEILING
                    case 0x67:
                        _stack.Push((int)Math.Ceiling(Convert.ToUInt32(_stack.Pop()).ToF26Dot6()));
                        break;
                    // ROUND[0]
                    case 0x68:
                        // TODO: Implement ROUND[0]
                        break;
                    // ROUND[1]
                    case 0x69:
                        // TODO: Implement ROUND[1]
                        break;
                    // ROUND[2]
                    case 0x6A:
                        // TODO: Implement ROUND[2]
                        break;
                    // ROUND[3]
                    case 0x6B:
                        // TODO: Implement ROUND[3]
                        break;
                    // NROUND[0]
                    case 0x6C:
                        // TODO: Implement NROUND[0]
                        break;
                    // NROUND[1]
                    case 0x6D:
                        // TODO: Implement NROUND[1]
                        break;
                    // NROUND[2]
                    case 0x6E:
                        // TODO: Implement NROUND[2]
                        break;
                    // NROUND[3]
                    case 0x6F:
                        // TODO: Implement NROUND[3]
                        break;
                    // WCVTF
                    case 0x70:
                        value = Convert.ToUInt32(_stack.Pop()).ToF26Dot6();
                        _cvtTable.WriteCvtValue(_stack.Pop(), value);
                        break;
                    // DELTAC1
                    case 0x73:
                        // TODO: Implement DELTAC1
                        break;
                    // SROUND
                    case 0x76:
                        GraphicsState.RoundState = (RoundState)_stack.Pop();
                        break;
                    // S45ROUND
                    case 0x77:
                        // TODO: Implement S45ROUND
                        break;
                    // JROT
                    case 0x78:
                        // TODO: Implement JROT
                        break;
                    // JROF
                    case 0x79:
                        // TODO: Implement JROF
                        break;
                    // ROFF
                    case 0x7A:
                        // TODO: Implement ROFF
                        break;
                    // RUTG
                    case 0x7C:
                        // TODO: Implement RUTG
                        break;
                    // RDTG
                    case 0x7D:
                        // TODO: Implement RDTG
                        break;
                    // SANGW
                    case 0x7E:
                        // TODO: Implement SANGW
                        break;
                    // AA
                    case 0x7F:
                        // TODO: Implement AA
                        break;
                    // FLIPPT
                    case 0x80:
                        // TODO: Implement FLIPPT
                        break;
                    // FLIPRGON
                    case 0x81:
                        // TODO: Implement FLIPRGON
                        break;
                    // FLIPRGOFF
                    case 0x82:
                        // TODO: Implement FLIPRGOFF
                        break;
                    // SCANCTRL
                    case 0x85:
                        GraphicsState.ScanControl = _stack.Pop();
                        break;
                    // SDPVTL[0]
                    case 0x86:
                        // TODO: Implement SDPVTL[0]
                        break;
                    // SDPVTL[1]
                    case 0x87:
                        // TODO: Implement SDPVTL[1]
                        break;
                    // GETINFO
                    case 0x88:
                        // TODO: Implement GETINFO
                        break;
                    // IDEF
                    case 0x89:
                        // TODO: Implement IDEF
                        break;
                    // ROLL
                    case 0x8A:
                        values = new int[3];
                        values[0] = _stack.Pop();
                        values[1] = _stack.Pop();
                        values[2] = _stack.Pop();
                        int last = values[2];
                        values[2] = values[1];
                        values[1] = values[0];
                        values[0] = last;
                        break;
                    // MAX
                    case 0x8B:
                        int value1 = _stack.Pop();
                        int value2 = _stack.Pop();
                        _stack.Push(Math.Max(value1, value2));
                        break;
                    // MIN
                    case 0x8C:
                        value1 = _stack.Pop();
                        value2 = _stack.Pop();
                        _stack.Push(Math.Min(value1, value2));
                        break;
                    // SCANTYPE
                    case 0x8D:
                        // TODO: Implement SCANTYPE
                        break;
                    // INSTCTRL
                    case 0x8E:
                        // TODO: Implement INSTCTRL
                        break;
                    // MDRP[00000]
                    case 0xC0:
                        // TODO: Implement MDRP[00000]
                        break;
                    // MDRP[00001]
                    case 0xC1:
                        // TODO: Implement MDRP[00001]
                        break;
                    // MDRP[00010]
                    case 0xC2:
                        // TODO: Implement MDRP[00010]
                        break;
                    // MDRP[00011]
                    case 0xC3:
                        // TODO: Implement MDRP[00011]
                        break;
                    // MDRP[00100]
                    case 0xC4:
                        // TODO: Implement MDRP[00100]
                        break;
                    // MDRP[00101]
                    case 0xC5:
                        // TODO: Implement MDRP[00101]
                        break;
                    // MDRP[00110]
                    case 0xC6:
                        // TODO: Implement MDRP[00110]
                        break;
                    // MDRP[00111]
                    case 0xC7:
                        // TODO: Implement MDRP[00111]
                        break;
                    // MDRP[01000]
                    case 0xC8:
                        // TODO: Implement MDRP[01000]
                        break;
                    // MDRP[01001]
                    case 0xC9:
                        // TODO: Implement MDRP[01001]
                        break;
                    // MDRP[01010]
                    case 0xCA:
                        //  TODO: Implement MDRP[01010]
                        break;
                    // MDRP[01011]
                    case 0xCB:
                        // TODO: Implement MDRP[01011]
                        break;
                    // MDRP[01100]
                    case 0xCC:
                        // TODO: Implement MDRP[01100]
                        break;
                    // MDRP[01101]
                    case 0xCD:
                        // TODO: Implement MDRP[01101]
                        break;
                    // MDRP[01110]
                    case 0xCE:
                        // TODO: Implement MDRP[01110]
                        break;
                    // MDRP[01111]
                    case 0xCF:
                        // TODO: Implement MDRP[01111]
                        break;
                    // MDRP[10000]
                    case 0xD0:
                        // TODO: Implement MDRP[10000]
                        break;
                    // MDRP[10001]
                    case 0xD1:
                        // TODO: Implement MDRP[10001]
                        break;
                    // MDRP[10010]
                    case 0xD2:
                        // TODO: Implement MDRP[10010]
                        break;
                    // MDRP[10011]
                    case 0xD3:
                        // TODO: Implement MDRP[10011]
                        break;
                    // MDRP[10100]
                    case 0xD4:
                        // TODO: Implement MDRP[10100]
                        break;
                    // MDRP[10101]
                    case 0xD5:
                        // TODO: Implement MDRP[10101]
                        break;
                    // MDRP[10110]
                    case 0xD6:
                        // TODO: Implement MDRP[10110]
                        break;
                    // MDRP[10111]
                    case 0xD7:
                        // TODO: Implement MDRP[10111]
                        break;
                    // MDRP[11000]
                    case 0xD8:
                        // TODO: Implement MDRP[11000]
                        break;
                    // MDRP[11001]
                    case 0xD9:
                        // TODO: Implement MDRP[11001]
                        break;
                    // MDRP[11010]
                    case 0xDA:
                        // TODO: Implement MDRP[11010]
                        break;
                    // MDRP[11011]
                    case 0xDB:
                        // TODO: Implement MDRP[11011]
                        break;
                    // MDRP[11100]
                    case 0xDC:
                        // TODO: Implement MDRP[11100]
                        break;
                    // MDRP[11101]
                    case 0xDD:
                        // TODO: Implement MDRP[11101]
                        break;
                    // MDRP[11110]
                    case 0xDE:
                        // TODO: Implement MDRP[11110]
                        break;
                    // MDRP[11111]
                    case 0xDF:
                        // TODO: Implement MDRP[11111]
                        break;
                    // MIRP[00000]
                    case 0xE0:
                        // TODO: Implement MIRP[00000]
                        break;
                    // MIRP[00001]
                    case 0xE1:
                        // TODO: Implement MIRP[00001]
                        break;
                    // MIRP[00010]
                    case 0xE2:
                        // TODO: Implement MIRP[00010]
                        break;
                    // MIRP[00011]
                    case 0xE3:
                        // TODO: Implement MIRP[00011]
                        break;
                    // MIRP[00100]
                    case 0xE4:
                        // TODO: Implement MIRP[00100]
                        break;
                    // MIRP[00101]
                    case 0xE5:
                        // TODO: Implement MIRP[00101]
                        break;
                    // MIRP[00110]
                    case 0xE6:
                        // TODO: Implement MIRP[00110]
                        break;
                    // MIRP[00111]
                    case 0xE7:
                        // TODO: Implement MIRP[00111]
                        break;
                    // MIRP[01000]
                    case 0xE8:
                        // TODO: Implement MIRP[01000]
                        break;
                    // MIRP[01001]
                    case 0xE9:
                        // TODO: Implement MIRP[01001]
                        break;
                    // MIRP[01010]
                    case 0xEA:
                        // TODO: Implement MIRP[01010]
                        break;
                    // MIRP[01011]
                    case 0xEB:
                        // TODO: Implement MIRP[01011]
                        break;
                    // MIRP[01100]
                    case 0xEC:
                        // TODO: Implement MIRP[01100]
                        break;
                    // MIRP[01101]
                    case 0xED:
                        // TODO: Implement MIRP[01101]
                        break;
                    // MIRP[01110]
                    case 0xEE:
                        // TODO: Implement MIRP[01110]
                        break;
                    // MIRP[01111]
                    case 0xEF:
                        // TODO: Implement MIRP[01111]
                        break;
                    // MIRP[10000]
                    case 0xF0:
                        // TODO: Implement MIRP[10000]
                        break;
                    // MIRP[10001]
                    case 0xF1:
                        // TODO: Implement MIRP[10001]
                        break;
                    // MIRP[10010]
                    case 0xF2:
                        // TODO: Implement MIRP[10010]
                        break;
                    // MIRP[10011]
                    case 0xF3:
                        // TODO: Implement MIRP[10011]
                        break;
                    // MIRP[10100]
                    case 0xF4:
                        // TODO: Implement MIRP[10100]
                        break;
                    // MIRP[10101]
                    case 0xF5:
                        // TODO: Implement MIRP[10101]
                        break;
                    // MIRP[10110]
                    case 0xF6:
                        // TODO: Implement MIRP[10110]
                        break;
                    // MIRP[10111]
                    case 0xF7:
                        // TODO: Implement MIRP[10111]
                        break;
                    // MIRP[11000]
                    case 0xF8:
                        // TODO: Implement MIRP[11000]
                        break;
                    // MIRP[11001]
                    case 0xF9:
                        // TODO: Implement MIRP[11001]
                        break;
                    // MIRP[11010]
                    case 0xFA:
                        // TODO: Implement MIRP[11010]
                        break;
                    // MIRP[11011]
                    case 0xFB:
                        // TODO: Implement MIRP[11011]
                        break;
                    // MIRP[11100]
                    case 0xFC:
                        // TODO: Implement MIRP[11100]
                        break;
                    // MIRP[11101]
                    case 0xFD:
                        // TODO: Implement MIRP[11101]
                        break;
                    // MIRP[11110]
                    case 0xFE:
                        // TODO: Implement MIRP[11110]
                        break;
                    // MIRP[11111]
                    case 0xFF:
                        // TODO: Implement MIRP[11111]
                        break;
                }
            }
        }

        private void ReadWriteStorage(byte instruction)
        {
            switch (instruction)
            {
                case 0x42:
                    int value = _stack.Pop();
                    int address = _stack.Pop();
                    _storageArea[address] = value;
                    break;
                case 0x43:
                    address = _stack.Pop();
                    _stack.Push(_storageArea[address]);
                    break;
            }
        }

        private void PushBytes(byte instruction)
        {
            int n;
            if (instruction == 0x40)
            {
                n = _reader.ReadByte();
            }
            else
            {
                n = (instruction - 0xB0) + 1;
            }
            for (var i = 0; i < n; i++)
            {
                _stack.Push(_reader.ReadByte());
            }
        }

        private void PushWords(byte instruction)
        {
            int n;
            if (instruction == 0x41)
            {
                n = _reader.ReadByte();
            }
            else
            {
                n = (instruction - 0xB8) + 1;
            }
            for (var i = 0; i < n; i++)
            {
                _stack.Push(_reader.ReadWord());
            }
        }

        private void FromReaderToStack(byte arg)
        {
            int numToXfer = arg - 0xB7;
            for (var i = 0; i < numToXfer; i++)
            {
                _stack.Push(_reader.ReadWord());
            }
        }
    }
}