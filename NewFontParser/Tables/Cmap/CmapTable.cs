using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using NewFontParser.Reader;
using NewFontParser.Tables.Cmap.SubTables;

namespace NewFontParser.Tables.Cmap
{
    public class CmapTable : IInfoTable
    {
        public static string Tag => "cmap";

        public ushort Version { get; }

        public ushort NumTables { get; }

        internal List<EncodingRecord> EncodingRecords { get; } = new List<EncodingRecord>();

        internal List<ICmapSubtable> SubTables { get; } = new List<ICmapSubtable>();

        public CmapTable(byte[] cmapData)
        {
            var reader = new BigEndianReader(cmapData);
            byte[] data = reader.ReadBytes(2);
            Version = BinaryPrimitives.ReadUInt16BigEndian(data);
            data = reader.ReadBytes(2);
            NumTables = BinaryPrimitives.ReadUInt16BigEndian(data);
            for (var i = 0; i < NumTables; i++)
            {
                EncodingRecords.Add(new EncodingRecord(reader.ReadBytes(EncodingRecord.RecordSize)));
            }
            EncodingRecords = EncodingRecords.OrderBy(x => x.Offset).ToList();
            foreach (EncodingRecord? encodingRecord in EncodingRecords)
            {
                reader.Seek(encodingRecord.Offset);
                data = reader.PeekBytes(2);
                ushort format = BinaryPrimitives.ReadUInt16BigEndian(data);
                switch (format)
                {
                    case 0:
                        SubTables.Add(new Format0(reader));
                        break;

                    case 2:
                        SubTables.Add(new Format2(reader));
                        break;

                    case 4:
                        SubTables.Add(new Format4(reader));
                        break;

                    case 6:
                        SubTables.Add(new Format6(reader));
                        break;

                    case 8:
                        SubTables.Add(new Format8(reader));
                        break;

                    case 10:
                        SubTables.Add(new Format10(reader));
                        break;

                    case 12:
                        SubTables.Add(new Format12(reader));
                        break;

                    case 13:
                        SubTables.Add(new Format13(reader));
                        break;

                    case 14:
                        SubTables.Add(new Format14(reader));
                        break;
                }
            }
        }
    }
}