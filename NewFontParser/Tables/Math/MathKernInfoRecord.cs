using NewFontParser.Reader;

namespace NewFontParser.Tables.Math
{
    public class MathKernInfoRecord
    {
        public MathKernTable? TopRightMathKern { get; }

        public MathKernTable? TopLeftMathKern { get; }

        public MathKernTable? BottomRightMathKern { get; }

        public MathKernTable? BottomLeftMathKern { get; }

        public MathKernInfoRecord(BigEndianReader reader)
        {
            long position = reader.Position;

            ushort topRightMathKernOffset = reader.ReadUShort();
            ushort topLeftMathKernOffset = reader.ReadUShort();
            ushort bottomRightMathKernOffset = reader.ReadUShort();
            ushort bottomLeftMathKernOffset = reader.ReadUShort();

            if (topRightMathKernOffset > 0)
            {
                reader.Seek(position + topRightMathKernOffset);
                TopRightMathKern = new MathKernTable(reader);
            }
            if (topLeftMathKernOffset > 0)
            {
                reader.Seek(position + topLeftMathKernOffset);
                TopLeftMathKern = new MathKernTable(reader);
            }
            if (bottomRightMathKernOffset > 0)
            {
                reader.Seek(position + bottomRightMathKernOffset);
                BottomRightMathKern = new MathKernTable(reader);
            }
            if (bottomLeftMathKernOffset > 0)
            {
                reader.Seek(position + bottomLeftMathKernOffset);
                BottomLeftMathKern = new MathKernTable(reader);
            }
        }
    }
}
