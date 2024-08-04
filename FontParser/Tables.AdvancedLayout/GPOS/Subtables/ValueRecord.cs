using System.IO;
using System.Text;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public class ValueRecord
    {
        //ValueRecord (all fields are optional)
        //Value 	Type 	    Description
        //--------------------------------
        //int16 	XPlacement 	Horizontal adjustment for placement-in design units
        //int16 	YPlacement 	Vertical adjustment for placement, in design units
        //int16 	XAdvance 	Horizontal adjustment for advance, in design units (only used for horizontal writing)
        //int16 	YAdvance 	Vertical adjustment for advance, in design units (only used for vertical writing)
        //Offset16 	XPlaDevice 	Offset to Device table (non-variable font) / VariationIndex table (variable font) for horizontal placement, from beginning of PosTable (may be NULL)
        //Offset16 	YPlaDevice 	Offset to Device table (non-variable font) / VariationIndex table (variable font) for vertical placement, from beginning of PosTable (may be NULL)
        //Offset16 	XAdvDevice 	Offset to Device table (non-variable font) / VariationIndex table (variable font) for horizontal advance, from beginning of PosTable (may be NULL)
        //Offset16 	YAdvDevice 	Offset to Device table (non-variable font) / VariationIndex table (variable font) for vertical advance, from beginning of PosTable (may be NULL)

        public short XPlacement;
        public short YPlacement;
        public short XAdvance;
        public short YAdvance;
        public ushort XPlaDevice;
        public ushort YPlaDevice;
        public ushort XAdvDevice;
        public ushort YAdvDevice;

        private ushort valueFormat;

        public void ReadFrom(BinaryReader reader, ushort valueFormat)
        {
            this.valueFormat = valueFormat;
            if (HasFormat(valueFormat, FMT_XPlacement))
            {
                XPlacement = reader.ReadInt16();
            }
            if (HasFormat(valueFormat, FMT_YPlacement))
            {
                YPlacement = reader.ReadInt16();
            }
            if (HasFormat(valueFormat, FMT_XAdvance))
            {
                XAdvance = reader.ReadInt16();
            }
            if (HasFormat(valueFormat, FMT_YAdvance))
            {
                YAdvance = reader.ReadInt16();
            }
            if (HasFormat(valueFormat, FMT_XPlaDevice))
            {
                XPlaDevice = reader.ReadUInt16();
            }
            if (HasFormat(valueFormat, FMT_YPlaDevice))
            {
                YPlaDevice = reader.ReadUInt16();
            }
            if (HasFormat(valueFormat, FMT_XAdvDevice))
            {
                XAdvDevice = reader.ReadUInt16();
            }
            if (HasFormat(valueFormat, FMT_YAdvDevice))
            {
                YAdvDevice = reader.ReadUInt16();
            }
        }

        private static bool HasFormat(ushort value, int flags)
        {
            return (value & flags) == flags;
        }

        //Mask 	Name 	Description
        //0x0001 	XPlacement 	Includes horizontal adjustment for placement
        //0x0002 	YPlacement 	Includes vertical adjustment for placement
        //0x0004 	XAdvance 	Includes horizontal adjustment for advance
        //0x0008 	YAdvance 	Includes vertical adjustment for advance
        //0x0010 	XPlaDevice 	Includes Device table (non-variable font) / VariationIndex table (variable font) for horizontal placement
        //0x0020 	YPlaDevice 	Includes Device table (non-variable font) / VariationIndex table (variable font) for vertical placement
        //0x0040 	XAdvDevice 	Includes Device table (non-variable font) / VariationIndex table (variable font) for horizontal advance
        //0x0080 	YAdvDevice 	Includes Device table (non-variable font) / VariationIndex table (variable font) for vertical advance
        //0xFF00 	Reserved 	For future use (set to zero)

        //check bits
        private const int FMT_XPlacement = 1;

        private const int FMT_YPlacement = 1 << 1;
        private const int FMT_XAdvance = 1 << 2;
        private const int FMT_YAdvance = 1 << 3;
        private const int FMT_XPlaDevice = 1 << 4;
        private const int FMT_YPlaDevice = 1 << 5;
        private const int FMT_XAdvDevice = 1 << 6;
        private const int FMT_YAdvDevice = 1 << 7;

        public static ValueRecord CreateFrom(BinaryReader reader, ushort valueFormat)
        {
            if (valueFormat == 0)
                return null;//empty

            var v = new ValueRecord();
            v.ReadFrom(reader, valueFormat);
            return v;
        }

#if DEBUG

        public override string ToString()
        {
            StringBuilder stbuilder = new StringBuilder();
            bool appendComma = false;
            if (XPlacement != 0)
            {
                stbuilder.Append("XPlacement=" + XPlacement);
                appendComma = true;
            }

            if (YPlacement != 0)
            {
                if (appendComma) { stbuilder.Append(','); }
                stbuilder.Append(" YPlacement=" + YPlacement);
                appendComma = true;
            }
            if (XAdvance != 0)
            {
                if (appendComma) { stbuilder.Append(','); }
                stbuilder.Append(" XAdvance=" + XAdvance);
                appendComma = true;
            }
            if (YAdvance != 0)
            {
                if (appendComma) { stbuilder.Append(','); }
                stbuilder.Append(" YAdvance=" + YAdvance);
                appendComma = true;
            }
            return stbuilder.ToString();
        }

#endif
    }
}