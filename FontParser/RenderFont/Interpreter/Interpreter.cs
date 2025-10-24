using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using FontParser.Extensions;
using FontParser.Tables;
using FontParser.Tables.Hmtx;
using FontParser.Tables.TtTables;
using FontParser.Tables.TtTables.Glyf;

// ReSharper disable EqualExpressionComparison

namespace FontParser.RenderFont.Interpreter
{
    public class Interpreter
    {
        public GraphicsState GraphicsState { get; }

        public Dictionary<int, byte[]> Functions { get; } = new Dictionary<int, byte[]>();

        private readonly InstructionStream _reader;
        private readonly StorageArea _storageArea;
        private readonly CvtTable _cvtTable;
        private readonly GlyphTable _glyphTable;
        private readonly MaxPTable _maxPTable;
        private readonly HmtxTable _hmtxTable;
        private readonly Stack<int> _stack;
        private readonly Zone _twilightZone;
        private readonly Zone _glyphZone = new Zone();
        private readonly SimpleGlyph _glyphData;
        private readonly IReadOnlyList<ushort> _contours = Array.Empty<ushort>();
        
        // Control flow fields
        private int _callStackSize = 0;
        private const int MaxCallStack = 128;

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
            _twilightZone = new Zone(true, new PointF[maxPTable.MaxTwilightPoints]);
        }

        public Interpreter(
            GlyphTable glyphTable,
            GlyphData glyphData,
            CvtTable cvtTable,
            HmtxTable hmtxTable,
            MaxPTable maxPTable,
            GraphicsState graphicsState,
            Dictionary<int, byte[]> functions,
            List<byte> instructions
        )
        {
            _reader = new InstructionStream(instructions.ToArray());
            _storageArea = new StorageArea(maxPTable.MaxStorage);
            _cvtTable = cvtTable;
            _glyphTable = glyphTable;
            GraphicsState = graphicsState;
            _stack = new Stack<int>();
            _maxPTable = maxPTable;
            _hmtxTable = hmtxTable;
            for (var i = 0; i < maxPTable.MaxFunctionDefs; i++)
            {
                Functions.Add(i, Array.Empty<byte>());
            }
            foreach (KeyValuePair<int, byte[]> keyValuePair in functions)
            {
                Functions[keyValuePair.Key] = keyValuePair.Value;
            }
            _twilightZone = new Zone(true, new PointF[maxPTable.MaxTwilightPoints]);
            short lsb = hmtxTable.LongHMetricRecords[glyphData.Index].LeftSideBearing;
            ushort advanceWidth = hmtxTable.LongHMetricRecords[glyphData.Index].AdvanceWidth;
            _glyphData = (SimpleGlyph)glyphData.GlyphSpec;
            List<SimpleGlyphCoordinate> coords = _glyphData.Coordinates;
            coords.Add(new SimpleGlyphCoordinate(new PointF(lsb, 0), false)); // Phantom point 0
            coords.Add(new SimpleGlyphCoordinate(new PointF(advanceWidth, 0), false)); // Phantom point 1
            coords.Add(new SimpleGlyphCoordinate(new PointF(0, 0), false)); // Phantom point 2
            coords.Add(new SimpleGlyphCoordinate(new PointF(0, 0), false)); // Phantom point 3
            _glyphZone = new Zone(false, coords.ToArray());
            _contours = _glyphData.EndPtsOfContours;
        }

        private int count;

        /// <summary>
        /// Public entry point for execution. Executes the main instruction stream.
        /// </summary>
        public void Execute()
        {
            Console.WriteLine("Execute");
            Execute(_reader);
        }

