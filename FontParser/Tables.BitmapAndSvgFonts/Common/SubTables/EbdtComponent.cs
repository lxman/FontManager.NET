namespace FontParser.Tables.BitmapAndSvgFonts.Common.SubTables
{
    //EbdtComponent Record

    //The EbdtComponent record is used in glyph bitmap data formats 8 and 9.
    //Type Name    Description
    //uint16  glyphID Component glyph ID
    //int8 xOffset     Position of component left
    //int8 yOffset     Position of component top

    //The EbdtComponent record contains the glyph ID of the component, which can be used to look up the location of component glyph data in the EBLC table, as well as xOffset and yOffset values, which specify where to position the top-left corner of the component in the composite.Nested composites (a composite of composites) are allowed, and the number of nesting levels is determined by implementation stack space.

    public struct EbdtComponent
    {
        public ushort glyphID;
        public sbyte xOffset;
        public sbyte yOffset;
    }
}
