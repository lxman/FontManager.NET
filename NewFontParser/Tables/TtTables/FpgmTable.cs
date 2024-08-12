using System.Collections.Generic;

namespace NewFontParser.Tables.TtTables
{
    public class FpgmTable : IInfoTable
    {
        public long Count { get; }

        private readonly Stack<byte> _instructionStream = new Stack<byte>();

        public FpgmTable(byte[] data)
        {
            Count = data.Length;
            foreach (byte b in data)
            {
                _instructionStream.Push(b);
            }
        }
    }
}
