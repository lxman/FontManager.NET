namespace FontParser.Tables.AdvancedLayout.FontMath
{
    public readonly struct GlyphPartRecord
    {
        //Thus, a GlyphPartRecord consists of the following fields:
        //1) Glyph ID for the part.
        //2) Lengths of the connectors on each end of the glyph.
        //      The connectors are straight parts of the glyph that can be used to link it with the next or previous part.
        //      The connectors of neighboring parts can overlap, which provides flexibility of how these glyphs can be put together.However, the overlap should not be less than the value of MinConnectorOverlap defined in the MathVariants tables, and it should not exceed the length of either of two overlapping connectors.If the part does not have a connector on one of its sides, the corresponding length should be set to zero.

        //3) The full advance of the part.
        //      It is also used to determine the measurement of the result by using the following formula:

        //  *** Size of Assembly = Offset of the Last Part + Full Advance of the Last Part ***

        //4) PartFlags is the last field.
        //      It identifies a number of parts as extenders – those parts that can be repeated(that is, multiple instances of them can be used in place of one) or skipped altogether.Usually the extenders are vertical or horizontal bars of the appropriate thickness, aligned with the rest of the assembly.

        //To ensure that the width/height is distributed equally and the symmetry of the shape is preserved,
        //following steps can be used by math handling client.

        //1. Assemble all parts by overlapping connectors by maximum amount, and removing all extenders.
        //  This gives the smallest possible result.

        //2. Determine how much extra width/height can be distributed into all connections between neighboring parts.
        //   If that is enough to achieve the size goal, extend each connection equally by changing overlaps of connectors to finish the job.
        //3. If all connections have been extended to minimum overlap and further growth is needed, add one of each extender,
        //and repeat the process from the first step.

        //Note that for assemblies growing in vertical direction,
        //the distribution of height or the result between ascent and descent is not defined.
        //The math handling client is responsible for positioning the resulting assembly relative to the baseline.

        //GlyphPartRecord Table
        //Type      Name                    Description
        //uint16    Glyph                   Glyph ID for the part.
        //uint16    StartConnectorLength    Advance width/ height of the straight bar connector material, in design units, is at the beginning of the glyph, in the direction of the extension.
        //uint16    EndConnectorLength      Advance width/ height of the straight bar connector material, in design units, is at the end of the glyph, in the direction of the extension.
        //uint16    FullAdvance             Full advance width/height for this part, in the direction of the extension.In design units.
        //uint16    PartFlags               Part qualifiers. PartFlags enumeration currently uses only one bit:
        //                                       0x0001 fExtender If set, the part can be skipped or repeated.
        //                                       0xFFFE Reserved.

        public readonly ushort GlyphId;
        public readonly ushort StartConnectorLength;
        public readonly ushort EndConnectorLength;
        public readonly ushort FullAdvance;
        public readonly ushort PartFlags;
        public bool IsExtender => (PartFlags & 0x0001) != 0;

        public GlyphPartRecord(ushort glyphId, ushort startConnectorLength, ushort endConnectorLength, ushort fullAdvance, ushort partFlags)
        {
            GlyphId = glyphId;
            StartConnectorLength = startConnectorLength;
            EndConnectorLength = endConnectorLength;
            FullAdvance = fullAdvance;
            PartFlags = partFlags;
        }

#if DEBUG

        public override string ToString()
        {
            return "glyph_id:" + GlyphId;
        }

#endif
    }
}
