using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FontParser.Tables.Cff.Type1;

namespace FontParser.Tables.Cff
{
    public static class DictEntryReader
    {
        public static void Read(
            List<byte> bytes,
            ReadOnlyDictionary<ushort, CffDictEntry?> src,
            List<CffDictEntry> dest)
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
                    CffDictEntry? entry = src[lookup];
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
                    dest.Add(entry);
                    operands.Clear();
                }
            }
        }
    }
}
