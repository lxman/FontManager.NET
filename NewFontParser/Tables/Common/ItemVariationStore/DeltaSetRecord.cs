using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.ItemVariationStore
{
    public class DeltaSetRecord
    {
        public int[] DeltaData { get; }
        
        public DeltaSetRecord(BigEndianReader reader, ushort regionIndexCount, bool useLongWords, int wordDeltaCount)
        {
            if (wordDeltaCount > regionIndexCount) return;
            DeltaData = new int[regionIndexCount];
            var index = 0;
            for (var i = 0; i < wordDeltaCount; i++)
            {
                DeltaData[index++] = useLongWords ? reader.ReadInt32() : reader.ReadInt16();
            }

            for (int i = wordDeltaCount; i < regionIndexCount; i++)
            {
                DeltaData[index++] = useLongWords ? reader.ReadInt16() : reader.ReadSByte();
            }
        }
    }
}