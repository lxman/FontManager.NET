using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Svg
{
    public class DocumentIndexEntry
    {
        public ushort StartGlyphId { get; }

        public ushort EndGlyphId { get; }

        public string Instructions { get; private set; }

        private byte[] _svgDocument;

        private readonly uint _svgDocOffset;
        private readonly uint _svgDocLength;

        public DocumentIndexEntry(BigEndianReader reader, long docIndexStart)
        {
            StartGlyphId = reader.ReadUShort();
            EndGlyphId = reader.ReadUShort();
            _svgDocOffset = reader.ReadUInt32();
            _svgDocLength = reader.ReadUInt32();
        }

        public void ReadDocument(BigEndianReader reader, long docIndexStart)
        {
            reader.Seek(docIndexStart + _svgDocOffset);
            _svgDocument = reader.ReadBytes(_svgDocLength);
            Instructions = Encoding.UTF8.GetString(_svgDocument);
        }
    }
}
