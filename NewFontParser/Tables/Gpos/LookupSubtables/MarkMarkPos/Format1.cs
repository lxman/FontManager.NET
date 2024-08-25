using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Gpos.LookupSubtables.Common;

namespace NewFontParser.Tables.Gpos.LookupSubtables.MarkMarkPos
{
    public class Format1 : ILookupSubTable
    {
        public ushort Format { get; }

        public ushort Mark1CoverageOffset { get; }

        public ushort Mark2CoverageOffset { get; }

        public ushort MarkClassCount { get; }

        public ushort MarkArrayOffset { get; }

        public ushort Mark2ArrayOffset { get; }

        public ushort[] MarkCoverage { get; }

        public ushort[] Mark2Coverage { get; }

        public MarkArray MarkArray { get; }

        public Mark2Array Mark2Array { get; }

        public Format1(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            Mark1CoverageOffset = reader.ReadUShort();
            Mark2CoverageOffset = reader.ReadUShort();
            MarkClassCount = reader.ReadUShort();
            MarkArrayOffset = reader.ReadUShort();
            Mark2ArrayOffset = reader.ReadUShort();

            //MarkCoverage = new CoverageTable(data, Mark1CoverageOffset).GlyphArray;
            //Mark2Coverage = new CoverageTable(data, Mark2CoverageOffset).GlyphArray;
            reader.Seek(MarkArrayOffset);
            MarkArray = new MarkArray(reader, MarkArrayOffset);
            reader.Seek(MarkClassCount);
            Mark2Array = new Mark2Array(reader, MarkClassCount);
        }
    }
}
