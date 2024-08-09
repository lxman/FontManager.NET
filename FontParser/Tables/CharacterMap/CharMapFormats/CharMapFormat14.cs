using System.Collections.Generic;
using System.IO;

namespace FontParser.Tables.CharacterMap.CharMapFormats
{
    //https://www.microsoft.com/typography/otspec/cmap.htm#format14
    // Subtable format 14 specifies the Unicode Variation Sequences(UVSes) supported by the font.
    // A Variation Sequence, according to the Unicode Standard, comprises a base character followed
    // by a variation selector; e.g. <U+82A6, U+E0101>.
    //
    // The subtable partitions the UVSes supported by the font into two categories: “default” and
    // “non-default” UVSes.Given a UVS, if the glyph obtained by looking up the base character of
    // that sequence in the Unicode cmap subtable(i.e.the UCS-4 or the BMP cmap subtable) is the
    // glyph to use for that sequence, then the sequence is a “default” UVS; otherwise it is a
    // “non-default” UVS, and the glyph to use for that sequence is specified in the format 14
    // subtable itself.
    public class CharMapFormat14 : CharacterMap
    {
        public override ushort Format => 14;

        public override ushort GetGlyphIndex(int character) => 0;

        public ushort CharacterPairToGlyphIndex(int codepoint, ushort defaultGlyphIndex, int nextCodepoint)
        {
            // Only check codepoint if nextCodepoint is a variation selector

            if (_variationSelectors.TryGetValue(nextCodepoint, out VariationSelector sel))
            {
                // If the sequence is a non-default UVS, return the mapped glyph

                if (sel.UVSMappings.TryGetValue(codepoint, out ushort ret))
                {
                    return ret;
                }

                // If the sequence is a default UVS, return the default glyph
                for (var i = 0; i < sel.DefaultStartCodes.Count; ++i)
                {
                    if (codepoint >= sel.DefaultStartCodes[i] && codepoint < sel.DefaultEndCodes[i])
                    {
                        return defaultGlyphIndex;
                    }
                }

                // At this point we are neither a non-default UVS nor a default UVS,
                // but we know the nextCodepoint is a variation selector. Unicode says
                // this glyph should be invisible: “no visible rendering for the VS”
                // (http://unicode.org/faq/unsup_char.html#4)
                return defaultGlyphIndex;
            }

            // In all other cases, return 0
            return 0;
        }

        public override void CollectUnicodeChars(List<uint> unicodes)
        {
            //TODO: review here
#if DEBUG
            System.Diagnostics.Debug.WriteLine("not implemented");
#endif
        }

