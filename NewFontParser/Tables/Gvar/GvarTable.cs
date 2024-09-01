using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.TupleVariationStore;

namespace NewFontParser.Tables.Gvar
{
    public class GvarTable : IInfoTable
    {
        public static string Tag => "gvar";

        public Header Header { get; }

        public TupleVariationHeader GlyphVariationData { get; }

        public List<TupleVariationHeader> GlyphVariationArray { get; } = new List<TupleVariationHeader>();

        public List<Tuple> SharedTuples { get; } = new List<Tuple>();

        public GvarTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Header = new Header(reader);
            GlyphVariationData = new TupleVariationHeader(reader, Header.AxisCount);
            reader.Seek(Header.SharedTuplesOffset);
            for (var i = 0; i < Header.SharedTupleCount; i++)
            {
                SharedTuples.Add(new Tuple(reader, Header.AxisCount));
            }
            reader.Seek(Header.GlyphVariationDataArrayOffset);
            var glyphVariationDataOffsets = new List<uint>();
            bool readLongOffsets = (Header.Flags & 0x0001) != 0;
            for (var i = 0; i <= Header.GlyphCount; i++)
            {
                glyphVariationDataOffsets.Add(readLongOffsets ? reader.ReadUInt32() : reader.ReadUShort());
            }
            for (var i = 0; i < Header.GlyphCount; i++)
            {
                reader.Seek(glyphVariationDataOffsets[i]);
                GlyphVariationArray.Add(new TupleVariationHeader(reader, Header.AxisCount));
            }
        }
    }
}
