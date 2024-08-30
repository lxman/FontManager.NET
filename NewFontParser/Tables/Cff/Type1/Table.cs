using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cff.Type1
{
    public class Table : IInfoTable
    {
        public Header Header { get; }

        public Index NameIndex { get; }

        public Index TopDictIndex { get; }

        public Index StringIndex { get; }

        public Index GlobalSubrIndex { get; }

        public Index Encodings { get; }

        public Index CharSets { get; }

        public Index CharStrings { get; }

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

        public Table(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Header = new Header(reader);

            NameIndex = new Index(reader);

            foreach (byte[] bytes in NameIndex.Data)
            {
                Names.Add(Encoding.ASCII.GetString(bytes));
            }

            TopDictIndex = new Index(reader);

            foreach (byte[] bytes in TopDictIndex.Data)
            {
                var index = 0;
                var operands = new List<double>();
                while (index < bytes.Length)
                {
                    byte b = bytes[index];
                    if (b >= 0x1C)
                    {
                        if (b == 0x1E)
                        {
                            operands.Add(Calc.Double(bytes, ref index));
                        }
                        else
                        {
                            operands.Add(Calc.Integer(bytes, ref index));
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

            StringIndex = new Index(reader);

            foreach (byte[] bytes in StringIndex.Data)
            {
                Strings.Add(Encoding.ASCII.GetString(bytes));
            }

            GlobalSubrIndex = new Index(reader);

            Encodings = new Index(reader);

            reader.Seek(Convert.ToInt64(TopDictOperatorEntries.First(e => e.Name == "charset").Operand));

            CharSets = new Index(reader);

            reader.Seek(Convert.ToInt64(TopDictOperatorEntries.First(e => e.Name == "CharStrings").Operand));

            CharStrings = new Index(reader);

            foreach (byte[] bytes in CharStrings.Data)
            {
                CharStringList.Add(Encoding.UTF32.GetString(bytes));
            }
        }
    }
}
