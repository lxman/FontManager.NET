using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Vdmx
{
    public class VdmxTable : IInfoTable
    {
        public static string Tag => "VDMX";

        public ushort Version { get; }

        public ushort NumRecs { get; }

        public ushort NumRatios { get; }

        public List<RatioRange> RatioRanges { get; } = new List<RatioRange>();

        public ushort[] GroupOffsets;

        public List<VdmxGroup> Groups { get; } = new List<VdmxGroup>();

        public VdmxTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Version = reader.ReadUShort();
            NumRecs = reader.ReadUShort();
            NumRatios = reader.ReadUShort();

            for (var i = 0; i < NumRatios; i++)
            {
                RatioRanges.Add(new RatioRange(reader));
            }

            GroupOffsets = new ushort[NumRecs];
            for (var i = 0; i < NumRecs; i++)
            {
                GroupOffsets[i] = reader.ReadUShort();
            }

            for (var i = 0; i < NumRecs; i++)
            {
                Groups.Add(new VdmxGroup(data[GroupOffsets[i]..]));
            }
        }
    }
}
