using System.IO;

namespace FontParser.Tables.AdvancedLayout.GPOS.Subtables
{
    public class PosClassRule
    {
        public PosLookupRecord[] PosLookupRecords;
        public ushort[] InputGlyphIds;

        public static PosClassRule CreateFrom(BinaryReader reader, long beginAt)
        {
            //--------
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //--------
            var posClassRule = new PosClassRule();
            ushort glyphCount = reader.ReadUInt16();
            ushort posCount = reader.ReadUInt16();
            if (glyphCount > 1)
            {
                posClassRule.InputGlyphIds = reader.ReadUInt16Array(glyphCount - 1);
            }

            posClassRule.PosLookupRecords = GPOS.CreateMultiplePosLookupRecords(reader, posCount);
            return posClassRule;
        }
    }
}