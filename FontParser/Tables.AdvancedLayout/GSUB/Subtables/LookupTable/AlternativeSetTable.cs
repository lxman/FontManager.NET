using System.IO;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    internal class AlternativeSetTable
    {
        public ushort[] alternativeGlyphIds;

        public static AlternativeSetTable CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            AlternativeSetTable altTable = new AlternativeSetTable();
            ushort glyphCount = reader.ReadUInt16();
            altTable.alternativeGlyphIds = reader.ReadUInt16Array(glyphCount);
            return altTable;
        }
    }
}