        /// <summary>
        /// Recursive execution method. Executes instructions from the given stream.
        /// This method is called recursively for CALL and LOOPCALL instructions.
        /// </summary>
        /// <param name="reader">The instruction stream to execute</param>
        private void Execute(InstructionStream reader)
        {
            while (!reader.EndOfProgram)
            {
                count++;
                byte instruction = reader.ReadByte();
                Console.WriteLine($"\tCount: {count}, Instruction: {instruction:X2}, Position: {reader.Position}");
                
                switch (instruction)
                {
                    // SVTCA[0]
                    case 0x00:
                        GraphicsState.FreedomVector = Vector2.UnitY;
                        GraphicsState.ProjectionVector = Vector2.UnitY;
                        break;
                    // SVTCA[1]
                    case 0x01:
                        GraphicsState.FreedomVector = Vector2.UnitX;
                        GraphicsState.ProjectionVector = Vector2.UnitX;
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
                        int point1 = _stack.Pop();
                        int point2 = _stack.Pop();
                        PointF p1 = GetCurrentPoint(point1, GraphicsState.ZonePointers[2]);
                        PointF p2 = GetCurrentPoint(point2, GraphicsState.ZonePointers[2]);
                        GraphicsState.ProjectionVector = ToUnit(p1, p2);
                        break;
                    // SPVTL[1]
                    case 0x07:
                        point1 = _stack.Pop();
                        point2 = _stack.Pop();
                        p1 = GetCurrentPoint(point1, GraphicsState.ZonePointers[2]);
                        p2 = GetCurrentPoint(point2, GraphicsState.ZonePointers[2]);
                        GraphicsState.ProjectionVector = ToUnit(p1, p2).Rotate(-90);
                        break;
                    // SFVTL[0]
                    case 0x08:
                        point1 = _stack.Pop();
                        point2 = _stack.Pop();
                        p1 = GetCurrentPoint(point1, GraphicsState.ZonePointers[2]);
                        p2 = GetCurrentPoint(point2, GraphicsState.ZonePointers[2]);
                        GraphicsState.FreedomVector = ToUnit(p1, p2);
                        break;
                    // SFVTL[1]
                    case 0x09:
                        point1 = _stack.Pop();
                        point2 = _stack.Pop();
                        p1 = GetCurrentPoint(point1, GraphicsState.ZonePointers[2]);
                        p2 = GetCurrentPoint(point2, GraphicsState.ZonePointers[2]);
                        GraphicsState.FreedomVector = ToUnit(p1, p2).Rotate(-90);
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
                        // As yet UNTESTED!!!!!!!!!!
                        int b1 = _stack.Pop();
                        int b0 = _stack.Pop();
                        int a1 = _stack.Pop();
                        int a0 = _stack.Pop();
                        int pointIndex = _stack.Pop();
                        PointF pointA0 = GetCurrentPoint(a0, GraphicsState.ZonePointers[1]);
                        PointF pointA1 = GetCurrentPoint(a1, GraphicsState.ZonePointers[1]);
                        PointF pointB0 = GetCurrentPoint(b0, GraphicsState.ZonePointers[0]);
                        PointF pointB1 = GetCurrentPoint(b1, GraphicsState.ZonePointers[0]);
                        PointF solution = Intersection(pointA0, pointA1, pointB0, pointB1);
                        GetZone(GraphicsState.ZonePointers[2]).MovePoint(pointIndex, solution);
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
                    {
                        // We hit the true block of an IF statement, so skip to the matching EIF
                        int indent = 1;
                        while (indent > 0)
                        {
                            byte nextOpcode = SkipNextInstruction(reader);
                            switch (nextOpcode)
                            {
                                case 0x58:  // IF
                                    indent++;
                                    break;
                                case 0x59:  // EIF
                                    indent--;
                                    break;
                            }
                        }
                    }
                    break;
                    
                    // JMPR
                    case 0x1C:
                    {
                        int offset = _stack.Pop();
                        Jump(reader, offset - 1);
                    }
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
                    // MINDEX
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
                    // ALIGNPTS
                    case 0x27:
                        // As yet UNTESTED!!!!!!!!!!
                        int pr1 = _stack.Pop();
                        int pr2 = _stack.Pop();
                        PointF pnt1 = GetCurrentPoint(pr1, GraphicsState.ZonePointers[1]);
                        PointF pnt2 = GetCurrentPoint(pr2, GraphicsState.ZonePointers[0]);
                        double scalarProjectionDiff = Vector2.Dot(pnt2.ToVector2() - pnt1.ToVector2(), GraphicsState.ProjectionVector) / 2;
                        GetZone(GraphicsState.ZonePointers[1]).MovePoint2(GraphicsState, pr1, Convert.ToSingle(scalarProjectionDiff));
                        GetZone(GraphicsState.ZonePointers[0]).MovePoint2(GraphicsState, pr2, -Convert.ToSingle(scalarProjectionDiff));
                        break;
                    // Deprecated
                    case 0x28:
                        throw new ArgumentException("Instruction 0x28 is deprecated.");
                    // UTP
                    case 0x29:
                        // As yet UNTESTED!!!!!!!!!!
                        pr1 = _stack.Pop();
                        GetZone(GraphicsState.ZonePointers[0]).UnTouchPoint(GraphicsState, pr1);
                        break;
                    // LOOPCALL
                    case 0x2A:
                    {
                        int funcId = _stack.Pop();
                        int loopCount = _stack.Pop();
                        
                        // Check for stack overflow
                        _callStackSize++;
                        if (_callStackSize > MaxCallStack)
                        {
                            throw new InvalidOperationException("Stack overflow; infinite recursion?");
                        }
                        
                        try
                        {
                            // Get the function bytecode
                            if (!Functions.TryGetValue(funcId, out byte[] functionCode))
                            {
                                throw new InvalidOperationException($"Function {funcId} is not defined");
                            }
                            
                            if (functionCode == null || functionCode.Length == 0)
                            {
                                throw new InvalidOperationException($"Function {funcId} has no bytecode");
                            }
                            
                            // Call the function 'loopCount' times
                            for (int i = 0; i < loopCount; i++)
                            {
                                var functionStream = new InstructionStream(functionCode);
                                Execute(functionStream);  // RECURSIVE CALL
                            }
                        }
                        finally
                        {
                            _callStackSize--;
                        }
                    }
                    break;
                    
                    // CALL
                    case 0x2B:
                    {
                        int funcId = _stack.Pop();
                        
                        // Check for stack overflow
                        _callStackSize++;
                        if (_callStackSize > MaxCallStack)
                        {
                            throw new InvalidOperationException("Stack overflow; infinite recursion?");
                        }
                        
                        try
                        {
                            // Get the function bytecode
                            if (!Functions.TryGetValue(funcId, out byte[] functionCode))
                            {
                                throw new InvalidOperationException($"Function {funcId} is not defined");
                            }
                            
                            if (functionCode == null || functionCode.Length == 0)
                            {
                                throw new InvalidOperationException($"Function {funcId} has no bytecode");
                            }
                            
                            // Execute the function with a new instruction stream
                            // The stack is automatically shared because we're in the same Interpreter instance
                            var functionStream = new InstructionStream(functionCode);
                            Execute(functionStream);  // RECURSIVE CALL
                        }
                        finally
                        {
                            _callStackSize--;
                        }
                    }
                    break;
                    
                    // FDEF
                    case 0x2C:
                        var function = new List<byte>();
                        int funcIndex = _stack.Pop();
                        byte currInstruction = 0;
                        while (currInstruction != 0x2D)
                        {
                            currInstruction = reader.ReadByte();
                            if (currInstruction != 0x2D)
                            {
                                function.Add(currInstruction);
                            }
                        }
                        Functions[funcIndex] = function.ToArray();
                        break;
                    // MDAP
                    case 0x2E:
                    case 0x2F:
                        // As yet UNTESTED!!!!!!!!!!
                        var round = Convert.ToBoolean(instruction - 0x2E);
                        int pNumber = _stack.Pop();
                        Zone zone = GetZone(GraphicsState.ZonePointers[0]);
                        InterpreterPointF p = zone.Current[pNumber];
                        var distance = 0.0f;
                        if (round)
                        {
                            distance = GraphicsState.Project(p);
                            distance = GraphicsState.Round(distance) - distance;
                        }
                        zone.MovePoint2(GraphicsState, pNumber, distance);
                        GraphicsState.ReferencePoints[0] = Convert.ToUInt32(pNumber);
                        GraphicsState.ReferencePoints[1] = Convert.ToUInt32(pNumber);
                        break;
                    // IUP[0] and IUP[1] - Interpolate Untouched Points
                    case 0x30:
                    case 0x31:
                    {
                        if (_contours.Count == 0)
                        {
                            break;
                        }

                        // IUP[0] = Y direction (vertical), IUP[1] = X direction (horizontal)
                        bool isXDirection = instruction == 0x31;
                        TouchState touchMask = isXDirection ? TouchState.X : TouchState.Y;

                        var iupPointIndex = 0;
                        foreach (ushort endPoint in _contours)
                        {
                            int firstPoint = iupPointIndex;
                            int firstTouched = -1;
                            int lastTouched = -1;

                            // Find all touched points in this contour
                            for (; iupPointIndex <= endPoint; iupPointIndex++)
                            {
                                if ((_glyphZone.Current[iupPointIndex].TouchState & touchMask) == 0) continue;
                                if (firstTouched < 0)
                                {
                                    firstTouched = iupPointIndex;
                                    lastTouched = iupPointIndex;
                                    continue;
                                }

                                // Interpolate untouched points between lastTouched and current point
                                InterpolateUntouchedPoints(
                                    isXDirection,
                                    lastTouched + 1,
                                    iupPointIndex - 1,
                                    lastTouched,
                                    iupPointIndex);

                                lastTouched = iupPointIndex;
                            }

                            // Handle different cases after scanning the contour
                            if (firstTouched < 0) continue;
                            // Case 1: Only one touched point in the contour
                            // Shift all other points by the same delta
                            if (lastTouched == firstTouched)
                            {
                                float delta = isXDirection
                                    ? _glyphZone.Current[lastTouched].PointF.X - _glyphZone.Original[lastTouched].PointF.X
                                    : _glyphZone.Current[lastTouched].PointF.Y - _glyphZone.Original[lastTouched].PointF.Y;

                                if (!(Math.Abs(delta) > float.Epsilon)) continue;
                                for (int i = firstPoint; i < lastTouched; i++)
                                {
                                    ShiftPoint(i, delta, isXDirection);
                                }

                                for (int i = lastTouched + 1; i <= endPoint; i++)
                                {
                                    ShiftPoint(i, delta, isXDirection);
                                }
                            }
                            // Case 2: Multiple touched points
                            // Interpolate from last to first (wrap around), and handle any gap at the start
                            else
                            {
                                // Interpolate from lastTouched to end of contour, wrapping to firstTouched
                                InterpolateUntouchedPoints(
                                    isXDirection,
                                    lastTouched + 1,
                                    endPoint,
                                    lastTouched,
                                    firstTouched);

                                // Interpolate from start of contour to firstTouched
                                if (firstTouched > firstPoint)
                                {
                                    InterpolateUntouchedPoints(
                                        isXDirection,
                                        firstPoint,
                                        firstTouched - 1,
                                        lastTouched,
                                        firstTouched);
                                }
                            }
                        }
                        break;
                    }
                    // SHP[0]
                    case 0x32:
                        throw new NotImplementedException();
                        // TODO: Implement SHP[0]
                        break;
                    // SHP[1]
                    case 0x33:
                        throw new NotImplementedException();
                        // TODO: Implement SHP[1]
                        break;
                    // SHC[0]
                    case 0x34:
                        throw new NotImplementedException();
                        // TODO: Implement SHC[0]
                        break;
                    // SHC[1]
                    case 0x35:
                        throw new NotImplementedException();
                        // TODO: Implement SHC[1]
                        break;
                    // SHZ[0]
                    case 0x36:
                        throw new NotImplementedException();
                        // TODO: Implement SHZ[0]
                        break;
                    // SHZ[1]
                    case 0x37:
                        throw new NotImplementedException();
                        // TODO: Implement SHZ[1]
                        break;
                    // SHPIX
                    case 0x38:
                        throw new NotImplementedException();
                        // TODO: Implement SHPIX
                        break;
                    // IP
                    case 0x39:
                        throw new NotImplementedException();
                        // TODO: Implement IP
                        break;
                    // MSIRP[0]
                    case 0x3A:
                        throw new NotImplementedException();
                        // TODO: Implement MSIRP[0]
                        break;
                    // MSIRP[1]
                    case 0x3B:
                        throw new NotImplementedException();
                        // TODO: Implement MSIRP[1]
                        break;
                    // AlignRP
                    case 0x3C:
                        throw new NotImplementedException();
                        // TODO: Implement AlignRP
                        break;
                    // RTDG
                    case 0x3D:
                        throw new NotImplementedException();
                        // TODO: Implement RTDG
                        break;
                    // MIAP[0]
                    case 0x3E:
                        throw new NotImplementedException();
                        // TODO: Implement MIAP[0]
                        break;
                    // MIAP[1]
                    case 0x3F:
                        throw new NotImplementedException();
                        // TODO: Implement MIAP[1]
                        int cvtEntryNumber = _stack.Pop();
                        int pointNumber = _stack.Pop();
                        float? cvtValue = _cvtTable.GetCvtValue(cvtEntryNumber);
                        SimpleGlyphCoordinate? point = _glyphData.Coordinates[pointNumber];
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
                    // GC[0] - Get Coordinate (current position projected onto projection vector)
                    case 0x46:
                    {
                        int gcPointNum = _stack.Pop();
                        Zone gcZone = GetZone(GraphicsState.ZonePointers[2]);
                        InterpreterPointF gcPoint = gcZone.Current[gcPointNum];
                        float gcCoord = GraphicsState.Project(gcPoint);
                        _stack.Push((int)(gcCoord * 64)); // Convert to F26.6
                        break;
                    }
                    // GC[1] - Get Coordinate (original position projected onto dual projection vector)
                    case 0x47:
                    {
                        int gcPointNum = _stack.Pop();
                        Zone gcZone = GetZone(GraphicsState.ZonePointers[2]);
                        InterpreterPointF gcOriginal = gcZone.Original[gcPointNum];
                        // Project onto dual projection vector (use original outline positions)
                        float gcCoord = Vector2.Dot(gcOriginal.ToVector2(), GraphicsState.DualProjectionVectors);
                        _stack.Push((int)(gcCoord * 64)); // Convert to F26.6
                        break;
                    }
                    // SCFS - Set Coordinate From Stack
                    case 0x48:
                    {
                        float scfsValue = Convert.ToUInt32(_stack.Pop()).ToF26Dot6();
                        int scfsPointNum = _stack.Pop();
                        Zone scfsZone = GetZone(GraphicsState.ZonePointers[2]);
                        InterpreterPointF scfsPoint = scfsZone.Current[scfsPointNum];
                        float scfsCurrentCoord = GraphicsState.Project(scfsPoint);
                        scfsZone.MovePoint2(GraphicsState, scfsPointNum, scfsValue - scfsCurrentCoord);
                        
                        // Moving twilight points moves their "original" value also
                        if (scfsZone.IsTwilight)
                        {
                            scfsZone.Original[scfsPointNum] = scfsZone.Current[scfsPointNum];
                        }
                        break;
                    }
                    // MD[0] - Measure Distance (original outline)
                    case 0x49:
                    {
                        int mdPoint2Num = _stack.Pop();
                        int mdPoint1Num = _stack.Pop();
                        Zone mdZone0 = GetZone(GraphicsState.ZonePointers[0]);
                        Zone mdZone1 = GetZone(GraphicsState.ZonePointers[1]);
                        InterpreterPointF mdP1 = mdZone1.Original[mdPoint2Num];
                        InterpreterPointF mdP2 = mdZone0.Original[mdPoint1Num];
                        // Measure distance using dual projection vector
                        Vector2 mdDiff = mdP1.ToVector2() - mdP2.ToVector2();
                        float mdDistance = Vector2.Dot(mdDiff, GraphicsState.DualProjectionVectors);
                        _stack.Push((int)(mdDistance * 64)); // Convert to F26.6
                        break;
                    }
                    // MD[1] - Measure Distance (current/grid-fitted position)
                    case 0x4A:
                    {
                        int mdPoint2Num = _stack.Pop();
                        int mdPoint1Num = _stack.Pop();
                        Zone mdZone0 = GetZone(GraphicsState.ZonePointers[0]);
                        Zone mdZone1 = GetZone(GraphicsState.ZonePointers[1]);
                        InterpreterPointF mdP1 = mdZone1.Current[mdPoint2Num];
                        InterpreterPointF mdP2 = mdZone0.Current[mdPoint1Num];
                        // Measure distance using projection vector
                        Vector2 mdDiff = mdP1.ToVector2() - mdP2.ToVector2();
                        float mdDistance = Vector2.Dot(mdDiff, GraphicsState.ProjectionVector);
                        _stack.Push((int)(mdDistance * 64)); // Convert to F26.6
                        break;
                    }
                    // MPPEM
                    case 0x4B:
                        throw new NotImplementedException();
                        // TODO: Implement MPPEM
                        _stack.Push(0);
                        break;
                    // MPS
                    case 0x4C:
                        throw new NotImplementedException();
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
                    {
                        // Pop the condition from the stack
                        bool condition = _stack.Pop() != 0;
                        
                        if (!condition)
                        {
                            // Condition is false; jump to the next ELSE block or EIF marker
                            int indent = 1;
                            while (indent > 0)
                            {
                                byte nextOpcode = SkipNextInstruction(reader);
                                switch (nextOpcode)
                                {
                                    case 0x58:  // IF - nested IF statement
                                        indent++;
                                        break;
                                    case 0x59:  // EIF - end of IF statement
                                        indent--;
                                        break;
                                    case 0x1B:  // ELSE
                                        if (indent == 1)
                                        {
                                            // Found the matching ELSE for our IF
                                            indent = 0;
                                        }
                                        break;
                                }
                            }
                        }
                        // If condition is true, we don't need to do anything - just continue executing
                    }
                    break;
                    
                    // EIF
                    case 0x59:
                    {
                        // Nothing to do - this is just a marker
                    }
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
                    // DELTAP
                    case 0x5D:
                    case 0x71:
                    case 0x72:
                        // As yet UNTESTED!!!!!!!!!!
                        int nPoints = _stack.Pop();
                        for (var i = 0; i < nPoints; i++)
                        {
                            int pn = _stack.Pop();
                            int arg = _stack.Pop();
                            int relPpem = (arg & 0xF0) >> 4;
                            int magnitude = arg & 0x0F;
                            int numSteps = magnitude > 7 ? magnitude - 7 : magnitude - 8;
                            relPpem += GraphicsState.DeltaBase;
                            if (instruction != 0x5D)
                            {
                                relPpem += (instruction - 0x71 + 1) * 16;
                            }

                            if (relPpem != GraphicsState.Ppem) continue;
                            if (numSteps >= 0) numSteps++;
                            numSteps *= 1 << (6 - GraphicsState.DeltaShift);
                            GetZone(GraphicsState.ZonePointers[0]).MovePoint2(GraphicsState, pn, numSteps.ToFixed());
                        }
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
                    // ROUND
                    case 0x68:
                    case 0x69:
                    case 0x6A:
                    case 0x6B:
                        throw new NotImplementedException();
                        // TODO: Implement ROUND
                        break;
                    // NROUND
                    case 0x6C:
                    case 0x6D:
                    case 0x6E:
                    case 0x6F:
                        throw new NotImplementedException();
                        // TODO: Implement NROUND
                        break;
                    // WCVTF
                    case 0x70:
                        value = Convert.ToUInt32(_stack.Pop()).ToF26Dot6();
                        _cvtTable.WriteCvtValue(_stack.Pop(), value);
                        break;
                    // DELTAC1
                    case 0x73:
                        throw new NotImplementedException();
                        // TODO: Implement DELTAC1
                        break;
                    // SROUND
                    case 0x76:
                        GraphicsState.RoundState = (RoundState)_stack.Pop();
                        break;
                    // S45ROUND
                    case 0x77:
                        throw new NotImplementedException();
                        // TODO: Implement S45ROUND
                        break;
                    // JROT - Jump Relative On True
                    case 0x78:
                    {
                        bool condition = _stack.Pop() != 0;
                        int offset = _stack.Pop();
                        
                        if (condition)
                        {
                            Jump(reader, offset - 1);
                        }
                    }
                    break;
                    
                    // JROF - Jump Relative On False
                    case 0x79:
                    {
                        bool condition = _stack.Pop() != 0;
                        int offset = _stack.Pop();
                        
                        if (!condition)
                        {
                            Jump(reader, offset - 1);
                        }
                    }
                    break;
                    
                    // ROFF
                    case 0x7A:
                        throw new NotImplementedException();
                        // TODO: Implement ROFF
                        break;
                    // RUTG
                    case 0x7C:
                        throw new NotImplementedException();
                        // TODO: Implement RUTG
                        break;
                    // RDTG
                    case 0x7D:
                        throw new NotImplementedException();
                        // TODO: Implement RDTG
                        break;
                    // SANGW
                    case 0x7E:
                        throw new NotImplementedException();
                        // TODO: Implement SANGW
                        break;
                    // AA
                    case 0x7F:
                        throw new NotImplementedException();
                        // TODO: Implement AA
                        break;
                    // FLIPPT
                    case 0x80:
                        throw new NotImplementedException();
                        // TODO: Implement FLIPPT
                        break;
                    // FLIPRGON
                    case 0x81:
                        throw new NotImplementedException();
                        // TODO: Implement FLIPRGON
                        break;
                    // FLIPRGOFF
                    case 0x82:
                        throw new NotImplementedException();
                        // TODO: Implement FLIPRGOFF
                        break;
                    // SCANCTRL
                    case 0x85:
                        GraphicsState.ScanControl = _stack.Pop();
                        break;
                    // SDPVTL[0]
                    case 0x86:
                        throw new NotImplementedException();
                        // TODO: Implement SDPVTL[0]
                        break;
                    // SDPVTL[1]
                    case 0x87:
                        throw new NotImplementedException();
                        // TODO: Implement SDPVTL[1]
                        break;
                    // GETINFO
                    case 0x88:
                        throw new NotImplementedException();
                        // TODO: Implement GETINFO
                        break;
                    // IDEF
                    case 0x89:
                        throw new NotImplementedException();
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
                        throw new NotImplementedException();
                        // TODO: Implement SCANTYPE
                        break;
                    // INSTCTRL
                    case 0x8E:
                        throw new NotImplementedException();
                        // TODO: Implement INSTCTRL
                        break;
                    // MDRP
                    case 0xC0:
                    case 0xC1:
                    case 0xC2:
                    case 0xC3:
                    case 0xC4:
                    case 0xC5:
                    case 0xC6:
                    case 0xC7:
                    case 0xC8:
                    case 0xC9:
                    case 0xCA:
                    case 0xCB:
                    case 0xCC:
                    case 0xCD:
                    case 0xCE:
                    case 0xCF:
                    case 0xD0:
                    case 0xD1:
                    case 0xD2:
                    case 0xD3:
                    case 0xD4:
                    case 0xD5:
                    case 0xD6:
                    case 0xD7:
                    case 0xD8:
                    case 0xD9:
                    case 0xDA:
                    case 0xDB:
                    case 0xDC:
                    case 0xDD:
                    case 0xDE:
                    case 0xDF:
                        // As yet UNTESTED!!!!!!!!!!
                        MoveDirectRelativePoint(instruction - 0xC0);
                        break;
                    // MIRP
                    case 0xE0:
                    case 0xE1:
                    case 0xE2:
                    case 0xE3:
                    case 0xE4:
                    case 0xE5:
                    case 0xE6:
                    case 0xE7:
                    case 0xE8:
                    case 0xE9:
                    case 0xEA:
                    case 0xEB:
                    case 0xEC:
                    case 0xED:
                    case 0xEE:
                    case 0xEF:
                    case 0xF0:
                    case 0xF1:
                    case 0xF2:
                    case 0xF3:
                    case 0xF4:
                    case 0xF5:
                    case 0xF6:
                    case 0xF7:
                    case 0xF8:
                    case 0xF9:
                    case 0xFA:
                    case 0xFB:
                    case 0xFC:
                    case 0xFD:
                    case 0xFE:
                    case 0xFF:
                        throw new NotImplementedException();
                        // TODO: Implement MIRP
                        break;
                }
            }
        }
        
