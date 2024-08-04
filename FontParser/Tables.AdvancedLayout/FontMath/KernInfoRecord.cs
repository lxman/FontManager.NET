namespace FontParser.Tables.AdvancedLayout.FontMath
{
    public class KernInfoRecord
    {
        //resolved value
        public readonly Kern TopRight;

        public readonly Kern TopLeft;
        public readonly Kern BottomRight;
        public readonly Kern BottomLeft;

        public KernInfoRecord(Kern topRight,
            Kern topLeft,
            Kern bottomRight,
            Kern bottomLeft)
        {
            TopRight = topLeft;
            TopLeft = topLeft;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;
        }
    }
}
