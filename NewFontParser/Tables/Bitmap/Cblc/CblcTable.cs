﻿using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Bitmap.Common;

namespace NewFontParser.Tables.Bitmap.Cblc
{
    public class CblcTable : IInfoTable
    {
        public static string Tag => "CBLC";

        public ushort MajorVersion { get; }

        public ushort MinorVersion { get; }

        public List<BitmapSize> BitmapSizes { get; } = new List<BitmapSize>();

        public CblcTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            MajorVersion = reader.ReadUShort();
            MinorVersion = reader.ReadUShort();
            uint numSizes = reader.ReadUInt32();
            for (var i = 0; i < numSizes; i++)
            {
                BitmapSizes.Add(new BitmapSize(reader));
            }
        }
    }
}