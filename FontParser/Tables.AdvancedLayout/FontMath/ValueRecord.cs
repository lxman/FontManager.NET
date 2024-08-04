namespace FontParser.Tables.AdvancedLayout.FontMath
{
    public class ValueRecord
    {
        //ValueRecord
        //Type      Name            Description
        //int16     Value           The X or Y value in design units
        //Offset16  DeviceTable     Offset to the device table – from the beginning of parent table.May be NULL. Suggested format for device table is 1.
        public readonly short Value;

        public readonly ushort DeviceTable;

        public ValueRecord(short value, ushort deviceTable)
        {
            Value = value;
            DeviceTable = deviceTable;
        }

#if DEBUG

        public override string ToString()
        {
            if (DeviceTable == 0)
            {
                return Value.ToString();
            }
            else
            {
                return Value + "," + DeviceTable;
            }
        }

#endif
    }
}
