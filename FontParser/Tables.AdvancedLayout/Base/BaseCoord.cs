namespace FontParser.Tables.AdvancedLayout.Base
{
    public class BaseCoord
    {
        public readonly ushort baseCoordFormat;

        /// <summary>
        ///  X or Y value, in design units
        /// </summary>
        public readonly short coord;

        public readonly ushort referenceGlyph; //found in format2
        public readonly ushort baseCoordPoint; //found in format2

        public BaseCoord()
        {
        }

        public BaseCoord(ushort baseCoordFormat, short coord)
        {
            this.baseCoordFormat = baseCoordFormat;
            this.coord = coord;
            referenceGlyph = baseCoordPoint = 0;
        }

        public BaseCoord(ushort baseCoordFormat, short coord, ushort referenceGlyph, ushort baseCoordPoint)
        {
            this.baseCoordFormat = baseCoordFormat;
            this.coord = coord;
            this.referenceGlyph = referenceGlyph;
            this.baseCoordPoint = baseCoordPoint;
        }

#if DEBUG

        public override string ToString()
        {
            return "format:" + baseCoordFormat + ",coord=" + coord;
        }

#endif
    }
}