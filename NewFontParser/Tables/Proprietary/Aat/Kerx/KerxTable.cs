﻿using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Proprietary.Aat.Kerx.Subtables;

namespace NewFontParser.Tables.Proprietary.Aat.Kerx
{
    public class KerxTable : IInfoTable
    {
        public static string Tag => "kerx";

        public ushort Version { get; }

        public ushort Padding { get; }

        public List<IKerxSubtable> Subtables { get; } = new List<IKerxSubtable>();

        public KerxTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Version = reader.ReadUShort();
            Padding = reader.ReadUShort();
            uint nTables = reader.ReadUInt32();
            for (var i = 0; i < nTables; i++)
            {
                byte[] formatInfo = reader.PeekBytes(8)[4..8];
                byte format = formatInfo[3];
                switch (format)
                {
                    case 0:
                        Subtables.Add(new Format0(reader));
                        break;
                    case 1:
                        Subtables.Add(new Format1(reader));
                        break;
                    case 2:
                        Subtables.Add(new Format2(reader));
                        break;
                    case 4:
                        Subtables.Add(new Format4(reader));
                        break;
                    case 6:
                        Subtables.Add(new Format6(reader));
                        break;
                }
            }
        }
    }
}
