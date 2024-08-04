using System.IO;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public class Mark2ArrayTable
    {
        ///Mark2Array table
        //Value 	Type 	        Description
        //uint16 	Mark2Count 	    Number of Mark2 records
        //struct 	Mark2Record[Mark2Count] 	Array of Mark2 records-in Coverage order

        //Each Mark2Record contains an array of offsets to Anchor tables (Mark2Anchor).
        //The array of zero-based offsets, measured from the beginning of the Mark2Array table,
        //defines the entire set of Mark2 attachment points used to attach Mark1 glyphs to a specific Mark2 glyph.
        //The Anchor tables in the Mark2Anchor array are ordered by Mark1 class value.

        //A Mark2Record declares one Anchor table for each mark class (including Class 0)
        //identified in the MarkRecords of the MarkArray.
        //Each Anchor table specifies one Mark2 attachment point used to attach all
        //the Mark1 glyphs in a particular class to the Mark2 glyph.

        //Mark2Record
        //Value 	Type 	                    Description
        //Offset16 	Mark2Anchor[ClassCount] 	Array of offsets (one per class) to Anchor tables-from beginning of Mark2Array table-zero-based array

        public static Mark2ArrayTable CreateFrom(BinaryReader reader, long beginAt, ushort classCount)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //---
            ushort mark2Count = reader.ReadUInt16();
            ushort[] offsets = reader.ReadUInt16Array(mark2Count * classCount);
            //read mark2 anchors
            AnchorPoint[] anchors = new AnchorPoint[mark2Count * classCount];
            for (int i = 0; i < mark2Count * classCount; ++i)
            {
                anchors[i] = AnchorPoint.CreateFrom(reader, beginAt + offsets[i]);
            }
            return new Mark2ArrayTable(classCount, anchors);
        }

        public AnchorPoint GetAnchorPoint(int index, int markClassId)
        {
            return _anchorPoints[index * _classCount + markClassId];
        }

        public Mark2ArrayTable(ushort classCount, AnchorPoint[] anchorPoints)
        {
            _classCount = classCount;
            _anchorPoints = anchorPoints;
        }

        internal readonly ushort _classCount;
        internal readonly AnchorPoint[] _anchorPoints;
    }
}