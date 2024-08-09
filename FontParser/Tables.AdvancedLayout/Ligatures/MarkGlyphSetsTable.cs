﻿using System.IO;

namespace FontParser.Tables.AdvancedLayout.Ligatures
{
    //-------------------------
    //CaretValue Format 1
    //-------------------------
    //The first format (CaretValueFormat1) consists of a format identifier (CaretValueFormat),
    //followed by a single coordinate for the caret position (Coordinate). The Coordinate is in design units.

    //This format has the benefits of small size and simplicity, but the Coordinate value cannot be hinted for fine adjustments at different device resolutions.

    //Example 4 at the end of this chapter shows a CaretValueFormat1 table.
    //-------------------------
    //CaretValueFormat1 table: Design units only
    //-------------------------
    //Type 	    Name 	            Description
    //uint16 	CaretValueFormat 	Format identifier-format = 1
    //int16 	Coordinate 	        X or Y value, in design units
    //-------------------------
    //NOTE: int16
    //
    //
    //CaretValue Format 2
    //
    //The second format (CaretValueFormat2) specifies the caret coordinate in terms of a contour point index on a specific glyph.
    //During font hinting, the contour point on the glyph outline may move.
    //The point's final position after hinting provides the final value for rendering a given font size.

    //The table contains a format identifier (CaretValueFormat) and a contour point index (CaretValuePoint).

    //Example 5 at the end of this chapter demonstrates a CaretValueFormat2 table.

    //-------------------------
    //CaretValueFormat2 table: Contour point
    //Type 	    Name 	            Description
    //uint16 	CaretValueFormat 	Format identifier-format = 2
    //uint16 	CaretValuePoint 	Contour point index on glyph
    //-------------------------
    //
    //CaretValue Format 3

    //The third format (CaretValueFormat3) also specifies the value in design units, but,
    //in non-variable fonts, it uses a Device table rather than a contour point to adjust the value.
    //This format offers the advantage of fine-tuning the Coordinate value for any device resolution.
    //(For more information about Device tables, see the chapter, Common Table Formats.)

    //In variable fonts, CaretValueFormat3 must be used to reference variation data to adjust caret positions for different variation instances,
    //if needed. In this case, CaretValueFormat3 specifies an offset to a VariationIndex table, which is a variant of the Device table used for variations.

    //    Note: While separate VariationIndex table references are required for each value that requires variation,
    //two or more values that require the same variation-data values can have offsets that point to the same VariationIndex table,
    //and two or more VariationIndex tables can reference the same variation data entries.

    //    Note: If no VariationIndex table is used for a particular caret position value, then that value is used for all variation instances.

    //The format consists of a format identifier (CaretValueFormat), an X or Y value (Coordinate), and an offset to a Device or VariationIndex table.

    //Example 6 at the end of this chapter shows a CaretValueFormat3 table.

    //-------------------------
    //CaretValueFormat3 table: Design units plus Device or VariationIndex table
    //Type  	Name 	            Description
    //uint16 	CaretValueFormat 	Format identifier-format = 3
    //int16 	Coordinate      	X or Y value, in design units
    //Offset16 	DeviceTable 	    Offset to Device table (non-variable font) / Variation Index table (variable font) for X or Y value-from beginning of CaretValue table
    //-------------------------------------------------------------------------------
    //NOTE:  Offset16
    //-------------------------------------------------------------------------------
    //
    //Mark Attachment Class Definition Table

    //A Mark Attachment Class Definition Table defines the class to which a mark glyph may belong.
    //This table uses the same format as the Class Definition table (for details, see the chapter, Common Table Formats ).

    //Example 7 in this document shows a MarkAttachClassDef table.
    //Mark Glyph Sets Table

    //Mark glyph sets are used in GSUB and GPOS lookups to filter which marks in a string are considered or ignored.
    //Mark glyph sets are defined in a MarkGlyphSets table, which contains offsets to individual sets each represented by a standard Coverage table:

    //---------------------------------------------------------
    //MarkGlyphSetsTable
    //---------------------------------------------------------
    //Type 	    Name 	                    Description
    //uint16 	MarkSetTableFormat 	        Format identifier == 1
    //uint16 	MarkSetCount 	            Number of mark sets defined
    //Offset32 	Coverage [MarkSetCount] 	Array of offsets to mark set coverage tables.
    //---------------------------------------------------------
    //Mark glyph sets are used for the same purpose as mark attachment classes, which is as filters for GSUB and GPOS lookups.
    //Mark glyph sets differ from mark attachment classes, however,
    //in that mark glyph sets may intersect as needed by the font developer.
    //As for mark attachment classes, only one mark glyph set can be referenced in any given lookup.

    //Note that the array of offsets for the Coverage tables uses ULONG, not Offset. ***
    public class MarkGlyphSetsTable
    {
        private ushort _format;
        private uint[] _coverageOffset;

        public static MarkGlyphSetsTable CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            var markGlyphSetsTable = new MarkGlyphSetsTable
            {
                _format = reader.ReadUInt16()
            };
            ushort markSetCount = reader.ReadUInt16();
            uint[] coverageOffset = markGlyphSetsTable._coverageOffset = new uint[markSetCount];
            for (var i = 0; i < markSetCount; ++i)
            {
                //Note that the array of offsets for the Coverage tables uses ULONG
                coverageOffset[i] = reader.ReadUInt32();//
            }

            return markGlyphSetsTable;
        }
    }
}
