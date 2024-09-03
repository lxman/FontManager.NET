using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Gdef;

namespace NewFontParser.Tables.Common.GlyphClassDef
{
    public class ClassDefinition2 : IClassDefinition
    {
        public ushort Format => 2;

        public ushort ClassRangeCount { get; }

        public List<ClassRangeRecord> ClassRangeRecords { get; }

        public ClassDefinition2(BigEndianReader reader)
        {
            _ = reader.ReadBytes(2);
            ClassRangeCount = reader.ReadUShort();
            ClassRangeRecords = new List<ClassRangeRecord>();
            for (var i = 0; i < ClassRangeCount; i++)
            {
                ClassRangeRecords.Add(new ClassRangeRecord(reader.ReadBytes(6)));
            }
        }
    }
}
