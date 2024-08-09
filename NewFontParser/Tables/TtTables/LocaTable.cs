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
                    Offsets[i] = reader.ReadUshort();
                }
                else
                {
                    Offsets[i] = (ushort)(reader.ReadUshort() / 2);
                }
            }
        }
    }
}
