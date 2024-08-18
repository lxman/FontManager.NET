using NewFontParser.Reader;

namespace NewFontParser.Tables.Gdef
{
    public class ClassRangeRecord
    {
        public ushort StartGlyphID { get; }

        public ushort EndGlyphID { get; }

        public GlyphClassType GlyphClass { get; }

        public ClassRangeRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);

            StartGlyphID = reader.ReadUShort();
            EndGlyphID = reader.ReadUShort();
            GlyphClass = (GlyphClassType)reader.ReadUShort();
        }
    }
}
