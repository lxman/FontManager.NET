using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.TupleVariationStore
{
    public class TupleVariationHeader
    {
        public ushort VariationDataSize { get; }

        public ushort TupleIndex { get; }

        public Tuple? PeakTuple { get; }

        public Tuple? IntermediateStartTuple { get; }

        public Tuple? IntermediateEndTuple { get; }

        public TupleVariationHeader(BigEndianReader reader, ushort axisCount)
        {
            VariationDataSize = reader.ReadUShort();
            TupleIndex = reader.ReadUShort();
            PeakTuple = new Tuple(reader, axisCount);
            IntermediateStartTuple = new Tuple(reader, axisCount);
            IntermediateEndTuple = new Tuple(reader, axisCount);
        }
    }
}
