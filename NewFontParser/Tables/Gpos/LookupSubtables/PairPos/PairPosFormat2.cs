using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;
using NewFontParser.Tables.Common.GlyphClassDef;

namespace NewFontParser.Tables.Gpos.LookupSubtables.PairPos
{
    public class PairPosFormat2 : ILookupSubTable
    {
        public ushort PosFormat { get; }

        public ICoverageFormat Coverage { get; }

        public ClassDefinition1 ClassDef1 { get; }

        public ClassDefinition2 ClassDef2 { get; }

        public ValueFormat ValueFormat1 { get; }

        public ValueFormat ValueFormat2 { get; }

        public List<Class1Record> Class1Records { get; } = new List<Class1Record>();

        public PairPosFormat2(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
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
            reader.Seek(startOfTable + coverageOffset);
            Coverage = CoverageTable.Retrieve(reader);

            // TODO: Come back and fix this
            //reader.Seek(startOfTable + classDef1Offset);
            //ClassDef1 = new ClassDefinition1(reader);
            //reader.Seek(startOfTable + classDef2Offset);
            //ClassDef2 = new ClassDefinition2(reader);
        }
    }
}