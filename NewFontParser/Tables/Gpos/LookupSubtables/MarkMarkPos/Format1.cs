using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.CoverageFormat;
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

        public ICoverageFormat MarkCoverage { get; }

        public ICoverageFormat Mark2Coverage { get; }

        public MarkArray MarkArray { get; }

        public Mark2Array Mark2Array { get; }

        public Format1(BigEndianReader reader)
        {
            long tableBase = reader.Position;

            Format = reader.ReadUShort();
            Mark1CoverageOffset = reader.ReadUShort();
            Mark2CoverageOffset = reader.ReadUShort();
            MarkClassCount = reader.ReadUShort();
            MarkArrayOffset = reader.ReadUShort();
            Mark2ArrayOffset = reader.ReadUShort();

            reader.Seek(tableBase + Mark1CoverageOffset);
            byte mark1CoverageFormat = reader.PeekBytes(2)[1];
            switch (mark1CoverageFormat)
            {
                case 1:
                    MarkCoverage = new CoverageFormat.Format1(reader);
                    break;

                case 2:
                    MarkCoverage = new Format2(reader);
                    break;
            }

            reader.Seek(tableBase + Mark2CoverageOffset);
            byte mark2CoverageFormat = reader.PeekBytes(2)[1];
            switch (mark2CoverageFormat)
            {
                case 1:
                    Mark2Coverage = new CoverageFormat.Format1(reader);
                    break;

                case 2:
                    Mark2Coverage = new Format2(reader);
                    break;
            }
            reader.Seek(tableBase + MarkArrayOffset);
            MarkArray = new MarkArray(reader);
            //reader.Seek(Mark2ArrayOffset);
            //Mark2Array = new Mark2Array(reader, MarkClassCount);
        }
    }
}