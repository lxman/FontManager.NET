using System;
using System.Collections.Generic;
using NewFontParser.Extensions;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;

// ReSharper disable EqualExpressionComparison

namespace NewFontParser.RenderFont.Interpreter
{
    public class Interpreter
    {
        private readonly InstructionStream _reader;
        private readonly StorageArea _storageArea;
        private readonly CvtTable _cvtTable;
        private readonly GlyphTable _glyphGlyphTable;
        private readonly GraphicsState _graphicsState;
        private readonly Stack<int> _stack;

        public Interpreter(
            InstructionStream reader,
            StorageArea storageArea,
            CvtTable cvtTable,
            GlyphTable glyphGlyphTable,
            GraphicsState graphicsState,
            Stack<int> stack
        )
        {
            _reader = reader;
            _storageArea = storageArea;
            _cvtTable = cvtTable;
            _glyphGlyphTable = glyphGlyphTable;
            _graphicsState = graphicsState;
            _stack = stack;
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
                        _graphicsState.ProjectionVector = new System.Drawing.PointF(1, 0);
                        break;
                    // SVTCA[1]
                    case 0x01:
                        _graphicsState.ProjectionVector = new System.Drawing.PointF(0, 1);
                        break;
                    // SPVTCA[0]
                    case 0x02:
                        _graphicsState.ProjectionVector = new System.Drawing.PointF(1, 0);
                        break;
                    // SPVTCA[1]
                    case 0x03:
                        _graphicsState.ProjectionVector = new System.Drawing.PointF(0, 1);
                        break;
                    // SFVTCA[0]
                    case 0x04:
                        _graphicsState.FreedomVector = new System.Drawing.PointF(1, 0);
                        break;
                    // SFVTCA[1]
                    case 0x05:
                        _graphicsState.FreedomVector = new System.Drawing.PointF(0, 1);
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
                        _graphicsState.ProjectionVector = new System.Drawing.PointF(x214, y214);
                        break;
                    // SFVFS
                    case 0x0B:
                        y = _stack.Pop();
                        x = _stack.Pop();
                        y214 = y / 16384f;
                        x214 = x / 16384f;
                        _graphicsState.FreedomVector = new System.Drawing.PointF(x214, y214);
                        break;
                    // GPV
                    case 0x0C:
                        y214 = _graphicsState.ProjectionVector.Y;
                        x214 = _graphicsState.ProjectionVector.X;
                        _stack.Push((int)(x214 * 16384));
                        _stack.Push((int)(y214 * 16384));
                        break;
                    // GFV
                    case 0x0D:
                        x214 = _graphicsState.FreedomVector.X;
                        y214 = _graphicsState.FreedomVector.Y;
                        _stack.Push((int)(x214 * 16384));
                        _stack.Push((int)(y214 * 16384));
                        break;
                    // SFVTPV
                    case 0x0E:
                        _graphicsState.FreedomVector = _graphicsState.ProjectionVector;
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
                        _graphicsState.ReferencePoints[0] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SRP1
                    case 0x11:
                        _graphicsState.ReferencePoints[1] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SRP2
                    case 0x12:
                        _graphicsState.ReferencePoints[2] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SZP0
                    case 0x13:
                        _graphicsState.ReferencePoints[3] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SZP1
                    case 0x14:
                        _graphicsState.ReferencePoints[4] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SZP2
                    case 0x15:
                        _graphicsState.ReferencePoints[5] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SZPS
                    case 0x16:
                        _graphicsState.ReferencePoints[6] = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SLOOP
                    case 0x17:
                        _graphicsState.Loop = _stack.Pop();
                        break;
                    // RTG
                    case 0x18:
                        _graphicsState.RoundState = (int)RoundState.Grid;
                        break;
                    // RTHG
                    case 0x19:
                        _graphicsState.RoundState = (int)RoundState.HalfGrid;
                        break;
                    // SMD
                    case 0x1A:
                        _graphicsState.MinimumDistance = Convert.ToUInt32(_stack.Pop());
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
                        _graphicsState.ControlValueCutIn = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SSWCI
                    case 0x1E:
                        _graphicsState.SingleWidthCutIn = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SSW
                    case 0x1F:
                        _graphicsState.SingleWidthValue = Convert.ToUInt32(_stack.Pop());
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
                        // TODO: Implement CINDEX
                        break;
                    // MINDEX
                    case 0x26:
                        // TODO: Implement MINDEX
                        break;
                    // ALIGNPTS
                    case 0x27:
                        // TODO: Implement ALIGNPTS
                        break;
                    // UTP
                    case 0x28:
                        // TODO: Implement UTP
                        break;
                    // LOOPCALL
                    case 0x29:
                        // TODO: Implement LOOPCALL
                        break;
                    // CALL
                    case 0x2A:
                        // TODO: Implement CALL
                        break;
                    // FDEF
                    case 0x2B:
                        // TODO: Implement FDEF
                        break;
                    // ENDF
                    case 0x2C:
                        // TODO: Implement ENDF
                        break;
                    // MDAP[0]
                    case 0x2D:
                        // TODO: Implement MDAP[0]
                        break;
                    // MDAP[1]
                    case 0x2E:
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
                        break;
                    // NPUSHB
                    case 0x40:
                        byte n = _reader.ReadByte();
                        for (var i = 0; i < n; i++)
                        {
                            _stack.Push(_reader.ReadByte());
                        }
                        break;
                    // NPUSHW
                    case 0x41:
                        n = _reader.ReadByte();
                        for (var i = 0; i < n; i++)
                        {
                            _stack.Push(_reader.ReadWord());
                        }
                        break;
                    // WS
                    case 0x42:
                        int value = _stack.Pop();
                        int address = _stack.Pop();
                        _storageArea[address] = value;
                        break;
                    // RS
                    case 0x43:
                        address = _stack.Pop();
                        _stack.Push(_storageArea[address]);
                        break;
                    // WCVTP
                    case 0x44:
                        value = _stack.Pop();
                        _cvtTable.WriteCvtValue(_stack.Pop(), value);
                        break;
                    // RCVT
                    case 0x45:
                        _stack.Push(Convert.ToInt32(_cvtTable.GetCvtValues(_stack.Pop(), 1)![0].FromF26Dot6()));
                        break;
                    // GC[0]
                    case 0x46:
                        _graphicsState.ControlValueCutIn = Convert.ToUInt32(_stack.Pop());
                        break;
                    // GC[1]
                    case 0x47:
                        _graphicsState.SingleWidthValue = Convert.ToUInt32(_stack.Pop());
                        break;
                    // SCFS
                    case 0x48:
                        _graphicsState.SingleWidthCutIn = Convert.ToUInt32(_stack.Pop());
                        break;
                    // MD[0]
                    case 0x49:
                        _graphicsState.MinimumDistance = Convert.ToUInt32(_stack.Pop());
                        break;
                    // MD[1]
                    case 0x4A:
                        _graphicsState.DeltaBase = _stack.Pop();
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
                        _graphicsState.AutoFlip = true;
                        break;
                    // FLIPOFF
                    case 0x4E:
                        _graphicsState.AutoFlip = false;
                        break;
                    // DEBUG
                    case 0x4F:
                        _graphicsState.Debug = true;
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
                        _graphicsState.DeltaBase = _stack.Pop();
                        break;
                    // SDS
                    case 0x5F:
                        _graphicsState.DeltaShift = _stack.Pop();
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
                        value = _stack.Pop();
                        _cvtTable.WriteCvtValue(_stack.Pop(), value);
                        break;
                    // DELTAC1
                    case 0x73:
                        // TODO: Implement DELTAC1
                        break;
                    // SROUND
                    case 0x76:
                        _graphicsState.RoundState = _stack.Pop();
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
                        _graphicsState.ScanControl = _stack.Pop();
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
                        // TODO: Implement ROLL
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
                    // PUSHB[000]
                    case 0xB0:
                        n = _reader.ReadByte();
                        for (var i = 0; i < n; i++)
                        {
                            _stack.Push(_reader.ReadByte());
                        }
                        break;
                    // PUSHB[001]
                    case 0xB1:
                        byte[] data = _reader.ReadBytes(2);
                        for (var i = 0; i < data.Length; i++)
                        {
                            _stack.Push(_reader.ReadByte());
                        }
                        break;
                    // PUSHB[010]
                    case 0xB2:
                        data = _reader.ReadBytes(3);
                        for (var i = 0; i < data.Length; i++)
                        {
                            _stack.Push(_reader.ReadByte());
                        }
                        break;
                    // PUSHB[011]
                    case 0xB3:
                        data = _reader.ReadBytes(4);
                        for (var i = 0; i < data.Length; i++)
                        {
                            _stack.Push(_reader.ReadByte());
                        }
                        break;
                    // PUSHB[100]
                    case 0xB4:
                        data = _reader.ReadBytes(5);
                        for (var i = 0; i < data.Length; i++)
                        {
                            _stack.Push(_reader.ReadByte());
                        }
                        break;
                    // PUSHB[101]
                    case 0xB5:
                        data = _reader.ReadBytes(6);
                        for (var i = 0; i < data.Length; i++)
                        {
                            _stack.Push(_reader.ReadByte());
                        }
                        break;
                    // PUSHB[110]
                    case 0xB6:
                        data = _reader.ReadBytes(7);
                        for (var i = 0; i < data.Length; i++)
                        {
                            _stack.Push(_reader.ReadByte());
                        }
                        break;
                    // PUSHB[111]
                    case 0xB7:
                        data = _reader.ReadBytes(8);
                        for (var i = 0; i < data.Length; i++)
                        {
                            _stack.Push(_reader.ReadByte());
                        }
                        break;
                    // PUSHW[000]
                    case 0xB8:
                        short word = _reader.ReadWord();
                        for (var i = 0; i < 1; i++)
                        {
                            _stack.Push(_reader.ReadWord());
                        }
                        break;
                    // PUSHW[001]
                    case 0xB9:
                        short[] words = _reader.ReadWords(2);
                        for (var i = 0; i < words.Length; i++)
                        {
                            _stack.Push(_reader.ReadWord());
                        }
                        break;
                    // PUSHW[010]
                    case 0xBA:
                        words = _reader.ReadWords(3);
                        for (var i = 0; i < words.Length; i++)
                        {
                            _stack.Push(_reader.ReadWord());
                        }
                        break;
                    // PUSHW[011]
                    case 0xBB:
                        words = _reader.ReadWords(4);
                        for (var i = 0; i < words.Length; i++)
                        {
                            _stack.Push(_reader.ReadWord());
                        }
                        break;
                    // PUSHW[100]
                    case 0xBC:
                        words = _reader.ReadWords(5);
                        for (var i = 0; i < words.Length; i++)
                        {
                            _stack.Push(_reader.ReadWord());
                        }
                        break;
                    // PUSHW[101]
                    case 0xBD:
                        words = _reader.ReadWords(6);
                        for (var i = 0; i < words.Length; i++)
                        {
                            _stack.Push(_reader.ReadWord());
                        }
                        break;
                    // PUSHW[110]
                    case 0xBE:
                        words = _reader.ReadWords(7);
                        for (var i = 0; i < words.Length; i++)
                        {
                            _stack.Push(_reader.ReadWord());
                        }
                        break;
                    // PUSHW[111]
                    case 0xBF:
                        words = _reader.ReadWords(8);
                        for (var i = 0; i < words.Length; i++)
                        {
                            _stack.Push(_reader.ReadWord());
                        }
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
    }
}