//https://docs.microsoft.com/en-us/typography/opentype/spec/math

using System.IO;

namespace FontParser.Tables.AdvancedLayout.FontMath
{
    public class MathTable : TableEntry
    {
        public const string _N = "MATH";
        public override string Name => _N;

        //
        internal Constants _mathConstTable;

        protected override void ReadContentFrom(BinaryReader reader)
        {
            //eg. latin-modern-math-regular.otf, asana-math.otf

            long beginAt = reader.BaseStream.Position;
            //math table header
            //Type          Name    Description
            //uint16        MajorVersion Major version of the MATH table, = 1.
            //uint16        MinorVersion    Minor version of the MATH table, = 0.
            //Offset16      MathConstants   Offset to MathConstants table -from the beginning of MATH table.
            //Offset16      MathGlyphInfo   Offset to MathGlyphInfo table -from the beginning of MATH table.
            //Offset16      MathVariants    Offset to MathVariants table -from the beginning of MATH table.

            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            ushort mathConstants_offset = reader.ReadUInt16();
            ushort mathGlyphInfo_offset = reader.ReadUInt16();
            ushort mathVariants_offset = reader.ReadUInt16();
            //---------------------------------

            //(1)
            reader.BaseStream.Position = beginAt + mathConstants_offset;
            ReadMathConstantsTable(reader);
            //
            //(2)
            reader.BaseStream.Position = beginAt + mathGlyphInfo_offset;
            ReadMathGlyphInfoTable(reader);
            //
            //(3)
            reader.BaseStream.Position = beginAt + mathVariants_offset;
            ReadMathVariantsTable(reader);

            //NOTE: expose  MinConnectorOverlap via _mathConstTable
            _mathConstTable.MinConnectorOverlap = VariantsTable.MinConnectorOverlap;
        }

