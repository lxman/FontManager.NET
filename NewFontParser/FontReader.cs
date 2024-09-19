using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NewFontParser.Models;
using NewFontParser.Reader;
using NewFontParser.Tables.Name;

namespace NewFontParser
{
    public class FontReader
    {
        public List<(string, List<IFontTable>)> GetTables(string file)
        {
            List<FontStructure> fontStructure = ReadFile(file);
            var toReturn = new List<(string, List<IFontTable>)>();
            fontStructure.ForEach(fs =>
            {
                var nameTable = (NameTable?)fs.Tables.FirstOrDefault(t => t is NameTable);
                NameRecord? nameRecord =
                    nameTable?.NameRecords.FirstOrDefault(r => r.LanguageId.Contains("English") && r.NameId == "Full Name");
                if (nameRecord is null)
                {
                    return;
                }

                string name = nameRecord.Name ?? string.Empty;
                var toAdd = new List<IFontTable>();
                fs.Tables.ForEach(t =>
                {
                    toAdd.Add(t);
                });
                toReturn.Add((name, toAdd));
            });

            return toReturn;
        }

        public List<(string, List<string>)> GetTableNames(string file)
        {
            var toReturn = new List<(string, List<string>)>();
            List<(string, List<IFontTable>)> tables = GetTables(file);
            tables.ForEach(t =>
            {
                toReturn.Add((t.Item1, t.Item2.Select(i => i.GetType().GetProperty("Tag").GetValue(i).ToString()).ToList()));
            });
            return toReturn;
        }

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
            else if (EqualByteArrays(data, new byte[] { 0x4F, 0x54, 0x43, 0x46 }))
            {
                fontStructure.FileType = FileType.Otc;
            }
            else if (EqualByteArrays(data, new byte[] { 0x77, 0x4F, 0x46, 0x46 }))
            {
                fontStructure.FileType = FileType.Woff;
            }
            else if (EqualByteArrays(data, new byte[] { 0x77, 0x4F, 0x46, 0x32 }))
            {
                fontStructure.FileType = FileType.Woff2;
            }
            else
            {
                throw new InvalidDataException("We do not know how to parse this file.");
            }

            switch (fontStructure.FileType)
            {
                case FileType.Unk:
                    Console.WriteLine("This is an unknown file type.");
                    break;

                case FileType.Ttf:
                case FileType.Otf:
                    return new List<FontStructure> { ParseSingle(reader, fontStructure) };

                case FileType.Ttc:
                    return ParseTtc(reader, file);

                case FileType.Otc:
                    Console.WriteLine("I am not aware how to parse otc files yet.");
                    break;

                case FileType.Woff:
                    return new List<FontStructure> { ParseWoff(reader, file) };
                case FileType.Woff2:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new List<FontStructure>();
        }

        private static List<FontStructure> ParseTtc(FileByteReader reader, string file)
        {
            var fontStructures = new List<FontStructure>();
            ushort majorVersion = reader.ReadUInt16();
            ushort minorVersion = reader.ReadUInt16();
            uint numFonts = reader.ReadUInt32();
            var offsets = new uint[numFonts];
            for (var i = 0; i < numFonts; i++)
            {
                offsets[i] = reader.ReadUInt32();
            }

            if (majorVersion == 2)
            {
                uint dsigTag = reader.ReadUInt32();
                uint dsigLength = reader.ReadUInt32();
                uint dsigOffset = reader.ReadUInt32();
            }
            for (var i = 0; i < numFonts; i++)
            {
                reader.Seek(offsets[i]);
                var fontStructure = new FontStructure(file) { FileType = FileType.Ttc };
                _ = reader.ReadBytes(4);
                Console.WriteLine($"\tParsing subfont {i + 1}");
                fontStructures.Add(ParseSingle(reader, fontStructure));
            }
            return fontStructures;
        }

        private static FontStructure ParseWoff(FileByteReader reader, string file)
        {
            var fontStructure = new FontStructure(file);
            var woffProcessor = new WoffPreprocessor(reader, 1);
            fontStructure.TableRecords = woffProcessor.TableRecords;
            fontStructure.CollectTableNames();
            fontStructure.Process();
            return fontStructure;
        }

        private static List<FontStructure> ParseWoff2(FileByteReader reader)
        {
            return new List<FontStructure>();
        }

        private static FontStructure ParseSingle(FileByteReader reader, FontStructure fontStructure)
        {
            fontStructure.TableCount = reader.ReadUInt16();
            fontStructure.SearchRange = reader.ReadUInt16();
            fontStructure.EntrySelector = reader.ReadUInt16();
            fontStructure.RangeShift = reader.ReadUInt16();
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
            return new TableRecord
            {
                Tag = reader.ReadString(4),
                CheckSum = reader.ReadUInt32(),
                Offset = reader.ReadUInt32(),
                Length = reader.ReadUInt32()
            };
        }
    }
}