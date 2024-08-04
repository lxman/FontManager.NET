namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables.LookupTable
{
    public class Lk2Class1Record
    {
        // a Class1Record enumerates all pairs that contain a particular class as a first component.
        //The Class1Record array stores all Class1Records according to class value.

        //Note: Class1Records are not tagged with a class value identifier.
        //Instead, the index value of a Class1Record in the array defines the class value represented by the record.
        //For example, the first Class1Record enumerates pairs that begin with a Class 0 glyph,
        //the second Class1Record enumerates pairs that begin with a Class 1 glyph, and so on.

        //Each Class1Record contains an array of Class2Records (Class2Record), which also are ordered by class value.
        //One Class2Record must be declared for each class in the ClassDef2 table, including Class 0.
        //--------------------------------
        //Class1Record
        //Value 	Type 	Description
        //struct 	Class2Record[Class2Count] 	Array of Class2 records-ordered by Class2
        //--------------------------------
        public readonly Lk2Class2Record[] class2Records;

        public Lk2Class1Record(Lk2Class2Record[] class2Records)
        {
            this.class2Records = class2Records;
        }

        //#if DEBUG
        //                public override string ToString()
        //                {
        //                    System.Text.StringBuilder stbuilder = new System.Text.StringBuilder();
        //                    for (int i = 0; i < class2Records.Length; ++i)
        //                    {
        //                        Lk2Class2Record rec = class2Records[i];
        //                        string str = rec.ToString();

        //                        if (str != "value1:,value2:")
        //                        {
        //                            //skip
        //                            stbuilder.Append("i=" + i + "=>" + str + "    ");
        //                        }
        //                    }
        //                    return stbuilder.ToString();
        //                    //return base.ToString();
        //                }
        //#endif
    }
}