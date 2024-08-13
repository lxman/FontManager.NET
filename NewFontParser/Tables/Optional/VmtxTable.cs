using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Optional
{
    public class VmtxTable : IInfoTable
    {
        public List<VerticalMetricsEntry> VerticalMetrics { get; } = new List<VerticalMetricsEntry>();

        public short[]? TopSideBearings { get; }

        // numOfLongVerMetrics from vhea table
        public VmtxTable(byte[] data, ushort numOfLongVerMetrics)
        {
            var reader = new BigEndianReader(data);

            for (var i = 0; i < numOfLongVerMetrics; i++)
            {
                VerticalMetrics.Add(new VerticalMetricsEntry(reader.ReadBytes(4)));
            }

            if (reader.WordsRemaining > 0)
            {
                TopSideBearings = new short[reader.WordsRemaining];
                for (var i = 0; i < reader.WordsRemaining; i++)
                {
                    TopSideBearings[i] = reader.ReadShort();
                }
            }
        }
    }
}
