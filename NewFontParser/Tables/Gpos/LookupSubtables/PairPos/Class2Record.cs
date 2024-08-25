using NewFontParser.Extensions;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables.PairPos
{
    public class Class2Record
    {
        public ValueRecord ValueRecord1 { get; }

        public ValueRecord ValueRecord2 { get; }

        public Class2Record(ValueFormat vf1, ValueFormat vf2, BigEndianReader reader)
        {
            ValueRecord1 = new ValueRecord(vf1.GetFlags(), reader);
            ValueRecord2 = new ValueRecord(vf2.GetFlags(), reader);
        }
    }
}
