using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.TupleVariationStore
{
    public class TupleVariationHeader
    {
        public ushort VariationDataSize { get; }

        public TupleIndexFormat TupleIndex { get; }

        public Tuple? PeakTuple { get; }

        public Tuple? IntermediateStartTuple { get; }

        public Tuple? IntermediateEndTuple { get; }

        public byte[] SerializedDate { get; }

        public TupleVariationHeader(BigEndianReader reader, ushort axisCount, long dataOffset)
        {
            VariationDataSize = reader.ReadUShort();
            ushort tupleIndex = reader.ReadUShort();
            int tupleIndexMask = tupleIndex & 0x0FFF;
            TupleIndex = (TupleIndexFormat)(tupleIndex - tupleIndexMask);
            switch (TupleIndex)
            {
                case TupleIndexFormat.EmbeddedPeakTuple:
                    PeakTuple = new Tuple(reader, axisCount);
                    break;

                case TupleIndexFormat.IntermediateRegion:
                    IntermediateStartTuple = new Tuple(reader, axisCount);
                    IntermediateEndTuple = new Tuple(reader, axisCount);
                    break;
            }
            //reader.Seek(dataOffset);
            //SerializedDate = reader.ReadBytes(VariationDataSize);
        }
    }
}