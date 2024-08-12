namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    public class Lk2Class2Record
    {
        //A Class2Record consists of two ValueRecords,
        //one for the first glyph in a class pair (Value1) and one for the second glyph (Value2).
        //If the PairPos subtable has a value of zero (0) for ValueFormat1 or ValueFormat2,
        //the corresponding record (ValueRecord1 or ValueRecord2) will be empty.

        //Class2Record
        //--------------------------------
        //Value 	    Type 	Description
        //ValueRecord 	Value1 	Positioning for first glyph-empty if ValueFormat1 = 0
        //ValueRecord 	Value2 	Positioning for second glyph-empty if ValueFormat2 = 0
        //--------------------------------
        public readonly ValueRecord value1;//null= empty

        public readonly ValueRecord value2;//null= empty

        public Lk2Class2Record(ValueRecord value1, ValueRecord value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

#if DEBUG

        public override string ToString()
        {
            return "value1:" + (value1) + ",value2:" + value2;
        }

#endif
    }
}