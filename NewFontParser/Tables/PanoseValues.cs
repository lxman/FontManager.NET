using System.Text;

namespace NewFontParser.Tables
{
    public class PanoseValues
    {
        public byte FamilyType => _values[0];
        
        private readonly byte[] _values;
        private readonly string[] _names = new string[10]
        {
            "FamilyType",
            "SerifStyle",
            "Weight",
            "Proportion",
            "Contrast",
            "StrokeVariation",
            "ArmStyle",
            "Letterform",
            "Midline",
            "XHeight"
        };

        public PanoseValues(byte[] values)
        {
            _values = values;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < _values.Length; i++)
            {
                sb.AppendLine($"{_names[i]}: {_values[i]}");
            }

            return sb.ToString();
        }

        public string GetFamilyType()
        {
            return _values[0].ToString();
        }

        public string GetSerifStyle()
        {
            return _values[1].ToString();
        }

        public string GetWeight()
        {
            return _values[2].ToString();
        }

        public string GetProportion()
        {
            return _values[3].ToString();
        }

        public string GetContrast()
        {
            return _values[4].ToString();
        }

        public string GetStrokeVariation()
        {
            return _values[5].ToString();
        }

        public string GetArmStyle()
        {
            return _values[6].ToString();
        }

        public string GetLetterform()
        {
            return _values[7].ToString();
        }

        public string GetMidline()
        {
            return _values[8].ToString();
        }
    }
}