        /// <summary>
        /// Skips forward in the instruction stream and returns the opcode found.
        /// Used for control flow to find matching IF/ELSE/EIF markers.
        /// </summary>
        private byte SkipNextInstruction(InstructionStream reader)
        {
            if (reader.EndOfProgram)
            {
                throw new InvalidOperationException("Unexpected end of program while skipping instructions");
            }

            byte opcode = reader.ReadByte();

            switch (opcode)
            {
                // Handle instructions that have inline data
                // NPUSHB - push n bytes
                case 0x40:
                {
                    int n = reader.ReadByte();
                    reader.ReadBytes(n);
                    break;
                }
                // NPUSHW - push n words
                case 0x41:
                {
                    int n = reader.ReadByte();
                    reader.ReadWords(n);
                    break;
                }
                default:
                {
                    if (opcode >= 0xB0 && opcode <= 0xB7)  // PUSHB[abc] - push 1-8 bytes
                    {
                        int skipCount = (opcode - 0xB0) + 1;
                        reader.ReadBytes(skipCount);
                    }
                    else if (opcode >= 0xB8 && opcode <= 0xBF)  // PUSHW[abc] - push 1-8 words
                    {
                        int skipCount = (opcode - 0xB8) + 1;
                        reader.ReadWords(skipCount);
                    }

                    break;
                }
            }
    
            return opcode;
        }

