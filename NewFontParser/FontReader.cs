using System.Buffers.Binary;
using System.IO;
using System.Linq;
using System.Text;
using NewFontParser.Models;
using NewFontParser.Reader;

namespace NewFontParser
{
    public class FontReader
    {
        public FontStructure ReadFile(string file)
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
            else
            {
                throw new InvalidDataException("We do not know how to parse this file.");
            }

            data = reader.ReadBytes(2);
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
                x.Data = reader.ReadBytes(x.Length);
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