        public static CharMapFormat14 Create(BinaryReader reader)
        {
            // 'cmap' Subtable Format 14:
            // Type                 Name                                Description
            // uint16               format                              Subtable format.Set to 14.
            // uint32               length                              Byte length of this subtable (including this header)
            // uint32               numVarSelectorRecords               Number of variation Selector Records
            // VariationSelector    varSelector[numVarSelectorRecords]  Array of VariationSelector records.
            // ---
            //
            // Each variation selector records specifies a variation selector character, and
            // offsets to “default” and “non-default” tables used to map variation sequences using
            // that variation selector.
            //
            // VariationSelector Record:
            // Type      Name                 Description
            // uint24    varSelector          Variation selector
            // Offset32  defaultUVSOffset     Offset from the start of the format 14 subtable to
            //                                Default UVS Table.May be 0.
            // Offset32  nonDefaultUVSOffset  Offset from the start of the format 14 subtable to
            //                                Non-Default UVS Table. May be 0.
            //
            // The Variation Selector Records are sorted in increasing order of ‘varSelector’. No
            // two records may have the same ‘varSelector’.
            // A Variation Selector Record and the data its offsets point to specify those UVSes
            // supported by the font for which the variation selector is the ‘varSelector’ value
            // of the record. The base characters of the UVSes are stored in the tables pointed
            // to by the offsets.The UVSes are partitioned by whether they are default or
            // non-default UVSes.
            // Glyph IDs to be used for non-default UVSes are specified in the Non-Default UVS table.

            long beginAt = reader.BaseStream.Position - 2; // account for header format entry
            uint length = reader.ReadUInt32(); // Byte length of this subtable (including the header)
            uint numVarSelectorRecords = reader.ReadUInt32();

            var variationSelectors = new Dictionary<int, VariationSelector>();
            var varSelectors = new int[numVarSelectorRecords];
            var defaultUVSOffsets = new uint[numVarSelectorRecords];
            var nonDefaultUVSOffsets = new uint[numVarSelectorRecords];
            for (var i = 0; i < numVarSelectorRecords; ++i)
            {
                varSelectors[i] = reader.ReadUInt24();
                defaultUVSOffsets[i] = reader.ReadUInt32();
                nonDefaultUVSOffsets[i] = reader.ReadUInt32();
            }

            for (var i = 0; i < numVarSelectorRecords; ++i)
            {
                var sel = new VariationSelector();

                if (defaultUVSOffsets[i] != 0)
                {
                    // Default UVS table
                    //
                    // A Default UVS Table is simply a range-compressed list of Unicode scalar
                    // values, representing the base characters of the default UVSes which use
                    // the ‘varSelector’ of the associated Variation Selector Record.
                    //
                    // DefaultUVS Table:
                    // Type          Name                           Description
                    // uint32        numUnicodeValueRanges          Number of Unicode character ranges.
                    // UnicodeRange  ranges[numUnicodeValueRanges]  Array of UnicodeRange records.
                    //
                    // Each Unicode range record specifies a contiguous range of Unicode values.
                    //
                    // UnicodeRange Record:
                    // Type    Name               Description
                    // uint24  startUnicodeValue  First value in this range
                    // uint8   additionalCount    Number of additional values in this range
                    //
                    // For example, the range U+4E4D&endash; U+4E4F (3 values) will set
                    // ‘startUnicodeValue’ to 0x004E4D and ‘additionalCount’ to 2. A singleton
                    // range will set ‘additionalCount’ to 0.
                    // (‘startUnicodeValue’ + ‘additionalCount’) must not exceed 0xFFFFFF.
                    // The Unicode Value Ranges are sorted in increasing order of
                    // ‘startUnicodeValue’. The ranges must not overlap; i.e.,
                    // (‘startUnicodeValue’ + ‘additionalCount’) must be less than the
                    // ‘startUnicodeValue’ of the following range (if any).

                    reader.BaseStream.Seek(beginAt + defaultUVSOffsets[i], SeekOrigin.Begin);
                    uint numUnicodeValueRanges = reader.ReadUInt32();
                    for (var n = 0; n < numUnicodeValueRanges; ++n)
                    {
                        int startCode = reader.ReadUInt24();
                        sel.DefaultStartCodes.Add(startCode);
                        sel.DefaultEndCodes.Add(startCode + reader.ReadByte());
                    }
                }

                if (nonDefaultUVSOffsets[i] != 0)
                {
                    // Non-Default UVS table
                    //
                    // A Non-Default UVS Table is a list of pairs of Unicode scalar values and
                    // glyph IDs.The Unicode values represent the base characters of all
                    // non -default UVSes which use the ‘varSelector’ of the associated Variation
                    // Selector Record, and the glyph IDs specify the glyph IDs to use for the
                    // UVSes.
                    //
                    // NonDefaultUVS Table:
                    // Type        Name                         Description
                    // uint32      numUVSMappings               Number of UVS Mappings that follow
                    // UVSMapping  uvsMappings[numUVSMappings]  Array of UVSMapping records.
                    //
                    // Each UVSMapping record provides a glyph ID mapping for one base Unicode
                    // character, when that base character is used in a variation sequence with
                    // the current variation selector.
                    //
                    // UVSMapping Record:
                    // Type    Name          Description
                    // uint24  unicodeValue  Base Unicode value of the UVS
                    // uint16  glyphID       Glyph ID of the UVS
                    //
                    // The UVS Mappings are sorted in increasing order of ‘unicodeValue’. No two
                    // mappings in this table may have the same ‘unicodeValue’ values.

                    reader.BaseStream.Seek(beginAt + nonDefaultUVSOffsets[i], SeekOrigin.Begin);
                    uint numUVSMappings = reader.ReadUInt32();
                    for (var n = 0; n < numUVSMappings; ++n)
                    {
                        int unicodeValue = reader.ReadUInt24();
                        ushort glyphID = reader.ReadUInt16();
                        sel.UVSMappings.Add(unicodeValue, glyphID);
                    }
                }

                variationSelectors.Add(varSelectors[i], sel);
            }

            return new CharMapFormat14 { _variationSelectors = variationSelectors };
        }

        private class VariationSelector
        {
            public readonly List<int> DefaultStartCodes = new List<int>();
            public readonly List<int> DefaultEndCodes = new List<int>();
            public readonly Dictionary<int, ushort> UVSMappings = new Dictionary<int, ushort>();
        }

        private Dictionary<int, VariationSelector> _variationSelectors;
    }
}