        /// <summary>
        /// Jumps to a relative offset in the instruction stream.
        /// The offset is relative to the current position.
        /// </summary>
        private void Jump(InstructionStream reader, int offset)
        {
            // Calculate target position
            int targetPosition = reader.Position + offset;
    
            // Sanity check to prevent jumping out of bounds
            if (targetPosition < 0)
            {
                throw new InvalidOperationException($"Invalid jump target: {targetPosition} (negative)");
            }
    
            reader.Seek(targetPosition);
        }

        private void MoveDirectRelativePoint(int flags)
        {
            int pointNumber = _stack.Pop();
            bool setRp0ToP = (flags & 0x10) == 0x10;
            bool keepDistance = (flags & 0x08) == 0x08;
            bool round = (flags & 0x04) == 0x04;
            var dType = (DistanceType)(flags & 0x03);
            var p1 = GetZone(GraphicsState.ZonePointers[0]).Current[GraphicsState.ReferencePoints[0]].ToVector2();
            var p2 = GetZone(GraphicsState.ZonePointers[1]).Current[pointNumber].ToVector2();
            float origDist = Vector2.Dot(p2 - p1, GraphicsState.DualProjectionVectors);

            if (Math.Abs(origDist - GraphicsState.SingleWidthValue) < GraphicsState.SingleWidthCutIn)
            {
                origDist = origDist >= 0 ? GraphicsState.SingleWidthValue : -GraphicsState.SingleWidthValue;
            }

            float distance = origDist;
            distance = round ? GraphicsState.Round(distance) : distance;
            if (keepDistance)
            {
                distance = origDist >= 0
                    ? Math.Max(distance, GraphicsState.MinimumDistance)
                    : Math.Min(distance, -GraphicsState.MinimumDistance);
            }

            var zP1 = GetZone(GraphicsState.ZonePointers[1]).Current[pointNumber].ToVector2();
            var zP2 = GetZone(GraphicsState.ZonePointers[0]).Current[GraphicsState.ReferencePoints[0]].ToVector2();
            origDist = Vector2.Dot(zP1 - zP2, GraphicsState.ProjectionVector);
            Zone zone = GetZone(GraphicsState.ZonePointers[1]);
            zone.MovePoint2(GraphicsState, pointNumber, distance - origDist);
            GraphicsState.ReferencePoints[1] = GraphicsState.ReferencePoints[0];
            GraphicsState.ReferencePoints[2] = Convert.ToUInt32(pointNumber);
            if (setRp0ToP)
            {
                GraphicsState.ReferencePoints[0] = Convert.ToUInt32(pointNumber);
            }
        }

