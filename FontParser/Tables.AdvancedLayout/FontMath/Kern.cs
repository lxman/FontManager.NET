namespace FontParser.Tables.AdvancedLayout.FontMath
{
    public class Kern
    {
        //reference =>see  MathKernTable
        public ushort HeightCount;

        public ValueRecord[] CorrectionHeights;
        public ValueRecord[] KernValues;

        public Kern(ushort heightCount, ValueRecord[] correctionHeights, ValueRecord[] kernValues)
        {
            HeightCount = heightCount;
            CorrectionHeights = correctionHeights;
            KernValues = kernValues;
        }

#if DEBUG

        public override string ToString()
        {
            return HeightCount.ToString();
        }

#endif
    }
}
