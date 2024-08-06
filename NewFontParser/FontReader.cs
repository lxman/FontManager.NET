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
            ByteReader reader = new ByteReader(file);
            var fontStructure = new FontStructure();
            var signature = reader.ReadBytes(4);

            if (EqualByteArrays(signature, new byte[] { 0x00, 0x01, 0x00, 0x00 }))
            {
                fontStructure.FileType = FileType.TTF;
            }
            else if (EqualByteArrays(signature, new byte[] { 0x4F, 0x54, 0x54, 0x4F }))
            {
                fontStructure.FileType = FileType.OTF;
            }
            else
            {
                throw new InvalidDataException("We do not know how to parse this file.");
            }

            signature = reader.ReadBytes(2);
            fontStructure.TableCount = BinaryPrimitives.ReadUInt16BigEndian(signature);
            signature = reader.ReadBytes(2);
            fontStructure.SearchRange = BinaryPrimitives.ReadUInt16BigEndian(signature);
            signature = reader.ReadBytes(2);
            fontStructure.EntrySelector = BinaryPrimitives.ReadUInt16BigEndian(signature);
            signature = reader.ReadBytes(2);
            fontStructure.RangeShift = BinaryPrimitives.ReadUInt16BigEndian(signature);
            for (var i = 0; i < fontStructure.TableCount; i++)
            {
                fontStructure.TableRecords.Add(ReadTableRecord(reader));
            }
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

        private static TableRecord ReadTableRecord(ByteReader reader)
        {
            var tableRecord = new TableRecord();
            var tag = reader.ReadBytes(4);
            tableRecord.Tag = ToString(tag);
            var checkSum = reader.ReadBytes(4);
            tableRecord.CheckSum = BinaryPrimitives.ReadUInt32BigEndian(checkSum);
            var offset = reader.ReadBytes(4);
            tableRecord.Offset = BinaryPrimitives.ReadUInt32BigEndian(offset);
            var length = reader.ReadBytes(4);
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
