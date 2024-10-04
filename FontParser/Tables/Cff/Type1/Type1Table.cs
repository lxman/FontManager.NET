using System;
using System.Collections.Generic;
using System.Linq;
using FontParser.Reader;
using FontParser.Tables.Cff.Type1.Charsets;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.

namespace FontParser.Tables.Cff.Type1
{
    public class Type1Table : IFontTable
    {
        public static string Tag => "CFF ";

        public Header Header { get; }

        public IEncoding Encoding { get; }

        public ICharset CharSet { get; }

        public List<string> Names { get; } = new List<string>();

        public List<string> Strings { get; } = new List<string>();

        public List<string> CharStringList { get; } = new List<string>();

        public List<CffDictEntry> TopDictOperatorEntries { get; } = new List<CffDictEntry>();

        private readonly TopDictOperatorEntries _topDictOperatorEntries = new TopDictOperatorEntries(new Dictionary<ushort, CffDictEntry?>
        {
            { 0x0000, new CffDictEntry("version", OperandKind.Number, 0) },
            { 0x0001, new CffDictEntry("Notice", OperandKind.StringId, 0) },
            { 0x0002, new CffDictEntry("FullName", OperandKind.StringId, 0) },
            { 0x0003, new CffDictEntry("FamilyName", OperandKind.StringId, 0) },
            { 0x0004, new CffDictEntry("Weight", OperandKind.StringId, 0) },
            { 0x0005, new CffDictEntry("FontBBox", OperandKind.Array, new List<int> { 0, 0, 0, 0 }) },
            { 0x000D, new CffDictEntry("UniqueID", OperandKind.Number, 0) },
            { 0x000E, new CffDictEntry("XUID", OperandKind.Array, new object()) },
            { 0x000F, new CffDictEntry("charset", OperandKind.Number, 0) },
            { 0x0010, new CffDictEntry("Encoding", OperandKind.Number, 0) },
            { 0x0011, new CffDictEntry("CharStrings", OperandKind.Number, 0) },
            { 0x0012, new CffDictEntry("Private", OperandKind.NumberNumber, 0) },
            { 0x0C00, new CffDictEntry("Copyright", OperandKind.StringId, 0) },
            { 0x0C01, new CffDictEntry("isFixedPitch", OperandKind.Boolean, false) },
            { 0x0C02, new CffDictEntry("ItalicAngle", OperandKind.Number, 0) },
            { 0x0C03, new CffDictEntry("UnderlinePosition", OperandKind.Number, -100) },
            { 0x0C04, new CffDictEntry("UnderlineThickness", OperandKind.Number, 50) },
            { 0x0C05, new CffDictEntry("PaintType", OperandKind.Number, 0) },
            { 0x0C06, new CffDictEntry("CharstringType", OperandKind.Number, 2) },
            { 0x0C07, new CffDictEntry("FontMatrix", OperandKind.Array, new List<double> { 0.001, 0, 0, 0.001, 0, 0 }) },
            { 0x0C08, new CffDictEntry("StrokeWidth", OperandKind.Number, 0) },
            { 0x0C14, new CffDictEntry("SyntheticBase", OperandKind.Number, 0) },
            { 0x0C15, new CffDictEntry("PostScript", OperandKind.StringId, 0) },
            { 0x0C16, new CffDictEntry("BaseFontName", OperandKind.StringId, 0) },
            { 0x0C17, new CffDictEntry("BaseFontBlend", OperandKind.Delta, new object()) },
            { 0x0C1E, new CffDictEntry("ROS", OperandKind.SidSidNumber, new object()) },
            { 0x0C1F, new CffDictEntry("CIDFontVersion", OperandKind.Number, 0) },
            { 0x0C20, new CffDictEntry("CIDFontRevision", OperandKind.Number, 0) },
            { 0x0C21, new CffDictEntry("CIDFontType", OperandKind.Number, 0) },
            { 0x0C22, new CffDictEntry("CIDCount", OperandKind.Number, 8720) },
            { 0x0C23, new CffDictEntry("UIDBase", OperandKind.Number, 0) },
            { 0x0C24, new CffDictEntry("FDArray", OperandKind.Number, 0) },
            { 0x0C25, new CffDictEntry("FDSelect", OperandKind.Number, 0) },
            { 0x0C26, new CffDictEntry("FontName", OperandKind.StringId, 0) }
        });

