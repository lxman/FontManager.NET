using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using FontParser.Reader;
using FontParser.Tables.Cmap.SubTables;

namespace FontParser.Tables.Cmap
{
    public class CmapTable : IFontTable
    {
        public static string Tag => "cmap";

        public ushort Version { get; }

        public List<EncodingRecord> EncodingRecords { get; } = new List<EncodingRecord>();

        public List<ICmapSubtable> SubTables { get; } = new List<ICmapSubtable>();

        public CmapTable(byte[] cmapData)
        {
            var reader = new BigEndianReader(cmapData);
            byte[] data = reader.ReadBytes(2);
            Version = BinaryPrimitives.ReadUInt16BigEndian(data);
            data = reader.ReadBytes(2);
            ushort numTables = BinaryPrimitives.ReadUInt16BigEndian(data);
            for (var i = 0; i < numTables; i++)
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
                        SubTables.Add(new CmapSubtableFormat0(reader));
                        break;

                    case 2:
                        SubTables.Add(new CmapSubtablesFormat2(reader));
                        break;

                    case 4:
                        SubTables.Add(new CmapSubtablesFormat4(reader));
                        break;

                    case 6:
                        SubTables.Add(new CmapSubtablesFormat6(reader));
                        break;

                    case 8:
                        SubTables.Add(new CmapSubtablesFormat8(reader));
                        break;

                    case 10:
                        SubTables.Add(new CmapSubtablesFormat10(reader));
                        break;

                    case 12:
                        SubTables.Add(new CmapSubtablesFormat12(reader));
                        break;

                    case 13:
                        SubTables.Add(new CmapSubtablesFormat13(reader));
                        break;

                    case 14:
                        if (encodingRecord is { PlatformId: 0, EncodingId0: { } } && (int)encodingRecord.EncodingId0.Value == 5)
                        {
                            try
                            {
                                SubTables.Add(new CmapSubtablesFormat14(reader));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Invalid data for encoding record - not read.");
                            }
                        }
                        break;
                }
            }
        }

        public ushort GetGlyphId(ushort codePoint)
        {
            return SubTables
                .Select(subTable => subTable.GetGlyphId(codePoint))
                .FirstOrDefault(glyphId => glyphId != 0);
        }
    }
}