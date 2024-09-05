using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Kern
{
    public class KernSubtableFormat0 : IKernSubtable
    {
        public ushort Version { get; }

        public ushort Length { get; }

        public ushort Coverage { get; }

        public List<KernPair> KernPairs { get; } = new List<KernPair>();

        public KernSubtableFormat0(BigEndianReader reader)
        {
            Version = reader.ReadUShort();
            Length = reader.ReadUShort();
            Coverage = reader.ReadUShort();
            ushort nPairs = reader.ReadUShort();
            ushort searchRange = reader.ReadUShort();
            ushort entrySelector = reader.ReadUShort();
            ushort rangeShift = reader.ReadUShort();
            for (var i = 0; i < nPairs; i++)
            {
                KernPairs.Add(new KernPair(reader));
            }
        }
    }
}
