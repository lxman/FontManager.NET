using System.IO;
using FontParser.Exceptions;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    /// <summary>
    /// To describe an anchor point
    /// </summary>
    public class AnchorPoint
    {
        //Anchor Table

        //A GPOS table uses anchor points to position one glyph with respect to another.
        //Each glyph defines an anchor point, and the text-processing client attaches the glyphs by aligning their corresponding anchor points.

        //To describe an anchor point, an Anchor table can use one of three formats.
        //The first format uses design units to specify a location for the anchor point.
        //The other two formats refine the location of the anchor point using contour points (Format 2) or Device tables (Format 3).
        //In a variable font, the third format uses a VariationIndex table (a variant of a Device table) to
        //reference variation data for adjustment of the anchor position for the current variation instance, as needed.

        public ushort format;
        public short xcoord;
        public short ycoord;

        /// <summary>
        /// an index to a glyph contour point (AnchorPoint)
        /// </summary>
        public ushort refGlyphContourPoint;

        public ushort xdeviceTableOffset;
        public ushort ydeviceTableOffset;

        public static AnchorPoint CreateFrom(BinaryReader reader, long beginAt)
        {
            AnchorPoint anchorPoint = new AnchorPoint();
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);

            switch (anchorPoint.format = reader.ReadUInt16())
            {
                default: throw new OpenFontNotSupportedException();
                case 1:
                    {
                        // AnchorFormat1 table: Design units only
                        //AnchorFormat1 consists of a format identifier (AnchorFormat) and a pair of design unit coordinates (XCoordinate and YCoordinate)
                        //that specify the location of the anchor point.
                        //This format has the benefits of small size and simplicity,
                        //but the anchor point cannot be hinted to adjust its position for different device resolutions.
                        //Value 	Type 	        Description
                        //uint16 	AnchorFormat 	Format identifier, = 1
                        //int16 	XCoordinate 	Horizontal value, in design units
                        //int16 	YCoordinate 	Vertical value, in design units
                        anchorPoint.xcoord = reader.ReadInt16();
                        anchorPoint.ycoord = reader.ReadInt16();
                    }
                    break;

                case 2:
                    {
                        //Anchor Table: Format 2

                        //Like AnchorFormat1, AnchorFormat2 specifies a format identifier (AnchorFormat) and
                        //a pair of design unit coordinates for the anchor point (Xcoordinate and Ycoordinate).

                        //For fine-tuning the location of the anchor point,
                        //AnchorFormat2 also provides an index to a glyph contour point (AnchorPoint)
                        //that is on the outline of a glyph (AnchorPoint).***
                        //Hinting can be used to move the AnchorPoint. In the rendered text,
                        //the AnchorPoint will provide the final positioning data for a given ppem size.

                        //Example 16 at the end of this chapter uses AnchorFormat2.

                        //AnchorFormat2 table: Design units plus contour point
                        //Value 	Type 	        Description
                        //uint16 	AnchorFormat 	Format identifier, = 2
                        //int16 	XCoordinate 	Horizontal value, in design units
                        //int16 	YCoordinate 	Vertical value, in design units
                        //uint16 	AnchorPoint 	Index to glyph contour point

                        anchorPoint.xcoord = reader.ReadInt16();
                        anchorPoint.ycoord = reader.ReadInt16();
                        anchorPoint.refGlyphContourPoint = reader.ReadUInt16();
                    }
                    break;

                case 3:
                    {
                        //Anchor Table: Format 3

                        //Like AnchorFormat1, AnchorFormat3 specifies a format identifier (AnchorFormat) and
                        //locates an anchor point (Xcoordinate and Ycoordinate).
                        //And, like AnchorFormat 2, it permits fine adjustments in variable fonts to the coordinate values.
                        //However, AnchorFormat3 uses Device tables, rather than a contour point, for this adjustment.

                        //With a Device table, a client can adjust the position of the anchor point for any font size and device resolution.
                        //AnchorFormat3 can specify offsets to Device tables for the the X coordinate (XDeviceTable)
                        //and the Y coordinate (YDeviceTable).
                        //If only one coordinate requires adjustment,
                        //the offset to the Device table may be set to NULL for the other coordinate.

                        //In variable fonts, AnchorFormat3 must be used to reference variation data to adjust anchor points for different variation instances,
                        //if needed.
                        //In this case, AnchorFormat3 specifies an offset to a VariationIndex table,
                        //which is a variant of the Device table used for variations.
                        //If no VariationIndex table is used for a particular anchor point X or Y coordinate,
                        //then that value is used for all variation instances.
                        //While separate VariationIndex table references are required for each value that requires variation,
                        //two or more values that require the same variation-data values can have offsets that point to the same VariationIndex table, and two or more VariationIndex tables can reference the same variation data entries.

                        //Example 17 at the end of the chapter shows an AnchorFormat3 table.

                        //AnchorFormat3 table: Design units plus Device or VariationIndex tables
                        //Value 	Type 	        Description
                        //uint16 	AnchorFormat 	Format identifier, = 3
                        //int16 	XCoordinate 	Horizontal value, in design units
                        //int16 	YCoordinate 	Vertical value, in design units
                        //Offset16 	XDeviceTable 	Offset to Device table (non-variable font) / VariationIndex table (variable font) for X coordinate, from beginning of Anchor table (may be NULL)
                        //Offset16 	YDeviceTable 	Offset to Device table (non-variable font) / VariationIndex table (variable font) for Y coordinate, from beginning of Anchor table (may be NULL)

                        anchorPoint.xcoord = reader.ReadInt16();
                        anchorPoint.ycoord = reader.ReadInt16();
                        anchorPoint.xdeviceTableOffset = reader.ReadUInt16();
                        anchorPoint.ydeviceTableOffset = reader.ReadUInt16();
                    }
                    break;
            }
            return anchorPoint;
        }

#if DEBUG

        public override string ToString()
        {
            switch (format)
            {
                default: return "";
                case 1:
                    return format + "(" + xcoord + "," + ycoord + ")";

                case 2:
                    return format + "(" + xcoord + "," + ycoord + "), ref_point=" + refGlyphContourPoint;

                case 3:
                    return format + "(" + xcoord + "," + ycoord + "), xy_device(" + xdeviceTableOffset + "," + ydeviceTableOffset + ")";
            }
        }

#endif
    }
}