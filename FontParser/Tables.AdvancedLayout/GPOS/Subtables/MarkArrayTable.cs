using System.IO;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public class MarkArrayTable
    {
        //Mark Array
        //The MarkArray table defines the class and the anchor point for a mark glyph.
        //Three GPOS subtables-MarkToBase, MarkToLigature, and MarkToMark Attachment
        //use the MarkArray table to specify data for attaching marks.

        //The MarkArray table contains a count of the number of mark records (MarkCount) and an array of those records (MarkRecord).
        //Each mark record defines the class of the mark and an offset to the Anchor table that contains data for the mark.

        //A class value can be 0 (zero), but the MarkRecord must explicitly assign that class value (this differs from the ClassDef table,
        //in which all glyphs not assigned class values automatically belong to Class 0).
        //The GPOS subtables that refer to MarkArray tables use the class assignments for indexing zero-based arrays that contain data for each mark class.

        // MarkArray table
        //-------------------
        //Value 	Type 	                Description
        //uint16 	MarkCount 	            Number of MarkRecords
        //struct 	MarkRecord[MarkCount] 	Array of MarkRecords in Coverage order
        //
        //MarkRecord
        //Value 	Type 	                Description
        //-------------------
        //uint16 	Class 	                Class defined for this mark
        //Offset16 	MarkAnchor 	            Offset to Anchor table-from beginning of MarkArray table
        internal MarkRecord[] _records;

        internal AnchorPoint[] _anchorPoints;

        public AnchorPoint GetAnchorPoint(int index)
        {
            return _anchorPoints[index];
        }

        public ushort GetMarkClass(int index)
        {
            return _records[index].markClass;
        }

        private void ReadFrom(BinaryReader reader)
        {
            long markTableBeginAt = reader.BaseStream.Position;
            ushort markCount = reader.ReadUInt16();
            _records = new MarkRecord[markCount];
            for (var i = 0; i < markCount; ++i)
            {
                //1 mark : 1 anchor
                _records[i] = new MarkRecord(
                    reader.ReadUInt16(),//mark class
                    reader.ReadUInt16()); //offset to anchor table
            }
            //---------------------------
            //read anchor
            _anchorPoints = new AnchorPoint[markCount];
            for (var i = 0; i < markCount; ++i)
            {
                MarkRecord markRec = _records[i];
                //bug?
                if (markRec.offset < 0)
                {
                    //TODO: review here
                    //found err on Tahoma
                    continue;
                }
                //read table detail
                _anchorPoints[i] = AnchorPoint.CreateFrom(reader, markTableBeginAt + markRec.offset);
            }
        }

#if DEBUG

        public int dbugGetAnchorCount()
        {
            return _anchorPoints.Length;
        }

#endif

        public static MarkArrayTable CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            var markArrTable = new MarkArrayTable();
            markArrTable.ReadFrom(reader);
            return markArrTable;
        }
    }
}