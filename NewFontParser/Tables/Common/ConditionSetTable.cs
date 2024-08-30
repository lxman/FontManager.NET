using NewFontParser.Reader;
using NewFontParser.Tables.Common.Condition.Format1;

namespace NewFontParser.Tables.Common
{
    public class ConditionSetTable
    {
        public ushort ConditionCount { get; }

        public uint[] ConditionOffsets { get; }

        public ConditionTableFormat1[] Conditions { get; }

        public ConditionSetTable(BigEndianReader reader)
        {
            ConditionCount = reader.ReadUShort();

            ConditionOffsets = reader.ReadUInt32Array(ConditionCount);

            var offsets = new ReadSubTablesFromOffset32Array<ConditionTableFormat1>(reader, ConditionOffsets);
            Conditions = new ConditionTableFormat1[ConditionCount];
            for (var i = 0; i < ConditionCount; i++)
            {
                reader.Seek(ConditionOffsets[i]);
                Conditions[i] = new ConditionTableFormat1(reader);
            }
        }
    }
}