using System.IO;
using FontParser.Exceptions;
using FontParser.Typeface;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    /// <summary>
    /// sub table of a lookup list
    /// </summary>
    public class LookupTable
    {
#if DEBUG
        public int dbugLkIndex;
#endif

        public readonly ushort lookupFlags;
        public readonly ushort markFilteringSet;

        //--------------------------
        public LookupTable(ushort lookupFlags, ushort markFilteringSet)
        {
            this.lookupFlags = lookupFlags;
            this.markFilteringSet = markFilteringSet;
        }

        public void DoGlyphPosition(IGlyphPositions inputGlyphs, int startAt, int len)
        {
            foreach (LookupSubTable subTable in SubTables)
            {
                subTable.DoGlyphPosition(inputGlyphs, startAt, len);
                //update len
                len = inputGlyphs.Count;
            }
        }

        public LookupSubTable[] SubTables { get; internal set; }

        public static LookupSubTable ReadSubTable(int lookupType, BinaryReader reader, long subTableStartAt)
        {
            switch (lookupType)
            {
                case 1: return ReadLookupType1(reader, subTableStartAt);
                case 2: return ReadLookupType2(reader, subTableStartAt);
                case 3: return ReadLookupType3(reader, subTableStartAt);
                case 4: return ReadLookupType4(reader, subTableStartAt);
                case 5: return ReadLookupType5(reader, subTableStartAt);
                case 6: return ReadLookupType6(reader, subTableStartAt);
                case 7: return ReadLookupType7(reader, subTableStartAt);
                case 8: return ReadLookupType8(reader, subTableStartAt);
                case 9: return ReadLookupType9(reader, subTableStartAt);
            }

            return new UnImplementedLookupSubTable($"GPOS Lookup Type {lookupType}");
        }

        public static int FindGlyphBackwardByKind(IGlyphPositions inputGlyphs, GlyphClassKind kind, int pos, int lim)
        {
            for (int i = pos; --i >= lim;)
            {
                if (inputGlyphs.GetGlyphClassKind(i) == kind)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Lookup Type 1: Single Adjustment Positioning Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType1(BinaryReader reader, long subTableStartAt)
        {
            // Single Adjustment Positioning: Format 1
            // Value         Type          Description
            // uint16        PosFormat     Format identifier-format = 1
            // Offset16      Coverage      Offset to Coverage table-from beginning of SinglePos subtable
            // uint16        ValueFormat   Defines the types of data in the ValueRecord
            // ValueRecord   Value         Defines positioning value(s)-applied to all glyphs in the Coverage table

            // Single Adjustment Positioning: Format 2
            // Value         Type                Description
            // USHORT        PosFormat           Format identifier-format = 2
            // Offset16      Coverage            Offset to Coverage table-from beginning of SinglePos subtable
            // uint16        ValueFormat         Defines the types of data in the ValueRecord
            // uint16        ValueCount          Number of ValueRecords
            // ValueRecord   Value[ValueCount]   Array of ValueRecords-positioning values applied to glyphs

            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            ushort coverage = reader.ReadUInt16();
            ushort valueFormat = reader.ReadUInt16();
            switch (format)
            {
                default: throw new OpenFontNotSupportedException();
                case 1:
                    {
                        var valueRecord = ValueRecord.CreateFrom(reader, valueFormat);
                        var coverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverage);
                        return new LkSubTableType1(coverageTable, valueRecord);
                    }
                case 2:
                    {
                        ushort valueCount = reader.ReadUInt16();
                        var valueRecords = new ValueRecord[valueCount];
                        for (var n = 0; n < valueCount; ++n)
                        {
                            valueRecords[n] = ValueRecord.CreateFrom(reader, valueFormat);
                        }
                        var coverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverage);
                        return new LkSubTableType1(coverageTable, valueRecords);
                    }
            }
        }

        /// <summary>
        ///  Lookup Type 2: Pair Adjustment Positioning Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType2(BinaryReader reader, long subTableStartAt)
        {
            //A pair adjustment positioning subtable(PairPos) is used to adjust the positions of two glyphs
            //in relation to one another-for instance,
            //to specify kerning data for pairs of glyphs.
            //
            //Compared to a typical kerning table, however, a PairPos subtable offers more flexiblity and
            //precise control over glyph positioning.

            //The PairPos subtable can adjust each glyph in a pair independently in both the X and Y directions,
            //and it can explicitly describe the particular type of adjustment applied to each glyph.
            //
            //PairPos subtables can be either of two formats:
            //1) one that identifies glyphs individually by index(Format 1),
            //or 2) one that identifies glyphs by class (Format 2).
            //-----------------------------------------------
            //FORMAT1:
            //Format 1 uses glyph indices to access positioning data for one or more specific pairs of glyphs
            //All pairs are specified in the order determined by the layout direction of the text.
            //
            //Note: For text written from right to left, the right - most glyph will be the first glyph in a pair;
            //conversely, for text written from left to right, the left - most glyph will be first.
            //
            //A PairPosFormat1 subtable contains a format identifier(PosFormat) and two ValueFormats:
            //ValueFormat1 applies to the ValueRecord of the first glyph in each pair.
            //ValueRecords for all first glyphs must use ValueFormat1.
            //If ValueFormat1 is set to zero(0),
            //the corresponding glyph has no ValueRecord and, therefore, should not be repositioned.
            //
            //ValueFormat2 applies to the ValueRecord of the second glyph in each pair.
            //ValueRecords for all second glyphs must use ValueFormat2.
            //If ValueFormat2 is set to null, then the second glyph of the pair is the “next” glyph for which a lookup should be performed.
            //
            //A PairPos subtable also defines an offset to a Coverage table(Coverage) that lists the indices of the first glyphs in each pair.
            //More than one pair can have the same first glyph, but the Coverage table will list that glyph only once.
            //
            //The subtable also contains an array of offsets to PairSet tables(PairSet) and a count of the defined tables(PairSetCount).
            //The PairSet array contains one offset for each glyph listed in the Coverage table and uses the same order as the Coverage Index.

            //-----------------
            //PairPosFormat1 subtable: Adjustments for glyph pairs
            //uint16 	PosFormat 	    Format identifier-format = 1
            //Offset16 	Coverage 	    Offset to Coverage table-from beginning of PairPos subtable-only the first glyph in each pair
            //uint16 	ValueFormat1 	Defines the types of data in ValueRecord1-for the first glyph in the pair -may be zero (0)
            //uint16 	ValueFormat2 	Defines the types of data in ValueRecord2-for the second glyph in the pair -may be zero (0)
            //uint16 	PairSetCount 	Number of PairSet tables
            //Offset16 	PairSetOffset[PairSetCount] Array of offsets to PairSet tables-from beginning of PairPos subtable-ordered by Coverage Index                //
            //-----------------
            //
            //PairSet table
            //Value 	Type 	            Description
            //uint16 	PairValueCount 	    Number of PairValueRecords
            //struct 	PairValueRecord[PairValueCount] 	Array of PairValueRecords-ordered by GlyphID of the second glyph
            //-----------------
            //A PairValueRecord specifies the second glyph in a pair (SecondGlyph) and defines a ValueRecord for each glyph (Value1 and Value2).
            //If ValueFormat1 is set to zero (0) in the PairPos subtable, ValueRecord1 will be empty; similarly, if ValueFormat2 is 0, Value2 will be empty.

            //PairValueRecord
            //Value 	    Type 	        Description
            //GlyphID 	    SecondGlyph 	GlyphID of second glyph in the pair-first glyph is listed in the Coverage table
            //ValueRecord 	Value1 	        Positioning data for the first glyph in the pair
            //ValueRecord 	Value2 	        Positioning data for the second glyph in the pair
            //-----------------------------------------------

            //PairPosFormat2 subtable: Class pair adjustment
            //Value 	Type 	            Description
            //uint16 	PosFormat 	        Format identifier-format = 2
            //Offset16 	Coverage 	        Offset to Coverage table-from beginning of PairPos subtable-for the first glyph of the pair
            //uint16 	ValueFormat1 	    ValueRecord definition-for the first glyph of the pair-may be zero (0)
            //uint16 	ValueFormat2 	    ValueRecord definition-for the second glyph of the pair-may be zero (0)
            //Offset16 	ClassDef1 	        Offset to ClassDef table-from beginning of PairPos subtable-for the first glyph of the pair
            //Offset16 	ClassDef2 	        Offset to ClassDef table-from beginning of PairPos subtable-for the second glyph of the pair
            //uint16 	Class1Count 	    Number of classes in ClassDef1 table-includes Class0
            //uint16 	Class2Count 	    Number of classes in ClassDef2 table-includes Class0
            //struct 	Class1Record[Class1Count] 	Array of Class1 records-ordered by Class1

            //Each Class1Record contains an array of Class2Records (Class2Record), which also are ordered by class value.
            //One Class2Record must be declared for each class in the ClassDef2 table, including Class 0.
            //--------------------------------
            //Class1Record
            //Value 	Type 	Description
            //struct 	Class2Record[Class2Count] 	Array of Class2 records-ordered by Class2
            //--------------------------------

            //A Class2Record consists of two ValueRecords,
            //one for the first glyph in a class pair (Value1) and one for the second glyph (Value2).
            //If the PairPos subtable has a value of zero (0) for ValueFormat1 or ValueFormat2,
            //the corresponding record (ValueRecord1 or ValueRecord2) will be empty.

            //Class2Record
            //--------------------------------
            //Value 	    Type 	Description
            //ValueRecord 	Value1 	Positioning for first glyph-empty if ValueFormat1 = 0
            //ValueRecord 	Value2 	Positioning for second glyph-empty if ValueFormat2 = 0
            //--------------------------------

            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default:
                    return new UnImplementedLookupSubTable($"GPOS Lookup Table Type 2 Format {format}");

                case 1:
                    {
                        ushort coverage = reader.ReadUInt16();
                        ushort value1Format = reader.ReadUInt16();
                        ushort value2Format = reader.ReadUInt16();
                        ushort pairSetCount = reader.ReadUInt16();
                        ushort[] pairSetOffsetArray = reader.ReadUInt16Array(pairSetCount);
                        PairSetTable[] pairSetTables = new PairSetTable[pairSetCount];
                        for (var n = 0; n < pairSetCount; ++n)
                        {
                            reader.BaseStream.Seek(subTableStartAt + pairSetOffsetArray[n], SeekOrigin.Begin);
                            var pairSetTable = new PairSetTable();
                            pairSetTable.ReadFrom(reader, value1Format, value2Format);
                            pairSetTables[n] = pairSetTable;
                        }
                        var subTable = new LkSubTableType2Fmt1(pairSetTables)
                        {
                            //coverage
                            CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverage)
                        };
                        return subTable;
                    }
                case 2:
                    {
                        ushort coverage = reader.ReadUInt16();
                        ushort value1Format = reader.ReadUInt16();
                        ushort value2Format = reader.ReadUInt16();
                        ushort classDef1_offset = reader.ReadUInt16();
                        ushort classDef2_offset = reader.ReadUInt16();
                        ushort class1Count = reader.ReadUInt16();
                        ushort class2Count = reader.ReadUInt16();

                        Lk2Class1Record[] class1Records = new Lk2Class1Record[class1Count];
                        for (var c1 = 0; c1 < class1Count; ++c1)
                        {
                            //for each c1 record

                            Lk2Class2Record[] class2Records = new Lk2Class2Record[class2Count];
                            for (var c2 = 0; c2 < class2Count; ++c2)
                            {
                                class2Records[c2] = new Lk2Class2Record(
                                      ValueRecord.CreateFrom(reader, value1Format),
                                      ValueRecord.CreateFrom(reader, value2Format));
                            }
                            class1Records[c1] = new Lk2Class1Record(class2Records);
                        }

                        var subTable = new LkSubTableType2Fmt2(class1Records,
                                            ClassDefTable.ClassDefTable.CreateFrom(reader, subTableStartAt + classDef1_offset),
                                            ClassDefTable.ClassDefTable.CreateFrom(reader, subTableStartAt + classDef2_offset))
                        {
                            CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverage)
                        };

                        return subTable;
                    }
            }
        }

        /// <summary>
        /// Lookup Type 3: Cursive Attachment Positioning Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType3(BinaryReader reader, long subTableStartAt)
        {
            // TODO: implement this

            return new UnImplementedLookupSubTable("GPOS Lookup Table Type 3");
        }

        /// <summary>
        /// Lookup Type 4: MarkToBase Attachment Positioning, or called (MarkBasePos) table
        /// </summary>

        /// <summary>
        /// Lookup Type 4: MarkToBase Attachment Positioning Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType4(BinaryReader reader, long subTableStartAt)
        {
            //The MarkToBase attachment (MarkBasePos) subtable is used to position combining mark glyphs with respect to base glyphs.
            //For example, the Arabic, Hebrew, and Thai scripts combine vowels, diacritical marks, and tone marks with base glyphs.

            //In the MarkBasePos subtable, every mark glyph has an anchor point and is associated with a class of marks.
            //Each base glyph then defines an anchor point for each class of marks it uses.

            //For example, assume two mark classes: all marks positioned above base glyphs (Class 0),
            //and all marks positioned below base glyphs (Class 1).
            //In this case, each base glyph that uses these marks would define two anchor points,
            //one for attaching the mark glyphs listed in Class 0,
            //and one for attaching the mark glyphs listed in Class 1.

            //To identify the base glyph that combines with a mark,
            //the text-processing client must look backward in the glyph string from the mark to the preceding base glyph.
            //To combine the mark and base glyph, the client aligns their attachment points,
            //positioning the mark with respect to the final pen point (advance) position of the base glyph.

            //The MarkToBase Attachment subtable has one format: MarkBasePosFormat1.
            //The subtable begins with a format identifier (PosFormat) and
            //offsets to two Coverage tables: one that lists all the mark glyphs referenced in the subtable (MarkCoverage),
            //and one that lists all the base glyphs referenced in the subtable (BaseCoverage).

            //For each mark glyph in the MarkCoverage table,
            //a record specifies its class and an offset to the Anchor table that describes the mark's attachment point (MarkRecord).
            //A mark class is identified by a specific integer, called a class value.
            //ClassCount specifies the total number of distinct mark classes defined in all the MarkRecords.

            //The MarkBasePosFormat1 subtable also contains an offset to a MarkArray table,
            //which contains all the MarkRecords stored in an array (MarkRecord) by MarkCoverage Index.
            //A MarkArray table also contains a count of the defined MarkRecords (MarkCount).
            //(For details about MarkArrays and MarkRecords, see the end of this chapter.)

            //The MarkBasePosFormat1 subtable also contains an offset to a BaseArray table (BaseArray).

            //MarkBasePosFormat1 subtable: MarkToBase attachment point
            //----------------------------------------------
            //Value 	Type 	        Description
            //uint16 	PosFormat 	    Format identifier-format = 1
            //Offset16 	MarkCoverage 	Offset to MarkCoverage table-from beginning of MarkBasePos subtable ( all the mark glyphs referenced in the subtable)
            //Offset16 	BaseCoverage 	Offset to BaseCoverage table-from beginning of MarkBasePos subtable (all the base glyphs referenced in the subtable)
            //uint16 	ClassCount 	    Number of classes defined for marks
            //Offset16 	MarkArray 	    Offset to MarkArray table-from beginning of MarkBasePos subtable
            //Offset16 	BaseArray 	    Offset to BaseArray table-from beginning of MarkBasePos subtable
            //----------------------------------------------

            //The BaseArray table consists of an array (BaseRecord) and count (BaseCount) of BaseRecords.
            //The array stores the BaseRecords in the same order as the BaseCoverage Index.
            //Each base glyph in the BaseCoverage table has a BaseRecord.

            //BaseArray table
            //Value 	Type 	Description
            //uint16 	BaseCount 	Number of BaseRecords
            //struct 	BaseRecord[BaseCount] 	Array of BaseRecords-in order of BaseCoverage Index

            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            if (format != 1)
            {
                return new UnImplementedLookupSubTable($"GPOS Lookup Sub Table Type 4 Format {format}");
            }
            ushort markCoverageOffset = reader.ReadUInt16(); //offset from
            ushort baseCoverageOffset = reader.ReadUInt16();
            ushort markClassCount = reader.ReadUInt16();
            ushort markArrayOffset = reader.ReadUInt16();
            ushort baseArrayOffset = reader.ReadUInt16();

            //read mark array table
            var lookupType4 = new LkSubTableType4
            {
                MarkCoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + markCoverageOffset),
                BaseCoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + baseCoverageOffset),
                MarkArrayTable = MarkArrayTable.CreateFrom(reader, subTableStartAt + markArrayOffset),
                BaseArrayTable = BaseArrayTable.CreateFrom(reader, subTableStartAt + baseArrayOffset, markClassCount)
            };
