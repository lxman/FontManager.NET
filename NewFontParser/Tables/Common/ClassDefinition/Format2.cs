using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ClassDefinition
{
    public class Format2 : IClassDefinition
    {
        public ushort Format { get; }

        public List<ClassRange> ClassRanges { get; } = new List<ClassRange>();

        public Format2(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            ushort classRangeCount = reader.ReadUShort();
            for (var i = 0; i < classRangeCount; i++)
            {
                ClassRanges.Add(new ClassRange(reader));
            }
        }
    }
}