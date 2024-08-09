using System.Collections.Generic;

namespace NewFontParser.Tables.TtTables
{
    public class FpgmTable : IInfoTable
    {
        public long Count { get; }

        private readonly byte[] _data;

        public FpgmTable(byte[] data)
        {
            Count = data.Length;
            _data = data;
        }

        public List<byte>? GetInstructions(long origin, long count)
        {
            if (origin < 0 || origin >= Count || count < 0 || origin + count > Count)
            {
                return null;
            }
            var instructions = new List<byte>();
            for (long i = origin; i < count; i++)
            {
                instructions.Add(_data[i]);
            }

            return instructions;
        }
    }
}
