using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.TtTables
{
    public class LocaTable : IInfoTable
    {
        public uint[] Offsets { get; }

        public LocaTable(byte[] data, int numGlyphs, bool isShort)
        {
            Offsets = new uint[numGlyphs + 1];
            var reader = new BigEndianReader(data);
            for (var i = 0; i < numGlyphs + 1; i++)
            {
                if (isShort)
                {
                    Offsets[i] = reader.ReadUShort();
                }
                else
                {
                    Offsets[i] = reader.ReadUint32();
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Loca Table");
            for (var i = 0; i < Offsets.Length; i++)
            {
                sb.AppendLine($"Offset {i}: {Offsets[i]}");
            }
            return sb.ToString();
        }
    }
}
