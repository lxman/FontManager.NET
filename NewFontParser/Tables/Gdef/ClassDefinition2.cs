using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Gdef
{
    public class ClassDefinition2 : IClassDefinition
    {
        public long Length { get; }

        public ushort Format { get; } = 2;

        public ushort ClassRangeCount { get; }

        public List<ClassRangeRecord> ClassRangeRecords { get; }

        public ClassDefinition2(byte[] data)
        {
            var reader = new BigEndianReader(data);
            _ = reader.ReadBytes(2);
            ClassRangeCount = reader.ReadUShort();
            ClassRangeRecords = new List<ClassRangeRecord>();
            Length = 4;
            for (var i = 0; i < ClassRangeCount; i++)
            {
                ClassRangeRecords.Add(new ClassRangeRecord(reader.ReadBytes(6)));
                Length += 6;
            }
        }
    }
}
