using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.TupleVariationStore;

namespace NewFontParser.Tables.Gvar
{
    public class GvarTable : IInfoTable
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
                glyphVariationDataOffsets.Add(readLongOffsets ? reader.ReadUInt32() : reader.ReadUShort());
            }
            reader.Seek(Header.SharedTuplesOffset);
            for (var i = 0; i < Header.SharedTupleCount; i++)
            {
                Tuples.Add(new Tuple(reader, Header.AxisCount));
            }

            foreach (uint offset in glyphVariationDataOffsets)
            {
                reader.Seek(Header.GlyphVariationDataArrayOffset + offset);
                GlyphVariations.Add(new Common.TupleVariationStore.Header(reader, Header.AxisCount, false));
            }
        }
    }
}