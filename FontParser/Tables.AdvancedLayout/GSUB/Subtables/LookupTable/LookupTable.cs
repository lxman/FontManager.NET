using System;
using System.Collections.Generic;
using System.IO;
using FontParser.Exceptions;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    /// <summary>
    /// sub table of a lookup list
    /// </summary>
    public class LookupTable
    {
#if DEBUG
        public int dbugLkIndex;
#endif
        //--------------------------

        public readonly ushort lookupFlags;
        public readonly ushort markFilteringSet;

        public LookupTable(ushort lookupFlags, ushort markFilteringSet)
        {
            this.lookupFlags = lookupFlags;
            this.markFilteringSet = markFilteringSet;
        }

        //
        public LookupSubTable[] SubTables { get; internal set; }

        //
        public bool DoSubstitutionAt(IGlyphIndexList inputGlyphs, int pos, int len)
        {
            foreach (LookupSubTable subTable in SubTables)
            {
                // We return after the first substitution, as explained in the spec:
                // "A lookup is finished for a glyph after the client locates the target
                // glyph or glyph context and performs a substitution, if specified."
                // https://www.microsoft.com/typography/otspec/gsub.htm
                if (subTable.DoSubstitutionAt(inputGlyphs, pos, len))
                    return true;
            }
            return false;
        }

        public void CollectAssociatedSubstitutionGlyph(List<ushort> outputAssocGlyphs)
        {
            //collect all substitution glyphs
            //
            //if we lookup glyph index from the unicode char
            // (eg. building pre-built glyph texture)
            //we may miss some glyph that is needed for substitution process.
            //
            //so, we collect it here, based on current script lang.
            foreach (LookupSubTable subTable in SubTables)
            {
                subTable.CollectAssociatedSubstitutionGlyphs(outputAssocGlyphs);
            }
        }

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
            }

            return new UnImplementedLookupSubTable($"GSUB Lookup Type {lookupType}");
        }

        /// <summary>
        /// LookupType 1: Single Substitution Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType1(BinaryReader reader, long subTableStartAt)
        {
            //---------------------
            //LookupType 1: Single Substitution Subtable
            //Single substitution (SingleSubst) subtables tell a client to replace a single glyph with another glyph.
            //The subtables can be either of two formats.
            //Both formats require two distinct sets of glyph indices: one that defines input glyphs (specified in the Coverage table),
            //and one that defines the output glyphs. Format 1 requires less space than Format 2, but it is less flexible.
            //------------------------------------
            // 1.1 Single Substitution Format 1
            //------------------------------------
            //Format 1 calculates the indices of the output glyphs,
            //which are not explicitly defined in the subtable.
            //To calculate an output glyph index, Format 1 adds a constant delta value to the input glyph index.
            //For the substitutions to occur properly, the glyph indices in the input and output ranges must be in the same order.
            //This format does not use the Coverage Index that is returned from the Coverage table.

            //The SingleSubstFormat1 subtable begins with a format identifier (SubstFormat) of 1.
            //An offset references a Coverage table that specifies the indices of the input glyphs.
            //DeltaGlyphID is the constant value added to each input glyph index to calculate the index of the corresponding output glyph.

            //Example 2 at the end of this chapter uses Format 1 to replace standard numerals with lining numerals.

            //---------------------------------
            //SingleSubstFormat1 subtable: Calculated output glyph indices
            //---------------------------------
            //Type 	    Name 	        Description
            //uint16 	SubstFormat 	Format identifier-format = 1
            //Offset16 	Coverage 	    Offset to Coverage table-from beginning of Substitution table
            //uint16 	DeltaGlyphID 	Add to original GlyphID to get substitute GlyphID

            //------------------------------------
            //1.2 Single Substitution Format 2
            //------------------------------------
            //Format 2 is more flexible than Format 1, but requires more space.
            //It provides an array of output glyph indices (Substitute) explicitly matched to the input glyph indices specified in the Coverage table.
            //The SingleSubstFormat2 subtable specifies a format identifier (SubstFormat), an offset to a Coverage table that defines the input glyph indices,
            //a count of output glyph indices in the Substitute array (GlyphCount), and a list of the output glyph indices in the Substitute array (Substitute).
            //The Substitute array must contain the same number of glyph indices as the Coverage table. To locate the corresponding output glyph index in the Substitute array, this format uses the Coverage Index returned from the Coverage table.

            //Example 3 at the end of this chapter uses Format 2 to substitute vertically oriented glyphs for horizontally oriented glyphs.
            //---------------------------------
            //SingleSubstFormat2 subtable: Specified output glyph indices
            //---------------------------------
            //Type 	    Name 	        Description
            //USHORT 	SubstFormat 	Format identifier-format = 2
            //Offset 	Coverage 	    Offset to Coverage table-from beginning of Substitution table
            //USHORT 	GlyphCount 	    Number of GlyphIDs in the Substitute array
            //GlyphID 	Substitute[GlyphCount] 	Array of substitute GlyphIDs-ordered by Coverage Index
            //---------------------------------

            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            ushort coverage = reader.ReadUInt16();
            switch (format)
            {
                default: throw new OpenFontNotSupportedException();
                case 1:
                    {
                        ushort deltaGlyph = reader.ReadUInt16();
                        var coverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverage);
                        return new LkSubTableT1Fmt1(coverageTable, deltaGlyph);
                    }
                case 2:
                    {
                        ushort glyphCount = reader.ReadUInt16();
                        ushort[] substituteGlyphs = reader.ReadUInt16Array(glyphCount); // 	Array of substitute GlyphIDs-ordered by Coverage Index
                        var coverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverage);
                        return new LkSubTableT1Fmt2(coverageTable, substituteGlyphs);
                    }
            }
        }

        /// <summary>
        /// LookupType 2: Multiple Substitution Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType2(BinaryReader reader, long subTableStartAt)
        {
            //LookupType 2: Multiple Substitution Subtable
            //A Multiple Substitution (MultipleSubst) subtable replaces a single glyph with more than one glyph,
            //as when multiple glyphs replace a single ligature.

            //The subtable has a single format: MultipleSubstFormat1.

            //The subtable specifies a format identifier (SubstFormat),
            //an offset to a Coverage table that defines the input glyph indices, a count of offsets in the Sequence array (SequenceCount),
            //and an array of offsets to Sequence tables that define the output glyph indices (Sequence).
            //The Sequence table offsets are ordered by the Coverage Index of the input glyphs.

            //For each input glyph listed in the Coverage table, a Sequence table defines the output glyphs.
            //Each Sequence table contains a count of the glyphs in the output glyph sequence (GlyphCount) and an array of output glyph indices (Substitute).

            //    Note: The order of the output glyph indices depends on the writing direction of the text.
            //For text written left to right, the left-most glyph will be first glyph in the sequence.
            //Conversely, for text written right to left, the right-most glyph will be first.

            //The use of multiple substitution for deletion of an input glyph is prohibited. GlyphCount should always be greater than 0.
            //Example 4 at the end of this chapter shows how to replace a single ligature with three glyphs.

            //----------------------
            //MultipleSubstFormat1 subtable: Multiple output glyphs
            //----------------------
            //Type 	    Name 	                Description
            //uint16 	SubstFormat 	        Format identifier-format = 1
            //Offset16 	Coverage    	        Offset to Coverage table-from beginning of Substitution table
            //uint16 	SequenceCount 	        Number of Sequence table offsets in the Sequence array
            //Offset16 	Sequence[SequenceCount] Array of offsets to Sequence tables-from beginning of Substitution table-ordered by Coverage Index
            ////----------------------
            //Sequence table
            //Type 	    Name 	                Description
            //uint16 	GlyphCount 	            Number of glyph IDs  in the Substitute array. This should always be greater than 0.
            //uint16 	Substitute[GlyphCount]  String of glyph IDs  to substitute
            //----------------------
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);
            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default:
                    throw new OpenFontNotSupportedException();
                case 1:
                    {
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort seqCount = reader.ReadUInt16();
                        ushort[] seqOffsets = reader.ReadUInt16Array(seqCount);

                        var subTable = new LkSubTableT2
                        {
                            SeqTables = new SequenceTable[seqCount]
                        };
                        for (var n = 0; n < seqCount; ++n)
                        {
                            reader.BaseStream.Seek(subTableStartAt + seqOffsets[n], SeekOrigin.Begin);
                            ushort glyphCount = reader.ReadUInt16();
                            subTable.SeqTables[n] = new SequenceTable(
                                reader.ReadUInt16Array(glyphCount));
                        }
                        subTable.CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverageOffset);

                        return subTable;
                    }
            }
        }

        /// <summary>
        /// LookupType 3: Alternate Substitution Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType3(BinaryReader reader, long subTableStartAt)
        {
            //LookupType 3: Alternate Substitution Subtable

            //An Alternate Substitution (AlternateSubst)subtable identifies any number of aesthetic alternatives
            //from which a user can choose a glyph variant to replace the input glyph.

            //For example, if a font contains four variants of the ampersand symbol,
            //the cmap table will specify the index of one of the four glyphs as the default glyph index,
            //and an AlternateSubst subtable will list the indices of the other three glyphs as alternatives.
            //A text - processing client would then have the option of replacing the default glyph with any of the three alternatives.

            //The subtable has one format: AlternateSubstFormat1.
            //The subtable contains a format identifier (SubstFormat),
            //    an offset to a Coverage table containing the indices of glyphs with alternative forms(Coverage),
            //    a count of offsets to AlternateSet tables(AlternateSetCount),
            //    and an array of offsets to AlternateSet tables(AlternateSet).

            //For each glyph, an AlternateSet subtable contains a count of the alternative glyphs(GlyphCount) and
            //   an array of their glyph indices(Alternate).
            //Because all the glyphs are functionally equivalent, they can be in any order in the array.

            //Example 5 at the end of this chapter shows how to replace the default ampersand glyph with alternative glyphs.

            //-----------------------
            //AlternateSubstFormat1 subtable: Alternative output glyphs
            //-----------------------
            //Type          Name                Description
            //uint16        SubstFormat         Format identifier - format = 1
            //Offset16      Coverage            Offset to Coverage table - from beginning of Substitution table
            //uint16        AlternateSetCount   Number of AlternateSet tables
            //Offset16      AlternateSet[AlternateSetCount] Array of offsets to AlternateSet tables - from beginning of Substitution table - ordered by Coverage Index
            //
            //AlternateSet table
            //Type    Name    Description
            //uint16  GlyphCount  Number of glyph IDs in the Alternate array
            //uint16  Alternate[GlyphCount]   Array of alternate glyph IDs -in arbitrary order
            //-----------------------

            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16(); //The subtable has one format: AlternateSubstFormat1.
            switch (format)
            {
                default: throw new OpenFontNotSupportedException();
                case 1:
                    {
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort alternativeSetCount = reader.ReadUInt16();
                        ushort[] alternativeTableOffsets = reader.ReadUInt16Array(alternativeSetCount);

                        var subTable = new LkSubTableT3();
                        AlternativeSetTable[] alternativeSetTables = new AlternativeSetTable[alternativeSetCount];
                        subTable.AlternativeSetTables = alternativeSetTables;
                        for (var n = 0; n < alternativeSetCount; ++n)
                        {
                            alternativeSetTables[n] = AlternativeSetTable.CreateFrom(reader, subTableStartAt + alternativeTableOffsets[n]);
                        }
                        subTable.CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverageOffset);

                        return subTable;
                    }
            }
        }

        /// <summary>
        /// LookupType 4: Ligature Substitution Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType4(BinaryReader reader, long subTableStartAt)
        {
            //LookupType 4: Ligature Substitution Subtable

            //A Ligature Substitution (LigatureSubst) subtable identifies ligature substitutions where a single glyph
            //replaces multiple glyphs. One LigatureSubst subtable can specify any number of ligature substitutions.

            //The subtable uses a single format: LigatureSubstFormat1.
            //It contains a format identifier (SubstFormat),
            //a Coverage table offset (Coverage), a count of the ligature sets defined in this table (LigSetCount),
            //and an array of offsets to LigatureSet tables (LigatureSet).
            //The Coverage table specifies only the index of the first glyph component of each ligature set.

            //-----------------------------
            //LigatureSubstFormat1 subtable: All ligature substitutions in a script
            //-----------------------------
            //Type 	    Name 	        Description
            //uint16 	SubstFormat 	Format identifier-format = 1
            //Offset16 	Coverage 	    Offset to Coverage table-from beginning of Substitution table
            //uint16 	LigSetCount 	Number of LigatureSet tables
            //Offset16 	LigatureSet[LigSetCount] 	Array of offsets to LigatureSet tables-from beginning of Substitution table-ordered by Coverage Index
            //-----------------------------

            //A LigatureSet table, one for each covered glyph,
            //specifies all the ligature strings that begin with the covered glyph.
            //For example, if the Coverage table lists the glyph index for a lowercase “f,”
            //then a LigatureSet table will define the “ffl,” “fl,” “ffi,” “fi,” and “ff” ligatures.
            //If the Coverage table also lists the glyph index for a lowercase “e,”
            //then a different LigatureSet table will define the “etc” ligature.

            //A LigatureSet table consists of a count of the ligatures that begin with
            //the covered glyph (LigatureCount) and an array of offsets to Ligature tables,
            //which define the glyphs in each ligature (Ligature).
            //The order in the Ligature offset array defines the preference for using the ligatures.
            //For example, if the “ffl” ligature is preferable to the “ff” ligature, then the Ligature array would list the offset to the “ffl” Ligature table before the offset to the “ff” Ligature table.
            //-----------------------------
            //LigatureSet table: All ligatures beginning with the same glyph
            //-----------------------------
            //Type  	Name 	                Description
            //uint16 	LigatureCount 	        Number of Ligature tables
            //Offset16 	Ligature[LigatureCount] Array of offsets to Ligature tables-from beginning of LigatureSet table-ordered by preference
            //-----------------------------

            //For each ligature in the set, a Ligature table specifies the GlyphID of the output ligature glyph (LigGlyph);
            // count of the total number of component glyphs in the ligature, including the first component (CompCount);
            //and an array of GlyphIDs for the components (Component).
            //The array starts with the second component glyph (array index = 1) in the ligature
            //because the first component glyph is specified in the Coverage table.

            //    Note: The Component array lists GlyphIDs according to the writing direction of the text.
            //For text written right to left, the right-most glyph will be first.
            //Conversely, for text written left to right, the left-most glyph will be first.

            //Example 6 at the end of this chapter shows how to replace a string of glyphs with a single ligature.
            //-----------------------------
            //Ligature table: Glyph components for one ligature
            //-----------------------------
            //Type 	    Name 	    Description
            //uint16 	LigGlyph 	GlyphID of ligature to substitute
            //uint16 	CompCount 	Number of components in the ligature
            //uint16 	Component[CompCount - 1] 	Array of component GlyphIDs-start with the second component-ordered in writing direction

            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default: throw new OpenFontNotSupportedException();
                case 1:
                    {
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort ligSetCount = reader.ReadUInt16();
                        ushort[] ligSetOffsets = reader.ReadUInt16Array(ligSetCount);
                        var subTable = new LkSubTableT4();
                        LigatureSetTable[] ligSetTables = subTable.LigatureSetTables = new LigatureSetTable[ligSetCount];
                        for (var n = 0; n < ligSetCount; ++n)
                        {
                            ligSetTables[n] = LigatureSetTable.CreateFrom(reader, subTableStartAt + ligSetOffsets[n]);
                        }
                        subTable.CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverageOffset);
                        return subTable;
                    }
            }
        }

        /// <summary>
        /// LookupType 5: Contextual Substitution Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType5(BinaryReader reader, long subTableStartAt)
        {
            //LookupType 5: Contextual Substitution Subtable
            //A Contextual Substitution (ContextSubst) subtable defines a powerful type of glyph substitution lookup:
            //it describes glyph substitutions in context that replace one or more glyphs within a certain pattern of glyphs.

            //ContextSubst subtables can be any of three formats that define a context in terms of
            //a specific sequence of glyphs,
            //glyph classes,
            //or glyph sets.

            //Each format can describe one or more input glyph sequences and one or more substitutions for each sequence.
            //All three formats specify substitution data in a SubstLookupRecord, described above.
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort substFormat = reader.ReadUInt16();
            switch (substFormat)
            {
                default: throw new OpenFontNotSupportedException();
                case 1:
                    {
                        //ContextSubstFormat1 Subtable
                        //Table 14
                        //Type  	Name 	            Description
                        //uint16 	substFormat 	    Format identifier: format = 1
                        //Offset16 	coverageOffset 	    Offset to Coverage table, from beginning of substitution subtable
                        //uint16 	subRuleSetCount 	Number of SubRuleSet tables — must equal glyphCount in Coverage table***
                        //Offset16 	subRuleSetOffsets[subRuleSetCount] 	Array of offsets to SubRuleSet tables.
                        //                              Offsets are from beginning of substitution subtable, ordered by Coverage index

                        var fmt1 = new LkSubTableT5Fmt1();
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort subRuleSetCount = reader.ReadUInt16();
                        ushort[] subRuleSetOffsets = reader.ReadUInt16Array(subRuleSetCount);

                        fmt1.coverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverageOffset);
                        fmt1.subRuleSets = new LkSubT5Fmt1_SubRuleSet[subRuleSetCount];

                        for (var i = 0; i < subRuleSetCount; ++i)
                        {
                            fmt1.subRuleSets[i] = LkSubT5Fmt1_SubRuleSet.CreateFrom(reader, subTableStartAt + subRuleSetOffsets[i]);
                        }

                        return fmt1;
                    }
                case 2:
                    {
                        //ContextSubstFormat2 Subtable
                        //Table 17
                        //Type  	Name 	            Description
                        //uint16 	substFormat 	    Format identifier: format = 2
                        //Offset16 	coverageOffset      Offset to Coverage table, from beginning of substitution subtable
                        //Offset16 	classDefOffset 	    Offset to glyph ClassDef table, from beginning of substitution subtable
                        //uint16 	subClassSetCount 	Number of SubClassSet tables
                        //Offset16 	subClassSetOffsets[subClassSetCount] 	Array of offsets to SubClassSet tables. Offsets are from beginning of substitution subtable, ordered by class (may be NULL).

                        var fmt2 = new LkSubTableT5Fmt2();
                        ushort coverageOffset = reader.ReadUInt16();
                        ushort classDefOffset = reader.ReadUInt16();
                        ushort subClassSetCount = reader.ReadUInt16();
                        ushort[] subClassSetOffsets = reader.ReadUInt16Array(subClassSetCount);
                        //
                        fmt2.coverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverageOffset);
                        fmt2.classDef = ClassDefTable.ClassDefTable.CreateFrom(reader, subTableStartAt + classDefOffset);

                        var subClassSets = new LkSubT5Fmt2_SubClassSet[subClassSetCount];
                        fmt2.subClassSets = subClassSets;
                        for (var i = 0; i < subClassSetCount; ++i)
                        {
                            subClassSets[i] = LkSubT5Fmt2_SubClassSet.CreateFrom(reader, subTableStartAt + subClassSetOffsets[i]);
                        }

                        return fmt2;
                    }

                case 3:
                    {
                        return new UnImplementedLookupSubTable("GSUB,Lookup Subtable Type 5,Fmt3");
                    }
            }
        }

        /// <summary>
        /// LookupType 6: Chaining Contextual Substitution Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType6(BinaryReader reader, long subTableStartAt)
        {
            //LookupType 6: Chaining Contextual Substitution Subtable
            //A Chaining Contextual Substitution subtable (ChainContextSubst) describes glyph substitutions in context with an ability to look back and/or look ahead
            //in the sequence of glyphs.
            //The design of the Chaining Contextual Substitution subtable is parallel to that of the Contextual Substitution subtable,
            //including the availability of three formats for handling sequences of glyphs, glyph classes, or glyph sets. Each format can describe one or more backtrack,
            //input, and lookahead sequences and one or more substitutions for each sequence.
            //-----------------------
            //TODO: impl here

            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);

            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default: throw new OpenFontNotSupportedException();
                case 1:
                    {
                        //6.1 Chaining Context Substitution Format 1: Simple Chaining Context Glyph Substitution
                        //-------------------------------
                        //ChainContextSubstFormat1 subtable: Simple context glyph substitution
                        //-------------------------------
                        //Type  	Name 	                Description
                        //uint16 	SubstFormat 	        Format identifier-format = 1
                        //Offset16 	Coverage 	            Offset to Coverage table-from beginning of Substitution table
                        //uint16 	ChainSubRuleSetCount 	Number of ChainSubRuleSet tables-must equal GlyphCount in Coverage table
                        //Offset16 	ChainSubRuleSet[ChainSubRuleSetCount] 	Array of offsets to ChainSubRuleSet tables-from beginning of Substitution table-ordered by Coverage Index
                        //-------------------------------

                        var subTable = new LkSubTableT6Fmt1();
                        ushort coverage = reader.ReadUInt16();
                        ushort chainSubRulesetCount = reader.ReadUInt16();
                        ushort[] chainSubRulesetOffsets = reader.ReadUInt16Array(chainSubRulesetCount);
                        ChainSubRuleSetTable[] subRuleSets = subTable.SubRuleSets = new ChainSubRuleSetTable[chainSubRulesetCount];
                        for (var n = 0; n < chainSubRulesetCount; ++n)
                        {
                            subRuleSets[n] = ChainSubRuleSetTable.CreateFrom(reader, subTableStartAt + chainSubRulesetOffsets[n]);
                        }
                        //----------------------------
                        subTable.CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverage);
                        return subTable;
                    }
                case 2:
                    {
                        //-------------------
                        //ChainContextSubstFormat2 subtable: Class-based chaining context glyph substitution
                        //-------------------
                        //Type 	    Name 	            Description
                        //uint16 	SubstFormat 	    Format identifier-format = 2
                        //Offset16 	Coverage 	        Offset to Coverage table-from beginning of Substitution table
                        //Offset16 	BacktrackClassDef 	Offset to glyph ClassDef table containing backtrack sequence data-from beginning of Substitution table
                        //Offset16 	InputClassDef 	    Offset to glyph ClassDef table containing input sequence data-from beginning of Substitution table
                        //Offset16 	LookaheadClassDef 	Offset to glyph ClassDef table containing lookahead sequence data-from beginning of Substitution table
                        //uint16 	ChainSubClassSetCnt 	Number of ChainSubClassSet tables
                        //Offset16 	ChainSubClassSet[ChainSubClassSetCnt] 	Array of offsets to ChainSubClassSet tables-from beginning of Substitution table-ordered by input class-may be NULL
                        //-------------------
                        var subTable = new LkSubTableT6Fmt2();
                        ushort coverage = reader.ReadUInt16();
                        ushort backtrackClassDefOffset = reader.ReadUInt16();
                        ushort inputClassDefOffset = reader.ReadUInt16();
                        ushort lookaheadClassDefOffset = reader.ReadUInt16();
                        ushort chainSubClassSetCount = reader.ReadUInt16();
                        ushort[] chainSubClassSetOffsets = reader.ReadUInt16Array(chainSubClassSetCount);
                        //
                        subTable.BacktrackClassDef = ClassDefTable.ClassDefTable.CreateFrom(reader, subTableStartAt + backtrackClassDefOffset);
                        subTable.InputClassDef = ClassDefTable.ClassDefTable.CreateFrom(reader, subTableStartAt + inputClassDefOffset);
                        subTable.LookaheadClassDef = ClassDefTable.ClassDefTable.CreateFrom(reader, subTableStartAt + lookaheadClassDefOffset);
                        if (chainSubClassSetCount != 0)
                        {
                            ChainSubClassSet[] chainSubClassSets = subTable.ChainSubClassSets = new ChainSubClassSet[chainSubClassSetCount];
                            for (var n = 0; n < chainSubClassSetCount; ++n)
                            {
                                ushort offset = chainSubClassSetOffsets[n];
                                if (offset > 0)
                                {
                                    chainSubClassSets[n] = ChainSubClassSet.CreateFrom(reader, subTableStartAt + offset);
                                }
                            }
                        }

                        subTable.CoverageTable = CoverageTable.CoverageTable.CreateFrom(reader, subTableStartAt + coverage);
                        return subTable;
                    }
                case 3:
                    {
                        //-------------------
                        //6.3 Chaining Context Substitution Format 3: Coverage-based Chaining Context Glyph Substitution
                        //-------------------
                        //uint16    substFormat                     Format identifier: format = 3
                        //uint16 	backtrackGlyphCount 	        Number of glyphs in the backtracking sequence
                        //Offset16 	backtrackCoverageOffsets[backtrackGlyphCount] 	Array of offsets to coverage tables in backtracking sequence, in glyph sequence order
                        //uint16 	inputGlyphCount 	            Number of glyphs in input sequence
                        //Offset16 	inputCoverageOffsets[InputGlyphCount] 	    Array of offsets to coverage tables in input sequence, in glyph sequence order
                        //uint16 	lookaheadGlyphCount 	        Number of glyphs in lookahead sequence
                        //Offset16 	lookaheadCoverageOffsets[LookaheadGlyphCount] 	Array of offsets to coverage tables in lookahead sequence, in glyph sequence order
                        //uint16 	substitutionCount 	                    Number of SubstLookupRecords
                        //struct 	substLookupRecords[SubstCount] 	Array of SubstLookupRecords, in design order
                        //-------------------
                        var subTable = new LkSubTableT6Fmt3();
                        ushort backtrackingGlyphCount = reader.ReadUInt16();
                        ushort[] backtrackingCoverageOffsets = reader.ReadUInt16Array(backtrackingGlyphCount);
                        ushort inputGlyphCount = reader.ReadUInt16();
                        ushort[] inputGlyphCoverageOffsets = reader.ReadUInt16Array(inputGlyphCount);
                        ushort lookAheadGlyphCount = reader.ReadUInt16();
                        ushort[] lookAheadCoverageOffsets = reader.ReadUInt16Array(lookAheadGlyphCount);
                        ushort substCount = reader.ReadUInt16();
                        subTable.SubstLookupRecords = SubstLookupRecord.CreateSubstLookupRecords(reader, substCount);

                        subTable.BacktrackingCoverages = CoverageTable.CoverageTable.CreateMultipleCoverageTables(subTableStartAt, backtrackingCoverageOffsets, reader);
                        subTable.InputCoverages = CoverageTable.CoverageTable.CreateMultipleCoverageTables(subTableStartAt, inputGlyphCoverageOffsets, reader);
                        subTable.LookaheadCoverages = CoverageTable.CoverageTable.CreateMultipleCoverageTables(subTableStartAt, lookAheadCoverageOffsets, reader);

                        return subTable;
                    }
            }
        }

        /// <summary>
        /// LookupType 7: Extension Substitution
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType7(BinaryReader reader, long subTableStartAt)
        {
            //LookupType 7: Extension Substitution
            //https://www.microsoft.com/typography/otspec/gsub.htm#ES

            //This lookup provides a mechanism whereby any other lookup type's subtables are stored at a 32-bit offset location in the 'GSUB' table.
            //This is needed if the total size of the subtables exceeds the 16-bit limits of the various other offsets in the 'GSUB' table.
            //In this specification, the subtable stored at the 32-bit offset location is termed the “extension” subtable.
            //----------------------------
            //ExtensionSubstFormat1 subtable
            //----------------------------
            //Type          Name                Description
            //uint16        SubstFormat         Format identifier.Set to 1.
            //uint16        ExtensionLookupType Lookup type of subtable referenced by ExtensionOffset (i.e.the extension subtable).
            //Offset32      ExtensionOffset     Offset to the extension subtable, of lookup type ExtensionLookupType, relative to the start of the ExtensionSubstFormat1 subtable.
            //----------------------------
            //ExtensionLookupType must be set to any lookup type other than 7.
            //All subtables in a LookupType 7 lookup must have the same ExtensionLookupType.
            //All offsets in the extension subtables are set in the usual way,
            //i.e.relative to the extension subtables themselves.

            //When an OpenType layout engine encounters a LookupType 7 Lookup table, it shall:

            //Proceed as though the Lookup table's LookupType field were set to the ExtensionLookupType of the subtables.
            //Proceed as though each extension subtable referenced by ExtensionOffset replaced the LookupType 7 subtable that referenced it.

            //Substitution Lookup Record

            //All contextual substitution subtables specify the substitution data in a Substitution Lookup Record (SubstLookupRecord).
            //Each record contains a SequenceIndex,
            //which indicates the position where the substitution will occur in the glyph sequence.
            //In addition, a LookupListIndex identifies the lookup to be applied at the glyph position specified by the SequenceIndex.

            //The contextual substitution subtables defined in Examples 7, 8, and 9 at the end of this chapter show SubstLookupRecords.
            reader.BaseStream.Seek(subTableStartAt, SeekOrigin.Begin);
            ushort format = reader.ReadUInt16();
            ushort extensionLookupType = reader.ReadUInt16();
            uint extensionOffset = reader.ReadUInt32();
            if (extensionLookupType == 7)
            {
                throw new OpenFontNotSupportedException();
            }
            // Simply read the lookup table again with updated offsets
            return ReadSubTable(extensionLookupType, reader, subTableStartAt + extensionOffset);
        }

        /// <summary>
        /// LookupType 8: Reverse Chaining Contextual Single Substitution Subtable
        /// </summary>
        /// <param name="reader"></param>
        private static LookupSubTable ReadLookupType8(BinaryReader reader, long subTableStartAt)
        {
            throw new NotImplementedException();
        }
    }
}