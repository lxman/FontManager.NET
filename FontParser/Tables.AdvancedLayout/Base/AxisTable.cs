namespace FontParser.Tables.AdvancedLayout.Base
{
    public class AxisTable
    {
        public bool isVerticalAxis; //false = horizontal , true= vertical axis
        public string[] baseTagList;
        public BaseScript[] baseScripts;
#if DEBUG

        public override string ToString()
        {
            return isVerticalAxis ? "vertical_axis" : "horizontal_axis";
        }

#endif
    }
}