using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.TtTables
{
    public class GaspTable : IInfoTable
    {
        public static string Tag => "gasp";

        public ushort Version { get; set; }

        public ushort NumRanges { get; set; }

        public List<GaspRange> GaspRanges { get; set; } = new List<GaspRange>();

        public GaspTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Version = reader.ReadUShort();
            NumRanges = reader.ReadUShort();

            for (var i = 0; i < NumRanges; i++)
            {
                var range = new GaspRange
                {
                    RangeMaxPPEM = reader.ReadUShort(),
                    RangeGaspBehavior = (RangeGaspBehavior)reader.ReadUShort()
                };

                GaspRanges.Add(range);
            }
        }
    }
}
