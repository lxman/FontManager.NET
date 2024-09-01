using System;
using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.TupleVariationStore
{
    public class Header
    {
        public ushort? MajorVersion { get; }

        public ushort? MinorVersion { get; }

        public ushort DataOffset { get; }

        public List<TupleVariationHeader> TupleVariationHeaders { get; } = new List<TupleVariationHeader>();

        public Header(BigEndianReader reader, ushort axisCount, bool isCvar)
        {
            if (isCvar)
            {
                MajorVersion = reader.ReadUShort();
                MinorVersion = reader.ReadUShort();
            }
            ushort tupleVariationCount = reader.ReadUShort();
            var hasSharedPointNumbers = Convert.ToBoolean(tupleVariationCount & 0x8000);
            int actualTupleVariationCount = tupleVariationCount & 0x0FFF;
            DataOffset = reader.ReadUShort();
            for (var i = 0; i < actualTupleVariationCount; i++)
            {
                TupleVariationHeaders.Add(new TupleVariationHeader(reader, axisCount));
            }
        }
    }
}