        private Zone GetZone(bool zonePointer)
        {
            return zonePointer ? _glyphZone : _twilightZone;
        }

        private PointF GetCurrentPoint(int index, bool zonePointer)
        {
            return GetZone(zonePointer).Current[index].PointF;
        }

        private static Vector2 ToUnit(PointF a, PointF b)
        {
            float length = Length(a, b);
            return new Vector2((b.X - a.X) / length, (b.Y - a.Y) / length);
        }

        private static float Length(PointF a, PointF b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        private static PointF Intersection(PointF a0, PointF a1, PointF b0, PointF b1)
        {
            float deltaY1 = a1.Y - a0.Y;
            float deltaX1 = a0.X - a1.X;
            float factor1 = deltaY1 * a0.X + deltaX1 * a0.Y;

            float deltaY2 = b1.Y - b0.Y;
            float deltaX2 = b0.X - b1.X;
            float factor2 = deltaY2 * b0.X + deltaX2 * b0.Y;

            float delta = deltaY1 * deltaX2 - deltaY2 * deltaX1;
            if (delta == 0) throw new InvalidOperationException("Lines do not intersect");

            float x = (deltaX2 * factor1 - deltaX1 * factor2) / delta;
            float y = (deltaY1 * factor2 - deltaY2 * factor1) / delta;

            return new PointF(x, y);
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
        
        /// <summary>
        /// Interpolates untouched points between two reference points for IUP instruction.
        /// </summary>
        private void InterpolateUntouchedPoints(bool isXDirection, int startIdx, int endIdx, int ref1Idx, int ref2Idx)
        {
            if (startIdx > endIdx)
            {
                return;
            }

            // Get the coordinate values we're working with (X or Y)
            float ref1Original = isXDirection 
                ? _glyphZone.Original[ref1Idx].PointF.X 
                : _glyphZone.Original[ref1Idx].PointF.Y;
            float ref2Original = isXDirection 
                ? _glyphZone.Original[ref2Idx].PointF.X 
                : _glyphZone.Original[ref2Idx].PointF.Y;

            float ref1Current = isXDirection 
                ? _glyphZone.Current[ref1Idx].PointF.X 
                : _glyphZone.Current[ref1Idx].PointF.Y;
            float ref2Current = isXDirection 
                ? _glyphZone.Current[ref2Idx].PointF.X 
                : _glyphZone.Current[ref2Idx].PointF.Y;

            // Determine ordering and calculate deltas
            float lowerOriginal, upperOriginal, lowerCurrent, upperCurrent;
            float delta1, delta2;

            if (ref1Original > ref2Original)
            {
                upperOriginal = ref1Original;
                lowerOriginal = ref2Original;
                delta1 = ref1Current - upperOriginal;
                delta2 = ref2Current - lowerOriginal;
                lowerCurrent = delta2 + lowerOriginal;
                upperCurrent = delta1 + upperOriginal;
            }
            else
            {
                lowerOriginal = ref1Original;
                upperOriginal = ref2Original;
                delta1 = ref1Current - lowerOriginal;
                delta2 = ref2Current - upperOriginal;
                lowerCurrent = delta1 + lowerOriginal;
                upperCurrent = delta2 + upperOriginal;
            }

            float scale = (Math.Abs(upperOriginal - lowerOriginal) > 0.001) 
                ? (upperCurrent - lowerCurrent) / (upperOriginal - lowerOriginal) 
                : 0f;

            // Interpolate or shift each untouched point
            for (int i = startIdx; i <= endIdx; i++)
            {
                float originalPos = isXDirection 
                    ? _glyphZone.Original[i].PointF.X 
                    : _glyphZone.Original[i].PointF.Y;
                
                float newPos;

                // Three cases:
                // 1. Point is below lower reference - shift by delta1
                // 2. Point is above upper reference - shift by delta2
                // 3. Point is between references - interpolate
                if (originalPos <= lowerOriginal)
                {
                    newPos = originalPos + delta1;
                }
                else if (originalPos >= upperOriginal)
                {
                    newPos = originalPos + delta2;
                }
                else
                {
                    newPos = lowerCurrent + ((originalPos - lowerOriginal) * scale);
                }

                // Update the point
                PointF currentPoint = _glyphZone.Current[i].PointF;
                if (isXDirection)
                {
                    _glyphZone.Current[i] = new InterpreterPointF(
                        new PointF(newPos, currentPoint.Y), 
                        _glyphZone.Current[i].TouchState);
                }
                else
                {
                    _glyphZone.Current[i] = new InterpreterPointF(
                        new PointF(currentPoint.X, newPos), 
                        _glyphZone.Current[i].TouchState);
                }
            }
        }

        /// <summary>
        /// Shifts a point by a delta amount in the specified direction for IUP instruction.
        /// </summary>
        private void ShiftPoint(int index, float delta, bool isXDirection)
        {
            PointF currentPoint = _glyphZone.Current[index].PointF;
            PointF newPoint = isXDirection
                ? new PointF(currentPoint.X + delta, currentPoint.Y)
                : new PointF(currentPoint.X, currentPoint.Y + delta);

            _glyphZone.Current[index] = new InterpreterPointF(
                newPoint, 
                _glyphZone.Current[index].TouchState);
        }
    }
}