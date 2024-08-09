using System.IO;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public class BaseArrayTable
    {
        //BaseArray table
        //Value 	Type 	                Description
        //uint16 	BaseCount 	            Number of BaseRecords
        //struct 	BaseRecord[BaseCount] 	Array of BaseRecords-in order of BaseCoverage Index

        //A BaseRecord declares one Anchor table for each mark class (including Class 0)
        //identified in the MarkRecords of the MarkArray.
        //Each Anchor table specifies one attachment point used to attach all the marks in a particular class to the base glyph.
        //A BaseRecord contains an array of offsets to Anchor tables (BaseAnchor).
        //The zero-based array of offsets defines the entire set of attachment points each base glyph uses to attach marks.
        //The offsets to Anchor tables are ordered by mark class.

        // Note: Anchor tables are not tagged with class value identifiers.
        //Instead, the index value of an Anchor table in the array defines the class value represented by the Anchor table.

        internal BaseRecord[] _records;

        public BaseRecord GetBaseRecords(int index)
        {
            return _records[index];
        }

        public static BaseArrayTable CreateFrom(BinaryReader reader, long beginAt, ushort classCount)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //---
            var baseArrTable = new BaseArrayTable();
            ushort baseCount = reader.ReadUInt16();
            baseArrTable._records = new BaseRecord[baseCount];
            // Read all baseAnchorOffsets in one go
            ushort[] baseAnchorOffsets = reader.ReadUInt16Array(classCount * baseCount);
            for (var i = 0; i < baseCount; ++i)
            {
                AnchorPoint[] anchors = new AnchorPoint[classCount];
                var baseRec = new BaseRecord(anchors);

                //each base has anchor point for mark glyph'class
                for (var n = 0; n < classCount; ++n)
                {
                    ushort offset = baseAnchorOffsets[i * classCount + n];
                    if (offset <= 0)
                    {
                        //TODO: review here
                        //bug?
                        continue;
                    }
                    anchors[n] = AnchorPoint.CreateFrom(reader, beginAt + offset);
                }

                baseArrTable._records[i] = baseRec;
            }
            return baseArrTable;
        }

#if DEBUG

        public int dbugGetRecordCount()
        {
            return _records.Length;
        }

#endif
    }
}