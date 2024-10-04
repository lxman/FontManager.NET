using System.Linq;
using FontParser.Reader;

namespace FontParser.Tables.Woff
{
    public class TransformedGlyfTable
    {
        public ushort Reserved { get; }

        public ushort OptionFlags { get; }

        public ushort GlyphCount { get; }

        public ushort IndexFormat { get; }

        public ushort[] NContourStream { get; }

        public ushort[] NPointsStream { get; }

        public byte[] FlagStream { get; }

        public byte[] GlyphStream { get; }

        public byte[] CompositeStream { get; }

        public short[] BBoxStream { get; }

        public byte[] InstructionStream { get; }

        public byte[] OverlapSimpleBitmap { get; }

        public TransformedGlyfTable(BigEndianReader reader)
        {
            Reserved = reader.ReadUShort();
            OptionFlags = reader.ReadUShort();
            GlyphCount = reader.ReadUShort();
            IndexFormat = reader.ReadUShort();
            uint nContourStreamSize = reader.ReadUInt32();
            uint nPointsStreamSize = reader.ReadUInt32();
            uint flagStreamSize = reader.ReadUInt32();
            uint glyphStreamSize = reader.ReadUInt32();
            uint compositeStreamSize = reader.ReadUInt32();
            uint bboxStreamSize = reader.ReadUInt32();
            uint instructionStreamSize = reader.ReadUInt32();

            long nCountStreamOffset = reader.Position;

            long nPointsStreamOffset = nCountStreamOffset + nContourStreamSize;
            long flagStreamOffset = nPointsStreamOffset + nPointsStreamSize;
            long glyphStreamOffset = flagStreamOffset + flagStreamSize;
            long compositeStreamOffset = glyphStreamOffset + glyphStreamSize;
            long bboxStreamOffset = compositeStreamOffset + compositeStreamSize;
            long instructionStreamOffset = bboxStreamOffset + bboxStreamSize;

            ushort[] nCountStream = reader.ReadUShortArray(GlyphCount);
            int contourCount =
                nCountStream
                    .Where(item => item > 0)
                    .Aggregate(0, (current, item) => current + item);

            var pointsPerContour = new ushort[contourCount];
            for (var i = 0; i < contourCount; i++)
            {
                pointsPerContour[i] = reader.Read255UInt16();
            }

            FlagStream = reader.ReadBytes(flagStreamSize);

            //NContourStream = reader.ReadUShortArray(nContourStreamSize);
            //reader.Seek(nCountStreamOffset + nPointsStreamOffset);
            //NPointsStream = reader.ReadUShortArray(nPointsStreamSize);
            //reader.Seek(nCountStreamOffset + flagStreamOffset);
            //FlagStream = reader.ReadBytes(flagStreamSize);
            //reader.Seek(nCountStreamOffset + flagStreamOffset);
            //GlyphStream = reader.ReadBytes(glyphStreamSize);
            //reader.Seek(nCountStreamOffset + compositeStreamOffset);
            //CompositeStream = reader.ReadBytes(compositeStreamSize);
            //reader.Seek(nCountStreamOffset + bboxStreamOffset);
            //BBoxStream = reader.ReadShortArray(bboxStreamSize);
            //reader.Seek(nCountStreamOffset + instructionStreamOffset);
            //InstructionStream = reader.ReadBytes(instructionStreamSize);
            //OverlapSimpleBitmap = reader.ReadBytes(GlyphCount);
        }
    }
}