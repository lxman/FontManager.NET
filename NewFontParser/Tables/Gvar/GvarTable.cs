using System;
using System.Collections.Generic;
using NewFontParser.Reader;
using Tuple = NewFontParser.Tables.Common.TupleVariationStore.Tuple;

namespace NewFontParser.Tables.Gvar
{
    public class GvarTable : IFontTable
    {
        public static string Tag => "gvar";

        public Header Header { get; }

        public List<Tuple> Tuples { get; } = new List<Tuple>();

        public List<Common.TupleVariationStore.Header> GlyphVariations { get; } = new List<Common.TupleVariationStore.Header>();

        public GvarTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Header = new Header(reader);
            var glyphVariationDataOffsets = new List<uint>();
            bool readLongOffsets = (Header.Flags & 0x0001) != 0;
            for (var i = 0; i <= Header.GlyphCount; i++)
            {
                glyphVariationDataOffsets.Add(readLongOffsets ? reader.ReadUInt32() : Convert.ToUInt32(reader.ReadUShort() * 2));
            }
            reader.Seek(Header.SharedTuplesOffset);
            for (var i = 0; i < Header.SharedTupleCount; i++)
            {
                Tuples.Add(new Tuple(reader, Header.AxisCount));
            }

            for (var i = 0; i < glyphVariationDataOffsets.Count - 1; i++)
            {
                reader.Seek(Header.GlyphVariationDataArrayOffset + glyphVariationDataOffsets[i]);
                GlyphVariations.Add(new Common.TupleVariationStore.Header(reader, Header.AxisCount, false));
            }
        }
    }
}