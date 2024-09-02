using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Bitmap.Ebsc
{
    public class EbscTable : IInfoTable
    {
        public ushort MajorVersion { get; }

        public ushort MinorVersion { get; }

        public List<BitmapScale> Strikes { get; } = new List<BitmapScale>();

        public EbscTable(byte[] bytes)
        {
            var reader = new BigEndianReader(bytes);
            MajorVersion = reader.ReadUShort();
            MinorVersion = reader.ReadUShort();
            ushort numStrikes = reader.ReadUShort();
            for (var i = 0; i < numStrikes; i++)
            {
                Strikes.Add(new BitmapScale(reader));
            }
        }
    }
}
