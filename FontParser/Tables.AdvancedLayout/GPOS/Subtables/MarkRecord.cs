namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public class MarkRecord
    {
        /// <summary>
        /// Class defined for this mark,. A mark class is identified by a specific integer, called a class value
        /// </summary>
        public readonly ushort markClass;

        /// <summary>
        /// Offset to Anchor table-from beginning of MarkArray table
        /// </summary>
        public readonly ushort offset;

        public MarkRecord(ushort markClass, ushort offset)
        {
            this.markClass = markClass;
            this.offset = offset;
        }

#if DEBUG

        public override string ToString()
        {
            return "class " + markClass + ",offset=" + offset;
        }

#endif
    }
}