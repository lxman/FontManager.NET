namespace NewFontParser.Models
{
    public class TableRecord
    {
        public string Tag { get; set; }

        public uint CheckSum { get; set; }

        public uint Offset { get; set; }

        public uint Length { get; set; }
    }
}
