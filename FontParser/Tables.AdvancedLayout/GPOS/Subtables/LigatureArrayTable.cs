using System.IO;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    // LigatureArray table
    //Value 	Type 	Description
    //USHORT 	LigatureCount 	Number of LigatureAttach table offsets
    //Offset 	LigatureAttach
    //[LigatureCount] 	Array of offsets to LigatureAttach tables-from beginning of LigatureArray table-ordered by LigatureCoverage Index

    //Each LigatureAttach table consists of an array (ComponentRecord) and count (ComponentCount) of the component glyphs in a ligature. The array stores the ComponentRecords in the same order as the components in the ligature. The order of the records also corresponds to the writing direction of the text. For text written left to right, the first component is on the left; for text written right to left, the first component is on the right.
    //-------------------------------
    //LigatureAttach table
    //Value 	Type 	                            Description
    //uint16 	ComponentCount 	                    Number of ComponentRecords in this ligature
    //struct 	ComponentRecord[ComponentCount] 	Array of Component records-ordered in writing direction
    //-------------------------------
    //A ComponentRecord, one for each component in the ligature, contains an array of offsets to the Anchor tables that define all the attachment points used to attach marks to the component (LigatureAnchor). For each mark class (including Class 0) identified in the MarkArray records, an Anchor table specifies the point used to attach all the marks in a particular class to the ligature base glyph, relative to the component.

    //In a ComponentRecord, the zero-based LigatureAnchor array lists offsets to Anchor tables by mark class. If a component does not define an attachment point for a particular class of marks, then the offset to the corresponding Anchor table will be NULL.

    //Example 8 at the end of this chapter shows a MarkLisPosFormat1 subtable used to attach mark accents to a ligature glyph in the Arabic script.
    //-------------------
    //ComponentRecord
    //Value 	Type 	Description
    //Offset16 	LigatureAnchor[ClassCount] 	Array of offsets (one per class) to Anchor tables-from beginning of LigatureAttach table-ordered by class-NULL if a component does not have an attachment for a class-zero-based array
    public class LigatureArrayTable
    {
        private LigatureAttachTable[] _ligatures;

        public void ReadFrom(BinaryReader reader, ushort classCount)
        {
            long startPos = reader.BaseStream.Position;
            ushort ligatureCount = reader.ReadUInt16();
            ushort[] offsets = reader.ReadUInt16Array(ligatureCount);

            _ligatures = new LigatureAttachTable[ligatureCount];

            for (var i = 0; i < ligatureCount; ++i)
            {
                //each ligature table
                reader.BaseStream.Seek(startPos + offsets[i], SeekOrigin.Begin);
                _ligatures[i] = LigatureAttachTable.ReadFrom(reader, classCount);
            }
        }

        public LigatureAttachTable GetLigatureAttachTable(int index) => _ligatures[index];
    }
}