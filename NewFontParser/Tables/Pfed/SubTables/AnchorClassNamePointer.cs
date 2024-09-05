using NewFontParser.Reader;

namespace NewFontParser.Tables.Pfed.SubTables
{
    public class AnchorClassNamePointer
    {
        public string AnchorClassName { get; }

        public AnchorClassNamePointer(BigEndianReader reader)
        {
            reader.Seek(reader.ReadUShort());
            AnchorClassName = reader.ReadNullTerminatedString(false);
        }
    }
}