using System.Collections.Generic;

namespace NewFontParser.Tables.TtTables
{
    public class CvtTable : IInfoTable
    {
        public long FWordCount { get; }

        private readonly byte[] _data;

        public CvtTable(byte[] data)
        {
            FWordCount = data.Length / 2;
            _data = data;
        }

        public List<short>? GetCvtValues(long origin, long count)
        {
            if (origin < 0 || origin >= FWordCount || count < 0 || origin + count > FWordCount)
            {
                return null;
            }
            var cvtValues = new List<short>();
            for (long i = origin * 2; i < count * 2; i += 2)
            {
                cvtValues.Add((short)((_data[i] << 8) | _data[i + 1]));
            }

            return cvtValues;
        }
    }
}