using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Aat.Bloc
{
    public class BlocTable : IInfoTable
    {
        public static string Tag => "bloc";

        public uint Version { get; }

        public List<BitmapSizeTable> BitmapSizeTables { get; } = new List<BitmapSizeTable>();

        public BlocTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Version = reader.ReadUInt32();
            uint numBitmapSizeTables = reader.ReadUInt32();
            for (var i = 0; i < numBitmapSizeTables; i++)
            {
                BitmapSizeTables.Add(new BitmapSizeTable(reader));
            }
        }
    }
}