using System.IO;

namespace FontParser.Tables.AdvancedLayout.GSUB.Subtables.LookupTable
{
    public class LigatureSetTable
    {
        //LigatureSet table: All ligatures beginning with the same glyph
        //Type 	    Name 	        Description
        //uint16 	LigatureCount 	Number of Ligature tables
        //Offset16 	Ligature[LigatureCount] 	Array of offsets to Ligature tables-from beginning of LigatureSet table-ordered by preference

        public LigatureTable[] Ligatures { get; set; }

        public static LigatureSetTable CreateFrom(BinaryReader reader, long beginAt)
        {
            LigatureSetTable ligSetTable = new LigatureSetTable();
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            ushort ligCount = reader.ReadUInt16(); //Number of Ligature tables
            ushort[] ligOffsets = reader.ReadUInt16Array(ligCount);
            //
            LigatureTable[] ligTables = ligSetTable.Ligatures = new LigatureTable[ligCount];
            for (int i = 0; i < ligCount; ++i)
            {
                ligTables[i] = LigatureTable.CreateFrom(reader, beginAt + ligOffsets[i]);
            }
            return ligSetTable;
        }
    }
}