        public Type1Table(byte[] data)
        {
            using var reader = new BigEndianReader(data);

            Header = new Header(reader);

            var nameIndex = new Index(reader);

            foreach (List<byte> bytes in nameIndex.Data)
            {
                Names.Add(System.Text.Encoding.ASCII.GetString(bytes.ToArray()));
            }

            var topDictIndex = new Index(reader);

            foreach (List<byte> bytes in topDictIndex.Data)
            {
                var index = 0;
                var operands = new List<double>();
                while (index < bytes.Count)
                {
                    byte b = bytes[index];
                    if (b >= 0x1C)
                    {
                        if (b == 0x1E)
                        {
                            operands.Add(Calc.Double(bytes.ToArray(), ref index));
                        }
                        else
                        {
                            operands.Add(Calc.Integer(bytes.ToArray(), ref index));
                        }
                    }
                    else
                    {
                        byte firstByte = bytes[index++];
                        ushort lookup = firstByte == 0x0C
                            ? Convert.ToUInt16(firstByte << 8 | bytes[index++])
                            : firstByte;
                        CffDictEntry? entry = _topDictOperatorEntries[lookup];
                        if (entry is null) continue;
                        switch (entry.OperandKind)
                        {
                            case OperandKind.StringId:
                                entry.Operand = Convert.ToInt32(operands[0]);
                                break;

                            case OperandKind.Boolean:
                                entry.Operand = Convert.ToInt32(operands[0]) == 1;
                                break;

                            case OperandKind.Number:
                                entry.Operand = Convert.ToInt32(operands[0]);
                                break;

                            case OperandKind.Array:
                                entry.Operand = new List<double>(operands);
                                break;

                            case OperandKind.Delta:
                                if (operands.Count > 1)
                                {
                                    entry.Operand = new List<double>(operands);
                                }
                                else
                                {
                                    entry.Operand = operands[0];
                                }
                                break;

                            case OperandKind.SidSidNumber:
                                break;

                            case OperandKind.NumberNumber:
                                entry.Operand = new List<double>(operands);
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        TopDictOperatorEntries.Add(entry);
                        operands.Clear();
                    }
                }
            }

            var stringIndex = new Index(reader);

            foreach (List<byte> bytes in stringIndex.Data)
            {
                Strings.Add(System.Text.Encoding.ASCII.GetString(bytes.ToArray()));
            }

            var globalSubrIndex = new Index(reader);

            byte encodingFormat = reader.ReadByte();
            Encoding = encodingFormat switch
            {
                0 => new Encoding0(reader),
                1 => new Encoding1(reader),
                _ => Encoding
            };

            reader.Seek(Convert.ToInt64(TopDictOperatorEntries.First(e => e.Name == "CharStrings").Operand));

            var charStrings = new Index(reader);

            reader.Seek(Convert.ToInt64(TopDictOperatorEntries.First(e => e.Name == "charset").Operand));

            byte charsetFormat = reader.ReadByte();
            CharSet = charsetFormat switch
            {
                0 => new CharsetsFormat0(reader,
                    Convert.ToUInt16(charStrings.Data.Count)),
                1 => new CharsetsFormat1(reader,
                    Convert.ToUInt16(charStrings.Data.Count)),
                2 => new CharsetsFormat2(reader,
                    Convert.ToUInt16(charStrings.Data.Count)),
                _ => CharSet
            };
        }
    }
}