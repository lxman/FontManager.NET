using System.IO;

namespace FontParser.Tables.AdvancedLayout.FontMath
{
    public static class ValueRecordReaderHelper
    {
        public static ValueRecord ReadMathValueRecord(this BinaryReader reader)
        {
            return new ValueRecord(reader.ReadInt16(), reader.ReadUInt16());
        }

        public static ValueRecord[] ReadMathValueRecords(this BinaryReader reader, int count)
        {
            ValueRecord[] records = new ValueRecord[count];
            for (var i = 0; i < count; ++i)
            {
                records[i] = reader.ReadMathValueRecord();
            }
            return records;
        }
    }
}