#if DEBUG
            //lookupType4.dbugTest();
#endif
            return lookupType4;
        }

        /// <summary>
        /// Lookup Type 5: MarkToLigature Attachment Positioning Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType5(BinaryReader reader, long subTableStartAt)
        {
            //uint16 	PosFormat 	        Format identifier-format = 1
            //Offset16 	MarkCoverage 	    Offset to Mark Coverage table-from beginning of MarkLigPos subtable
            //Offset16 	LigatureCoverage 	Offset to Ligature Coverage table-from beginning of MarkLigPos subtable
            //uint16 	ClassCount 	        Number of defined mark classes
            //Offset16 	MarkArray 	        Offset to MarkArray table-from beginning of MarkLigPos subtable
            //Offset16 	LigatureArray 	    Offset to LigatureArray table-from beginning of MarkLigPos subtable

            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            if (format != 1)
            {
                return new UnImplementedLookupSubTable($"GPOS Lookup Sub Table Type 5 Format {format}");
            }
            ushort markCoverageOffset = reader.ReadUInt16(); //from beginning of MarkLigPos subtable
            ushort ligatureCoverageOffset = reader.ReadUInt16();
            ushort classCount = reader.ReadUInt16();
            ushort markArrayOffset = reader.ReadUInt16();
            ushort ligatureArrayOffset = reader.ReadUInt16();
            //-----------------------
            var subTable = new LkSubTableType5
            {
                MarkCoverage = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + markCoverageOffset),
                LigatureCoverage = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + ligatureCoverageOffset),
                MarkArrayTable = MarkArrayTable.CreateFrom(reader, subTableStartAt + markArrayOffset)
            };

            reader.BaseStream.Seek(subTableStartAt + ligatureArrayOffset, SeekOrigin.Begin);
            var ligatureArrayTable = new LigatureArrayTable();
            ligatureArrayTable.ReadFrom(reader, classCount);
            subTable.LigatureArrayTable = ligatureArrayTable;

            return subTable;
        }

        /// <summary>
        /// Lookup Type 6: MarkToMark Attachment Positioning Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType6(BinaryReader reader, long subTableStartAt)
        {
            // uint16     PosFormat      Format identifier-format = 1
            // Offset16   Mark1Coverage  Offset to Combining Mark Coverage table-from beginning of MarkMarkPos subtable
            // Offset16   Mark2Coverage  Offset to Base Mark Coverage table-from beginning of MarkMarkPos subtable
            // uint16     ClassCount     Number of Combining Mark classes defined
            // Offset16   Mark1Array     Offset to MarkArray table for Mark1-from beginning of MarkMarkPos subtable
            // Offset16   Mark2Array     Offset to Mark2Array table for Mark2-from beginning of MarkMarkPos subtable

            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            if (format != 1)
            {
                return new UnImplementedLookupSubTable($"GPOS Lookup Sub Table Type 6 Format {format}");
            }
            ushort mark1CoverageOffset = reader.ReadUInt16();
            ushort mark2CoverageOffset = reader.ReadUInt16();
            ushort classCount = reader.ReadUInt16();
            ushort mark1ArrayOffset = reader.ReadUInt16();
            ushort mark2ArrayOffset = reader.ReadUInt16();
            //
            var subTable = new LkSubTableType6
            {
                MarkCoverage1 = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + mark1CoverageOffset),
                MarkCoverage2 = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + mark2CoverageOffset),
                Mark1ArrayTable = MarkArrayTable.CreateFrom(reader, subTableStartAt + mark1ArrayOffset),
                Mark2ArrayTable = Mark2ArrayTable.CreateFrom(reader, subTableStartAt + mark2ArrayOffset, classCount)
            };

            return subTable;
        }

        /// <summary>
        /// Lookup Type 7: Contextual Positioning Subtables
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType7(BinaryReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default:
                    return new UnImplementedLookupSubTable($"GPOS Lookup Sub Table Type 7 Format {format}");

                case 1:
                    {
                        //Context Positioning Subtable: Format 1
                        //ContextPosFormat1 subtable: Simple context positioning
                        //Value 	Type 	            Description
                        //uint16 	PosFormat 	        Format identifier-format = 1
                        //Offset16 	Coverage 	        Offset to Coverage table-from beginning of ContextPos subtable
                        //uint16 	PosRuleSetCount 	Number of PosRuleSet tables
                        //Offset16 	PosRuleSet[PosRuleSetCount]
                        //
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort posRuleSetCount = reader.ReadUInt16();
                        ushort[] posRuleSetOffsets = reader.ReadUInt16Array(posRuleSetCount);

                        var subTable = new LkSubTableType7Fmt1
                        {
                            PosRuleSetTables = GPOS.CreateMultiplePosRuleSetTables(subTableStartAt, posRuleSetOffsets, reader),
                            CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverageOffset)
                        };
                        return subTable;
                    }
                case 2:
                    {
                        //Context Positioning Subtable: Format 2
                        //uint16 	PosFormat 	        Format identifier-format = 2
                        //Offset16 	Coverage 	        Offset to Coverage table-from beginning of ContextPos subtable
                        //Offset16 	ClassDef 	        Offset to ClassDef table-from beginning of ContextPos subtable
                        //uint16 	PosClassSetCnt      Number of PosClassSet tables
                        //Offset16 	PosClassSet[PosClassSetCnt] 	Array of offsets to PosClassSet tables-from beginning of ContextPos subtable-ordered by class-may be NULL

                        ushort coverageOffset = reader.ReadUInt16();
                        ushort classDefOffset = reader.ReadUInt16();
                        ushort posClassSetCount = reader.ReadUInt16();
                        ushort[] posClassSetOffsets = reader.ReadUInt16Array(posClassSetCount);

                        var subTable = new LkSubTableType7Fmt2
                        {
                            CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverageOffset),
                            ClassDef = ClassDefTable.ClassDefTable.CreateFrom(reader, subTableStartAt + classDefOffset)
                        };

                        PosClassSetTable[] posClassSetTables = new PosClassSetTable[posClassSetCount];
                        subTable.PosClassSetTables = posClassSetTables;
                        for (var n = 0; n < posClassSetCount; ++n)
                        {
                            ushort offset = posClassSetOffsets[n];
                            if (offset > 0)
                            {
                                posClassSetTables[n] = PosClassSetTable.CreateFrom(reader, subTableStartAt + offset);
                            }
                        }
                        return subTable;
                    }
                case 3:
                    {
                        //ContextPosFormat3 subtable: Coverage-based context glyph positioning
                        //Value 	Type 	    Description
                        //uint16 	PosFormat 	Format identifier-format = 3
                        //uint16 	GlyphCount 	Number of glyphs in the input sequence
                        //uint16 	PosCount 	Number of PosLookupRecords
                        //Offset16 	Coverage[GlyphCount] 	Array of offsets to Coverage tables-from beginning of ContextPos subtable
                        //struct 	PosLookupRecord[PosCount] Array of positioning lookups-in design order
                        var subTable = new LkSubTableType7Fmt3();
                        ushort glyphCount = reader.ReadUInt16();
                        ushort posCount = reader.ReadUInt16();
                        //read each lookahead record
                        ushort[] coverageOffsets = reader.ReadUInt16Array(glyphCount);
                        subTable.PosLookupRecords = GPOS.CreateMultiplePosLookupRecords(reader, posCount);
                        subTable.CoverageTables = CoverageTable.CoverageTable.CreateMultipleCoverageTables(subTableStartAt, coverageOffsets, reader);

                        return subTable;
                    }
            }
        }

        /// <summary>
        /// LookupType 8: Chaining Contextual Positioning Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType8(BinaryReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default:
                    return new UnImplementedLookupSubTable($"GPOS Lookup Table Type 8 Format {format}");

                case 1:
                    {
                        //Chaining Context Positioning  Format 1: Simple Chaining Context Glyph Positioning
                        //uint16 	PosFormat 	        Format identifier-format = 1
                        //Offset16 	Coverage 	        Offset to Coverage table-from beginning of ContextPos subtable
                        //uint16 	ChainPosRuleSetCount 	Number of ChainPosRuleSet tables
                        //Offset16 	ChainPosRuleSet[ChainPosRuleSetCount] 	Array of offsets to ChainPosRuleSet tables-from beginning of ContextPos subtable-ordered by Coverage Index

                        ushort coverageOffset = reader.ReadUInt16();
                        ushort chainPosRuleSetCount = reader.ReadUInt16();
                        ushort[] chainPosRuleSetOffsetList = reader.ReadUInt16Array(chainPosRuleSetCount);

                        var subTable = new LkSubTableType8Fmt1
                        {
                            PosRuleSetTables = GPOS.CreateMultiplePosRuleSetTables(subTableStartAt, chainPosRuleSetOffsetList, reader),
                            CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverageOffset)
                        };
                        return subTable;
                    }
                case 2:
                    {
                        //Chaining Context Positioning Format 2: Class-based Chaining Context Glyph Positioning
                        //uint16 	PosFormat 	                Format identifier-format = 2
                        //Offset16 	Coverage 	                Offset to Coverage table-from beginning of ChainContextPos subtable
                        //Offset16 	BacktrackClassDef 	        Offset to ClassDef table containing backtrack sequence context-from beginning of ChainContextPos subtable
                        //Offset16 	InputClassDef 	            Offset to ClassDef table containing input sequence context-from beginning of ChainContextPos subtable
                        //Offset16 	LookaheadClassDef                   	Offset to ClassDef table containing lookahead sequence context-from beginning of ChainContextPos subtable
                        //uint16 	ChainPosClassSetCnt 	                Number of ChainPosClassSet tables
                        //Offset16 	ChainPosClassSet[ChainPosClassSetCnt] 	Array of offsets to ChainPosClassSet tables-from beginning of ChainContextPos subtable-ordered by input class-may be NULL

                        ushort coverageOffset = reader.ReadUInt16();
                        ushort backTrackClassDefOffset = reader.ReadUInt16();
                        ushort inputClassDefOffset = reader.ReadUInt16();
                        ushort lookaheadClassDefOffset = reader.ReadUInt16();
                        ushort chainPosClassSetCnt = reader.ReadUInt16();
                        ushort[] chainPosClassSetOffsetArray = reader.ReadUInt16Array(chainPosClassSetCnt);

                        var subTable = new LkSubTableType8Fmt2
                        {
                            BackTrackClassDef = ClassDefTable.ClassDefTable.CreateFrom(reader, subTableStartAt + backTrackClassDefOffset),
                            InputClassDef = ClassDefTable.ClassDefTable.CreateFrom(reader, subTableStartAt + inputClassDefOffset),
                            LookaheadClassDef = ClassDefTable.ClassDefTable.CreateFrom(reader, subTableStartAt + lookaheadClassDefOffset)
                        };

                        //----------
                        PosClassSetTable[] posClassSetTables = new PosClassSetTable[chainPosClassSetCnt];
                        for (var n = 0; n < chainPosClassSetCnt; ++n)
                        {
                            ushort offset = chainPosClassSetOffsetArray[n];
                            if (offset > 0)
                            {
                                posClassSetTables[n] = PosClassSetTable.CreateFrom(reader, subTableStartAt + offset);
                            }
                        }
                        subTable.PosClassSetTables = posClassSetTables;
                        subTable.CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverageOffset);

                        return subTable;
                    }
                case 3:
                    {
                        //Chaining Context Positioning Format 3: Coverage-based Chaining Context Glyph Positioning
                        //uint16 	PosFormat 	                    Format identifier-format = 3
                        //uint16 	BacktrackGlyphCount 	        Number of glyphs in the backtracking sequence
                        //Offset16 	Coverage[BacktrackGlyphCount] 	Array of offsets to coverage tables in backtracking sequence, in glyph sequence order
                        //uint16 	InputGlyphCount 	            Number of glyphs in input sequence
                        //Offset16 	Coverage[InputGlyphCount] 	    Array of offsets to coverage tables in input sequence, in glyph sequence order
                        //uint16 	LookaheadGlyphCount 	        Number of glyphs in lookahead sequence
                        //Offset16 	Coverage[LookaheadGlyphCount] 	Array of offsets to coverage tables in lookahead sequence, in glyph sequence order
                        //uint16 	PosCount 	                    Number of PosLookupRecords
                        //struct 	PosLookupRecord[PosCount] 	    Array of PosLookupRecords,in design order

                        var subTable = new LkSubTableType8Fmt3();

                        ushort backtrackGlyphCount = reader.ReadUInt16();
                        ushort[] backtrackCoverageOffsets = reader.ReadUInt16Array(backtrackGlyphCount);
                        ushort inputGlyphCount = reader.ReadUInt16();
                        ushort[] inputGlyphCoverageOffsets = reader.ReadUInt16Array(inputGlyphCount);
                        ushort lookaheadGlyphCount = reader.ReadUInt16();
                        ushort[] lookaheadCoverageOffsets = reader.ReadUInt16Array(lookaheadGlyphCount);

                        ushort posCount = reader.ReadUInt16();
                        subTable.PosLookupRecords = GPOS.CreateMultiplePosLookupRecords(reader, posCount);

                        subTable.BacktrackCoverages = CoverageTable.CoverageTable.CreateMultipleCoverageTables(subTableStartAt, backtrackCoverageOffsets, reader);
                        subTable.InputGlyphCoverages = CoverageTable.CoverageTable.CreateMultipleCoverageTables(subTableStartAt, inputGlyphCoverageOffsets, reader);
                        subTable.LookaheadCoverages = CoverageTable.CoverageTable.CreateMultipleCoverageTables(subTableStartAt, lookaheadCoverageOffsets, reader);

                        return subTable;
                    }
            }
        }

        /// <summary>
        /// LookupType 9: Extension Positioning
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType9(BinaryReader reader, long subTableStartAt)
        {
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);
            ushort format = reader.ReadUInt16();
            ushort extensionLookupType = reader.ReadUInt16();
            uint extensionOffset = reader.ReadUInt32();
            if (extensionLookupType == 9)
            {
                throw new OpenFontNotSupportedException();
            }
            // Simply read the lookup table again with updated offsets

            return ReadSubTable(extensionLookupType, reader, subTableStartAt + extensionOffset);
        }
    }
}