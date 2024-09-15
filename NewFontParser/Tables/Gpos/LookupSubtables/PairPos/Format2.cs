using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables.PairPos
{
    public class Format2 : ILookupSubTable
    {
        public ushort PosFormat { get; }

        public ValueFormat ValueFormat1 { get; }

        public ValueFormat ValueFormat2 { get; }

        public List<Class1Record> Class1Records { get; } = new List<Class1Record>();

        public Format2(BigEndianReader reader)
        {
            PosFormat = reader.ReadUShort();
            ushort coverageOffset = reader.ReadUShort();
            ValueFormat1 = (ValueFormat)reader.ReadUShort();
            ValueFormat2 = (ValueFormat)reader.ReadUShort();
            ushort classDef1Offset = reader.ReadUShort();
            ushort classDef2Offset = reader.ReadUShort();
            ushort class1Count = reader.ReadUShort();
            ushort class2Count = reader.ReadUShort();
            for (var i = 0; i < class1Count; i++)
            {
                Class1Records.Add(new Class1Record(class2Count, ValueFormat1, ValueFormat2, reader));
            }
        }
    }
}