        /// <summary>
        /// (1) MathConstants
        /// </summary>
        /// <param name="reader"></param>
        private void ReadMathConstantsTable(BinaryReader reader)
        {
            //MathConstants Table

            //The MathConstants table defines miscellaneous constants required to properly position elements of mathematical formulas.
            //These constants belong to several groups of semantically related values such as values needed to properly position accents,
            //values for positioning superscripts and subscripts, and values for positioning elements of fractions.
            //The table also contains general use constants that may affect all parts of the formula,
            //such as axis height and math leading.Note that most of the constants deal with the vertical positioning.

            var mc = new Constants
            {
                ScriptPercentScaleDown = reader.ReadInt16(),
                ScriptScriptPercentScaleDown = reader.ReadInt16(),
                DelimitedSubFormulaMinHeight = reader.ReadUInt16(),
                DisplayOperatorMinHeight = reader.ReadUInt16(),
                //
                //
                Leading = reader.ReadMathValueRecord(),
                AxisHeight = reader.ReadMathValueRecord(),
                AccentBaseHeight = reader.ReadMathValueRecord(),
                FlattenedAccentBaseHeight = reader.ReadMathValueRecord(),
                //
                SubscriptShiftDown = reader.ReadMathValueRecord(),
                SubscriptTopMax = reader.ReadMathValueRecord(),
                SubscriptBaselineDropMin = reader.ReadMathValueRecord(),
                //
                SuperscriptShiftUp = reader.ReadMathValueRecord(),
                SuperscriptShiftUpCramped = reader.ReadMathValueRecord(),
                SuperscriptBottomMin = reader.ReadMathValueRecord(),
                SuperscriptBaselineDropMax = reader.ReadMathValueRecord(),
                //
                SubSuperscriptGapMin = reader.ReadMathValueRecord(),
                SuperscriptBottomMaxWithSubscript = reader.ReadMathValueRecord(),
                SpaceAfterScript = reader.ReadMathValueRecord(),
                //
                UpperLimitGapMin = reader.ReadMathValueRecord(),
                UpperLimitBaselineRiseMin = reader.ReadMathValueRecord(),
                LowerLimitGapMin = reader.ReadMathValueRecord(),
                LowerLimitBaselineDropMin = reader.ReadMathValueRecord(),
                //
                StackTopShiftUp = reader.ReadMathValueRecord(),
                StackTopDisplayStyleShiftUp = reader.ReadMathValueRecord(),
                StackBottomShiftDown = reader.ReadMathValueRecord(),
                StackBottomDisplayStyleShiftDown = reader.ReadMathValueRecord(),
                StackGapMin = reader.ReadMathValueRecord(),
                StackDisplayStyleGapMin = reader.ReadMathValueRecord(),
                //
                StretchStackTopShiftUp = reader.ReadMathValueRecord(),
                StretchStackBottomShiftDown = reader.ReadMathValueRecord(),
                StretchStackGapAboveMin = reader.ReadMathValueRecord(),
                StretchStackGapBelowMin = reader.ReadMathValueRecord(),
                //
                FractionNumeratorShiftUp = reader.ReadMathValueRecord(),
                FractionNumeratorDisplayStyleShiftUp = reader.ReadMathValueRecord(),
                FractionDenominatorShiftDown = reader.ReadMathValueRecord(),
                FractionDenominatorDisplayStyleShiftDown = reader.ReadMathValueRecord(),
                FractionNumeratorGapMin = reader.ReadMathValueRecord(),
                FractionNumDisplayStyleGapMin = reader.ReadMathValueRecord(),
                FractionRuleThickness = reader.ReadMathValueRecord(),
                FractionDenominatorGapMin = reader.ReadMathValueRecord(),
                FractionDenomDisplayStyleGapMin = reader.ReadMathValueRecord(),
                //
                SkewedFractionHorizontalGap = reader.ReadMathValueRecord(),
                SkewedFractionVerticalGap = reader.ReadMathValueRecord(),
                //
                OverbarVerticalGap = reader.ReadMathValueRecord(),
                OverbarRuleThickness = reader.ReadMathValueRecord(),
                OverbarExtraAscender = reader.ReadMathValueRecord(),
                //
                UnderbarVerticalGap = reader.ReadMathValueRecord(),
                UnderbarRuleThickness = reader.ReadMathValueRecord(),
                UnderbarExtraDescender = reader.ReadMathValueRecord(),
                //
                RadicalVerticalGap = reader.ReadMathValueRecord(),
                RadicalDisplayStyleVerticalGap = reader.ReadMathValueRecord(),
                RadicalRuleThickness = reader.ReadMathValueRecord(),
                RadicalExtraAscender = reader.ReadMathValueRecord(),
                RadicalKernBeforeDegree = reader.ReadMathValueRecord(),
                RadicalKernAfterDegree = reader.ReadMathValueRecord(),
                RadicalDegreeBottomRaisePercent = reader.ReadInt16()
            };

            _mathConstTable = mc;
        }

        //--------------------------------------------------------------------------

        /// <summary>
        /// (2) MathGlyphInfo
        /// </summary>
        /// <param name="reader"></param>
        private void ReadMathGlyphInfoTable(BinaryReader reader)
        {
            //MathGlyphInfo Table
            //  The MathGlyphInfo table contains positioning information that is defined on per - glyph basis.
            //  The table consists of the following parts:
            //    Offset to MathItalicsCorrectionInfo table that contains information on italics correction values.
            //    Offset to MathTopAccentAttachment table that contains horizontal positions for attaching mathematical accents.
            //    Offset to Extended Shape coverage table.The glyphs covered by this table are to be considered extended shapes.
            //    Offset to MathKernInfo table that provides per - glyph information for mathematical kerning.

            //  NOTE: Here, and elsewhere in the subclause – please refer to subclause 6.2.4 "Features and Lookups" for description of the coverage table formats.

            long startAt = reader.BaseStream.Position;
            ushort offsetTo_MathItalicsCorrectionInfo_Table = reader.ReadUInt16();
            ushort offsetTo_MathTopAccentAttachment_Table = reader.ReadUInt16();
            ushort offsetTo_Extended_Shape_coverage_Table = reader.ReadUInt16();
            ushort offsetTo_MathKernInfo_Table = reader.ReadUInt16();
            //-----------------------

            //(2.1)
            reader.BaseStream.Position = startAt + offsetTo_MathItalicsCorrectionInfo_Table;
            ReadMathItalicCorrectionInfoTable(reader);

            //(2.2)
            reader.BaseStream.Position = startAt + offsetTo_MathTopAccentAttachment_Table;
            ReadMathTopAccentAttachment(reader);
            //

            //TODO:...
            //The glyphs covered by this table are to be considered extended shapes.
            //These glyphs are variants extended in the vertical direction, e.g.,
            //to match height of another part of the formula.
            //Because their dimensions may be very large in comparison with normal glyphs in the glyph set,
            //the standard positioning algorithms will not produce the best results when applied to them.
            //In the vertical direction, other formula elements will be positioned not relative to those glyphs,
            //but instead to the ink box of the subexpression containing them

            //....

            //(2.3)
            if (offsetTo_Extended_Shape_coverage_Table > 0)
            {
                //may be null, eg. found in font Linux Libertine Regular (https://sourceforge.net/projects/linuxlibertine/)
                _extendedShapeCoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, startAt + offsetTo_Extended_Shape_coverage_Table);
            }

