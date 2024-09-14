using System.Collections.Generic;
using NewFontParser.Extensions;

namespace NewFontParser.Tables.TtTables
{
    public class CvtTable : IInfoTable
    {
        public static string Tag => "cvt ";

        public int FWordCount { get; }

        private readonly ushort[] _data;

        public CvtTable(byte[] data)
        {
            FWordCount = data.Length / 2;
            _data = new ushort[FWordCount];
            for (var i = 0; i < FWordCount; i++)
            {
                _data[i] = (ushort)((data[i * 2] << 8) | data[i * 2 + 1]);
            }
        }

        public List<float>? GetCvtValues(long origin, long count)
        {
            if (origin < 0 || origin >= FWordCount || count < 0 || origin + count > FWordCount)
            {
                return null;
            }
            var cvtValues = new List<float>();
            for (long i = origin; i < count; i++)
            {
                cvtValues.Add(_data[i].ToF26Dot6());
            }

            return cvtValues;
        }

        public float? GetCvtValue(int location)
        {
            if (location < 0 || location >= FWordCount)
            {
                return null;
            }
            return _data[location].ToF26Dot6();
        }

        public void WriteCvtValue(int location, float value)
        {
            if (location < 0 || location >= FWordCount)
            {
                return;
            }
            _data[location] = (ushort)value.FromF26Dot6();
        }
    }
}