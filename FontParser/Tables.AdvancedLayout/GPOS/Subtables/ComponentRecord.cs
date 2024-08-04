namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public readonly struct ComponentRecord
    {
        //ComponentRecord
        //Value         Type                          Description
        //Offset16      LigatureAnchor[ClassCount]    Array of offsets(one per class) to Anchor tables-from beginning of LigatureAttach table-ordered by class-NULL if a component does not have an attachment for a class-zero-based array

        public readonly ushort[] offsets;

        public ComponentRecord(ushort[] offsets)
        {
            this.offsets = offsets;
        }
    }
}