            //(2.4)
            if (offsetTo_MathKernInfo_Table > 0)
            {
                //may be null, eg. latin-modern-math.otf => not found
                //we found this in Asana-math-regular
                reader.BaseStream.Position = startAt + offsetTo_MathKernInfo_Table;
                ReadMathKernInfoTable(reader);
            }
        }

        /// <summary>
        /// (2.1)
        /// </summary>
        internal ItalicsCorrectionInfoTable ItalicCorrectionInfo;

        /// <summary>
        /// (2.1)
        /// </summary>
        /// <param name="reader"></param>
        private void ReadMathItalicCorrectionInfoTable(BinaryReader reader)
        {
            long beginAt = reader.BaseStream.Position;
            ItalicCorrectionInfo = new ItalicsCorrectionInfoTable();
            //MathItalicsCorrectionInfo Table
            //Type           Name                           Description
            //Offset16       Coverage                       Offset to Coverage table - from the beginning of MathItalicsCorrectionInfo table.
            //uint16         ItalicsCorrectionCount         Number of italics correction values.Should coincide with the number of covered glyphs.
            //ValueRecord ItalicsCorrection[ItalicsCorrectionCount]  Array of MathValueRecords defining italics correction values for each covered glyph.
            ushort coverageOffset = reader.ReadUInt16();
            ushort italicCorrectionCount = reader.ReadUInt16();
            ItalicCorrectionInfo.ItalicCorrections = reader.ReadMathValueRecords(italicCorrectionCount);
            //read coverage ...
            if (coverageOffset > 0)
            {
                //may be null?, eg. found in font Linux Libertine Regular (https://sourceforge.net/projects/linuxlibertine/)
                ItalicCorrectionInfo.CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, beginAt + coverageOffset);
            }
        }

        /// <summary>
        /// (2.2)
        /// </summary>
        internal TopAccentAttachmentTable TopAccentAttachmentTable;

        /// <summary>
        /// (2.2)
        /// </summary>
        /// <param name="reader"></param>
        private void ReadMathTopAccentAttachment(BinaryReader reader)
        {
            //MathTopAccentAttachment Table

            //The MathTopAccentAttachment table contains information on horizontal positioning of top math accents.
            //The table consists of the following parts:

            //Coverage of glyphs for which information on horizontal positioning of math accents is provided.
            //To position accents over any other glyph, its geometrical center(with respect to advance width) can be used.

            //Count of covered glyphs.

            //Array of top accent attachment points for each covered glyph, in order of coverage.
            //These attachment points are to be used for finding horizontal positions of accents over characters.
            //It is done by aligning the attachment point of the base glyph with the attachment point of the accent.
            //Note that this is very similar to mark-to-base attachment, but here alignment only happens in the horizontal direction,
            //and the vertical positions of accents are determined by different means.

            //MathTopAccentAttachment Table
            //Type          Name                        Description
            //Offset16      TopAccentCoverage           Offset to Coverage table - from the beginning of MathTopAccentAttachment table.
            //uint16        TopAccentAttachmentCount    Number of top accent attachment point values.Should coincide with the number of covered glyphs.
            //ValueRecord TopAccentAttachment[TopAccentAttachmentCount]  Array of MathValueRecords defining top accent attachment points for each covered glyph.

            long beginAt = reader.BaseStream.Position;
            TopAccentAttachmentTable = new TopAccentAttachmentTable();

            ushort coverageOffset = reader.ReadUInt16();
            ushort topAccentAttachmentCount = reader.ReadUInt16();
            TopAccentAttachmentTable.TopAccentAttachment = reader.ReadMathValueRecords(topAccentAttachmentCount);
            if (coverageOffset > 0)
            {
                //may be null?, eg. found in font Linux Libertine Regular (https://sourceforge.net/projects/linuxlibertine/)
                TopAccentAttachmentTable.CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, beginAt + coverageOffset);
            }
        }

        /// <summary>
        /// (2.3)
        /// </summary>
        internal CoverageTable.CoverageTable _extendedShapeCoverageTable;

        /// <summary>
        /// (2.4)
        /// </summary>
        internal CoverageTable.CoverageTable _mathKernInfoCoverage;

        /// <summary>
        /// (2.4)
        /// </summary>
        internal KernInfoRecord[] _mathKernInfoRecords;

        /// <summary>
        /// (2.4)
        /// </summary>
        /// <param name="reader"></param>
        private void ReadMathKernInfoTable(BinaryReader reader)
        {
            // MathKernInfo Table

            //The MathKernInfo table provides information on glyphs for which mathematical (height - dependent) kerning values are defined.
            //It consists of the following fields:

            //    Coverage of glyphs for which mathematical kerning information is provided.
            //    Count of MathKernInfoRecords.Should coincide with the number of glyphs in Coverage table.
            //    Array of MathKernInfoRecords for each covered glyph, in order of coverage.

            //MathKernInfo Table
            //Type          Name                Description
            //Offset16      MathKernCoverage    Offset to Coverage table - from the beginning of the MathKernInfo table.
            //uint16        MathKernCount       Number of MathKernInfoRecords.
            //KernInfoRecord MathKernInfoRecords[MathKernCount]     Array of MathKernInfoRecords, per - glyph information for mathematical positioning of subscripts and superscripts.

            //...
            //Each KernInfoRecord points to up to four kern tables for each of the corners around the glyph.

            long beginAt = reader.BaseStream.Position;

            ushort mathKernCoverage_offset = reader.ReadUInt16();
            ushort mathKernCount = reader.ReadUInt16();

            //KernInfoRecord Table
            //Each KernInfoRecord points to up to four kern tables for each of the corners around the glyph.

            //    //KernInfoRecord Table
            //    //Type      Name                Description
            //    //Offset16  TopRightKern    Offset to Kern table for top right corner - from the beginning of MathKernInfo table.May be NULL.
            //    //Offset16  TopLeftKern     Offset to Kern table for the top left corner - from the beginning of MathKernInfo table. May be NULL.
            //    //Offset16  BottomRightKern Offset to Kern table for bottom right corner - from the beginning of MathKernInfo table. May be NULL.
            //    //Offset16  BottomLeftKern  Offset to Kern table for bottom left corner - from the beginning of MathKernInfo table. May be NULL.

            ushort[] allKernRecOffset = reader.ReadUInt16Array(4 * mathKernCount);//***

            //read each kern table
            _mathKernInfoRecords = new KernInfoRecord[mathKernCount];
            var index = 0;
            ushort m_kern_offset = 0;

            for (var i = 0; i < mathKernCount; ++i)
            {
                //top-right
                m_kern_offset = allKernRecOffset[index];

                Kern topRight = null, topLeft = null, bottomRight = null, bottomLeft = null;

                if (m_kern_offset > 0)
                {
                    reader.BaseStream.Position = beginAt + m_kern_offset;
                    topRight = ReadMathKernTable(reader);
                }
                //top-left
                m_kern_offset = allKernRecOffset[index + 1];
                if (m_kern_offset > 0)
                {
                    reader.BaseStream.Position = beginAt + m_kern_offset;
                    topLeft = ReadMathKernTable(reader);
                }
                //bottom-right
                m_kern_offset = allKernRecOffset[index + 2];
                if (m_kern_offset > 0)
                {
                    reader.BaseStream.Position = beginAt + m_kern_offset;
                    bottomRight = ReadMathKernTable(reader);
                }
                //bottom-left
                m_kern_offset = allKernRecOffset[index + 3];
                if (m_kern_offset > 0)
                {
                    reader.BaseStream.Position = beginAt + m_kern_offset;
                    bottomLeft = ReadMathKernTable(reader);
                }

                _mathKernInfoRecords[i] = new KernInfoRecord(topRight, topLeft, bottomRight, bottomLeft);

                index += 4;//***
            }

            //-----
            _mathKernInfoCoverage = CoverageTable.CoverageTable.CreateFrom(reader, beginAt + mathKernCoverage_offset);
        }

        /// <summary>
        /// (2.4)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static Kern ReadMathKernTable(BinaryReader reader)
        {
            //The Kern table contains adjustments to horizontal positions of subscripts and superscripts
            //The kerning algorithm consists of the following steps:

            //1. Calculate vertical positions of subscripts and superscripts.
            //2. Set the default horizontal position for the subscript immediately after the base glyph.
            //3. Set the default horizontal position for the superscript as shifted relative to the position of the subscript by the italics correction of the base glyph.
            //4. Based on the vertical positions, calculate the height of the top/ bottom for the bounding boxes of sub/superscript relative to the base glyph, and the height of the top/ bottom of the base relative to the super/ subscript.These will be the correction heights.
            //5. Get the kern values corresponding to these correction heights for the appropriate corner of the base glyph and sub/superscript glyph from the appropriate Kern tables.Kern the default horizontal positions by the minimum of sums of those values at the correction heights for the base and for the sub/superscript.
            //6. If either one of the base or superscript expression has to be treated as a box not providing glyph
            //Kern Table
            //Type              Name                                Description
            //uint16            HeightCount                         Number of heights on which the kern value changes.
            //ValueRecord   CorrectionHeight[HeightCount]       Array of correction heights at which the kern value changes.Sorted by the height value in design units.
            //ValueRecord   KernValue[HeightCount+1]            Array of kern values corresponding to heights.

            //First value is the kern value for all heights less or equal than the first height in this table.
            //Last value is the value to be applied for all heights greater than the last height in this table.
            //Negative values are interpreted as "move glyphs closer to each other".

            ushort heightCount = reader.ReadUInt16();
            return new Kern(heightCount,
                reader.ReadMathValueRecords(heightCount),
                reader.ReadMathValueRecords(heightCount + 1)
            );
        }

        //--------------------------------------------------------------------------

        /// <summary>
        /// (3)
        /// </summary>
        internal VariantsTable VariantsTable;

        /// <summary>
        /// (3) MathVariants
        /// </summary>
        /// <param name="reader"></param>
        private void ReadMathVariantsTable(BinaryReader reader)
        {
            //MathVariants Table

            //The MathVariants table solves the following problem:
            //given a particular default glyph shape and a certain width or height,
            //find a variant shape glyph(or construct created by putting several glyph together)
            //that has the required measurement.
            //This functionality is needed for growing the parentheses to match the height of the expression within,
            //growing the radical sign to match the height of the expression under the radical,
            //stretching accents like tilde when they are put over several characters,
            //for stretching arrows, horizontal curly braces, and so forth.

            //The MathVariants table consists of the following fields:

            //  Count and coverage of glyph that can grow in the vertical direction.
            //  Count and coverage of glyphs that can grow in the horizontal direction.
            //  MinConnectorOverlap defines by how much two glyphs need to overlap with each other when used to construct a larger shape.
            //  Each glyph to be used as a building block in constructing extended shapes will have a straight part at either or both ends.
            //  This connector part is used to connect that glyph to other glyphs in the assembly.
            //  These connectors need to overlap to compensate for rounding errors and hinting corrections at a lower resolution.
            //  The MinConnectorOverlap value tells how much overlap is necessary for this particular font.

            //  Two arrays of offsets to MathGlyphConstruction tables:
            //  one array for glyphs that grow in the vertical direction,
            //  and the other array for glyphs that grow in the horizontal direction.
            //  The arrays must be arranged in coverage order and have specified sizes.

            //MathVariants Table
            //Type          Name                    Description
            //uint16        MinConnectorOverlap     Minimum overlap of connecting glyphs during glyph construction, in design units.
            //Offset16      VertGlyphCoverage       Offset to Coverage table - from the beginning of MathVariants table.
            //Offset16      HorizGlyphCoverage      Offset to Coverage table - from the beginning of MathVariants table.
            //uint16        VertGlyphCount          Number of glyphs for which information is provided for vertically growing variants.
            //uint16        HorizGlyphCount         Number of glyphs for which information is provided for horizontally growing variants.
            //Offset16      VertGlyphConstruction[VertGlyphCount]  Array of offsets to MathGlyphConstruction tables - from the beginning of the MathVariants table, for shapes growing in vertical direction.
            //Offset16      HorizGlyphConstruction[HorizGlyphCount]    Array of offsets to MathGlyphConstruction tables - from the beginning of the MathVariants table, for shapes growing in horizontal direction.

            VariantsTable = new VariantsTable();

            long beginAt = reader.BaseStream.Position;
            //
            VariantsTable.MinConnectorOverlap = reader.ReadUInt16();
            //
            ushort vertGlyphCoverageOffset = reader.ReadUInt16();
            ushort horizGlyphCoverageOffset = reader.ReadUInt16();
            ushort vertGlyphCount = reader.ReadUInt16();
            ushort horizGlyphCount = reader.ReadUInt16();
            ushort[] vertGlyphConstructions = reader.ReadUInt16Array(vertGlyphCount);
            ushort[] horizonGlyphConstructions = reader.ReadUInt16Array(horizGlyphCount);
            //

            if (vertGlyphCoverageOffset > 0)
            {
                VariantsTable.vertCoverage = CoverageTable.CoverageTable.CreateFrom(reader, beginAt + vertGlyphCoverageOffset);
            }

            if (horizGlyphCoverageOffset > 0)
            {
                //may be null?, e.g. found in font Linux Libertine Regular (https://sourceforge.net/projects/linuxlibertine/)
                VariantsTable.horizCoverage = CoverageTable.CoverageTable.CreateFrom(reader, beginAt + horizGlyphCoverageOffset);
            }

            //read math construction table

            //(3.1)
            //vertical
            var vertGlyphConstructionTables = VariantsTable.vertConstructionTables = new GlyphConstruction[vertGlyphCount];
            for (var i = 0; i < vertGlyphCount; ++i)
            {
                reader.BaseStream.Position = beginAt + vertGlyphConstructions[i];
                vertGlyphConstructionTables[i] = ReadMathGlyphConstructionTable(reader);
            }

            //(3.2)
            //horizon
            var horizGlyphConstructionTables = VariantsTable.horizConstructionTables = new GlyphConstruction[horizGlyphCount];
            for (var i = 0; i < horizGlyphCount; ++i)
            {
                reader.BaseStream.Position = beginAt + horizonGlyphConstructions[i];
                horizGlyphConstructionTables[i] = ReadMathGlyphConstructionTable(reader);
            }
        }

        /// <summary>
        /// (3.1, 3.2)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private GlyphConstruction ReadMathGlyphConstructionTable(BinaryReader reader)
        {
            //MathGlyphConstruction Table
            //The MathGlyphConstruction table provides information on finding or assembling extended variants for one particular glyph.
            //It can be used for shapes that grow in both horizontal and vertical directions.

            //The first entry is the offset to the GlyphAssembly table that specifies how the shape for this glyph can be assembled
            //from parts found in the glyph set of the font.
            //If no such assembly exists, this offset will be set to NULL.

            //The MathGlyphConstruction table also contains the count and array of ready-made glyph variants for the specified glyph.
            //Each variant consists of the glyph index and this glyph’s measurement in the direction of extension(vertical or horizontal).

            //Note that it is quite possible that both the GlyphAssembly table and some variants are defined for a particular glyph.
            //For example, the font may specify several variants for curly braces of different sizes,
            //and a general mechanism of how larger versions of curly braces can be constructed by stacking parts found in the glyph set.
            //First attempt is made to find glyph among provided variants.
            //
            //However, if the required size is bigger than all glyph variants provided,
            //the general mechanism can be employed to typeset the curly braces as a glyph assembly.

            //MathGlyphConstruction Table
            //Type          Name            Description
            //Offset16      GlyphAssembly   Offset to GlyphAssembly table for this shape - from the beginning of MathGlyphConstruction table.May be NULL.
            //uint16        VariantCount    Count of glyph growing variants for this glyph.
            //MathGlyphVariantRecord MathGlyphVariantRecord [VariantCount]   MathGlyphVariantRecords for alternative variants of the glyphs.

            long beginAt = reader.BaseStream.Position;

            var glyphConstructionTable = new GlyphConstruction();

            ushort glyphAsmOffset = reader.ReadUInt16();
            ushort variantCount = reader.ReadUInt16();

            var variantRecords = glyphConstructionTable.glyphVariantRecords = new GlyphVariantRecord[variantCount];

            for (var i = 0; i < variantCount; ++i)
            {
                variantRecords[i] = new GlyphVariantRecord(
                    reader.ReadUInt16(),
                    reader.ReadUInt16()
                );
            }

            //read glyph asm table
            if (glyphAsmOffset > 0)//may be NULL
            {
                reader.BaseStream.Position = beginAt + glyphAsmOffset;
                FillGlyphAssemblyInfo(reader, glyphConstructionTable);
            }
            return glyphConstructionTable;
        }

        /// <summary>
        /// (3.1, 3.2,)
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="glyphConstruction"></param>
        private static void FillGlyphAssemblyInfo(BinaryReader reader, GlyphConstruction glyphConstruction)
        {
            //since MathGlyphConstructionTable: GlyphAssembly is 1:1
            //---------
            //GlyphAssembly Table
            //The GlyphAssembly table specifies how the shape for a particular glyph can be constructed from parts found in the glyph set.
            //The table defines the italics correction of the resulting assembly, and a number of parts that have to be put together to form the required shape.

            //GlyphAssembly
            //Type              Name                    Description
            //ValueRecord   ItalicsCorrection       Italics correction of this GlyphAssembly.Should not depend on the assembly size.
            //uint16            PartCount               Number of parts in this assembly.
            //GlyphPartRecord   PartRecords[PartCount]  Array of part records,
            //                                          from left to right  (for assemblies that extend horizontally) and
            //                                          bottom to top(for assemblies that extend vertically)..

            //The result of the assembly process is an array of glyphs with an offset specified for each of those glyphs.
            //When drawn consecutively at those offsets, the glyphs should combine correctly and produce the required shape.

            //The offsets in the direction of growth (advance offsets), as well as the number of parts labeled as extenders,
            //are determined based on the size requirement for the resulting assembly.

            //Note that the glyphs comprising the assembly should be designed so that they align properly in the direction that is orthogonal to the direction of growth.

            glyphConstruction.GlyphAsm_ItalicCorrection = reader.ReadMathValueRecord();
            ushort partCount = reader.ReadUInt16();
            var partRecords = glyphConstruction.GlyphAsm_GlyphPartRecords = new GlyphPartRecord[partCount];
            for (var i = 0; i < partCount; ++i)
            {
                partRecords[i] = new GlyphPartRecord(
                    reader.ReadUInt16(),
                    reader.ReadUInt16(),
                    reader.ReadUInt16(),
                    reader.ReadUInt16(),
                    reader.ReadUInt16()
                );
            }
        }
    }
}