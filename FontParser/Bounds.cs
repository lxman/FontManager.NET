namespace FontParser
{
    /// <summary>
    /// original glyph bounds
    /// </summary>
    public class Bounds
    {
        //TODO: will be changed to => public readonly struct Bounds

        public static readonly Bounds Zero = new Bounds(0, 0, 0, 0);

        public Bounds()
        {
        }

        public Bounds(short xmin, short ymin, short xmax, short ymax)
        {
            XMin = xmin;
            YMin = ymin;
            XMax = xmax;
            YMax = ymax;
        }

        public short XMin { get; }
        public short YMin { get; }
        public short XMax { get; }
        public short YMax { get; }
#if DEBUG

        public override string ToString()
        {
            return "(" + XMin + "," + YMin + "," + XMax + "," + YMax + ")";
        }

#endif
    }
}