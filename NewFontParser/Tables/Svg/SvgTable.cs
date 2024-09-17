using NewFontParser.Reader;

namespace NewFontParser.Tables.Svg
{
    public class SvgTable : IFontTable
    {
        public static string Tag => "SVG ";

        public ushort Version { get; }

        public DocumentIndex Documents { get; }

        public SvgTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Version = reader.ReadUShort();
            uint docListOffset = reader.ReadUInt32();
            _ = reader.ReadUInt32();
            reader.Seek(docListOffset);
            Documents = new DocumentIndex(reader);
        }
    }
}