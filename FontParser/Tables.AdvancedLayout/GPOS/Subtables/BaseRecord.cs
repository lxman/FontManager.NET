using System.Text;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public readonly struct BaseRecord
    {
        //BaseRecord
        //Value 	Type 	Description
        //Offset16 	BaseAnchor[ClassCount] 	Array of offsets (one per class) to
        //Anchor tables-from beginning of BaseArray table-ordered by class-zero-based

        public readonly AnchorPoint[] anchors;

        public BaseRecord(AnchorPoint[] anchors)
        {
            this.anchors = anchors;
        }

#if DEBUG

        public override string ToString()
        {
            var stbuilder = new StringBuilder();
            if (anchors != null)
            {
                var i = 0;
                foreach (AnchorPoint a in anchors)
                {
                    if (i > 0)
                    {
                        stbuilder.Append(',');
                    }
                    if (a == null)
                    {
                        stbuilder.Append("null");
                    }
                    else
                    {
                        stbuilder.Append(a.ToString());
                    }
                }
            }
            return stbuilder.ToString();
        }

#endif
    }
}