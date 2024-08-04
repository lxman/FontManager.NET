using System.IO;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public class LigatureAttachTable
    {
        //LigatureAttach table
        //Value 	Type 	                            Description
        //uint16 	ComponentCount 	                    Number of ComponentRecords in this ligature
        //struct 	ComponentRecord[ComponentCount] 	Array of Component records-ordered in writing direction
        //-------------------------------
        private ComponentRecord[] _records;

        public static LigatureAttachTable ReadFrom(BinaryReader reader, ushort classCount)
        {
            LigatureAttachTable table = new LigatureAttachTable();
            ushort componentCount = reader.ReadUInt16();
            ComponentRecord[] componentRecs = new ComponentRecord[componentCount];
            table._records = componentRecs;
            for (int i = 0; i < componentCount; ++i)
            {
                componentRecs[i] = new ComponentRecord(
                    reader.ReadUInt16Array(classCount));
            }
            return table;
        }

        public ComponentRecord GetComponentRecord(int index) => _records[index];
    }
}