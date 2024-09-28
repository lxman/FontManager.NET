using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gdef
{
    public class AttachPointTable
    {
        public ushort PointCount { get; }

        public List<ushort> PointIndices { get; } = new List<ushort>();

        public AttachPointTable(byte[] data)
        {
            using var reader = new BigEndianReader(data);

            PointCount = reader.ReadUShort();
            for (var i = 0; i < PointCount; i++)
            {
                PointIndices.Add(reader.ReadUShort());
            }
        }
    }
}