using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NewFontParser.Models;
using NewFontParser.Reader;

namespace NewFontParser
{
    public class FontReader
    {
        public List<FontStructure> ReadFile(string file)
        {
            var reader = new FileByteReader(file);
            var fontStructure = new FontStructure(file);
            byte[] data = reader.ReadBytes(4);

            if (EqualByteArrays(data, new byte[] { 0x00, 0x01, 0x00, 0x00 }))
            {
                fontStructure.FileType = FileType.Ttf;
            }
            else if (EqualByteArrays(data, new byte[] { 0x4F, 0x54, 0x54, 0x4F }))
            {
                fontStructure.FileType = FileType.Otf;
            }
            else if (EqualByteArrays(data, new byte[] { 0x74, 0x74, 0x63, 0x66 }))
            {
                fontStructure.FileType = FileType.Ttc;
            }
            else
            {
                throw new InvalidDataException("We do not know how to parse this file.");
            }

            switch (fontStructure.FileType)
            {
                case FileType.Unk:
                    Console.WriteLine("This is an unknown file type.");
                    return new List<FontStructure>();

                case FileType.Ttf:
                case FileType.Otf:
                    return new List<FontStructure> { ParseSingle(reader, fontStructure) };

                case FileType.Ttc:
                    return ParseTtc(reader, file);

                case FileType.Otc:
                    Console.WriteLine("I am not aware how to parse otc files yet.");
                    return new List<FontStructure>();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static List<FontStructure> ParseTtc(FileByteReader reader, string file)
        {
            var fontStructures = new List<FontStructure>();
            byte[] data = reader.ReadBytes(2);
            ushort majorVersion = BinaryPrimitives.ReadUInt16BigEndian(data);
            data = reader.ReadBytes(2);
            ushort minorVersion = BinaryPrimitives.ReadUInt16BigEndian(data);
            data = reader.ReadBytes(4);
            uint numFonts = BinaryPrimitives.ReadUInt32BigEndian(data);
            var offsets = new uint[numFonts];
            for (var i = 0; i < numFonts; i++)
            {
                data = reader.ReadBytes(4);
                offsets[i] = BinaryPrimitives.ReadUInt32BigEndian(data);
            }

            if (majorVersion == 2)
            {
                data = reader.ReadBytes(4);
                uint dsigTag = BinaryPrimitives.ReadUInt32BigEndian(data);
                data = reader.ReadBytes(4);
                uint dsigLength = BinaryPrimitives.ReadUInt32BigEndian(data);
                data = reader.ReadBytes(4);
                uint dsigOffset = BinaryPrimitives.ReadUInt32BigEndian(data);
            }
            for (var i = 0; i < numFonts; i++)
            {
                reader.Seek(offsets[i]);
                var fontStructure = new FontStructure(file) { FileType = FileType.Ttc };
                data = reader.ReadBytes(4);
                Console.WriteLine($"\tParsing subfont {i + 1}");
                fontStructures.Add(ParseSingle(reader, fontStructure));
            }
            return fontStructures;
        }

        private static FontStructure ParseSingle(FileByteReader reader, FontStructure fontStructure)
        {
            byte[] data = reader.ReadBytes(2);
            fontStructure.TableCount = BinaryPrimitives.ReadUInt16BigEndian(data);
            data = reader.ReadBytes(2);
            fontStructure.SearchRange = BinaryPrimitives.ReadUInt16BigEndian(data);
            data = reader.ReadBytes(2);
            fontStructure.EntrySelector = BinaryPrimitives.ReadUInt16BigEndian(data);
            data = reader.ReadBytes(2);
            fontStructure.RangeShift = BinaryPrimitives.ReadUInt16BigEndian(data);
            for (var i = 0; i < fontStructure.TableCount; i++)
            {
                fontStructure.TableRecords.Add(ReadTableRecord(reader));
            }
            fontStructure.TableRecords = fontStructure.TableRecords.OrderBy(x => x.Offset).ToList();
            fontStructure.CollectTableNames();
            fontStructure.TableRecords.ForEach(x =>
            {
                reader.Seek(x.Offset);
                List<byte> sectionData = reader.ReadBytes(x.Length).ToList();
                // Pad 4 bytes for badly formed tables
                if (reader.BytesRemaining >= 4) sectionData.AddRange(reader.ReadBytes(4));
                x.Data = sectionData.ToArray();
            });
            fontStructure.Process();
            return fontStructure;
        }

        private static bool EqualByteArrays(byte[]? a, byte[]? b)
        {
            if (a is null || b is null)
            {
                return a is null && b is null;
            }
            return a.Length == b.Length && a.SequenceEqual(b);
        }

        private static TableRecord ReadTableRecord(FileByteReader reader)
        {
            var tableRecord = new TableRecord();
            byte[] tag = reader.ReadBytes(4);
            tableRecord.Tag = ToString(tag);
            byte[] checkSum = reader.ReadBytes(4);
            tableRecord.CheckSum = BinaryPrimitives.ReadUInt32BigEndian(checkSum);
            byte[] offset = reader.ReadBytes(4);
            tableRecord.Offset = BinaryPrimitives.ReadUInt32BigEndian(offset);
            byte[] length = reader.ReadBytes(4);
            tableRecord.Length = BinaryPrimitives.ReadUInt32BigEndian(length);
            return tableRecord;
        }

        private static string ToString(byte[]? bytes)
        {
            return bytes is null
                ? string.Empty
                : Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}