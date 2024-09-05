using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables.PairPos
{
    public class Format2 : ILookupSubTable
    {
        public ushort PosFormat { get; }

        public ushort CoverageOffset { get; }

        public ValueFormat ValueFormat1 { get; }

        public ValueFormat ValueFormat2 { get; }

        public ushort ClassDef1Offset { get; }

        public ushort ClassDef2Offset { get; }

        public ushort Class1Count { get; }

        public ushort Class2Count { get; }

        public List<Class1Record> Class1Records { get; } = new List<Class1Record>();

        public Format2(BigEndianReader reader)
        {
            PosFormat = reader.ReadUShort();
            CoverageOffset = reader.ReadUShort();
            ValueFormat1 = (ValueFormat)reader.ReadUShort();
            ValueFormat2 = (ValueFormat)reader.ReadUShort();
            ClassDef1Offset = reader.ReadUShort();
            ClassDef2Offset = reader.ReadUShort();
            Class1Count = reader.ReadUShort();
            Class2Count = reader.ReadUShort();
            for (var i = 0; i < Class1Count; i++)
            {
                Class1Records.Add(new Class1Record(Class2Count, ValueFormat1, ValueFormat2, reader));
            }
        }
    }
}