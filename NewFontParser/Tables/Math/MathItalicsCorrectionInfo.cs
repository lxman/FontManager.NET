using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Math
{
    public class MathItalicsCorrectionInfo
    {
        public ICoverageFormat Coverage { get; }

        public List<MathValueRecord> ItalicsCorrections { get; } = new List<MathValueRecord>();

        public MathItalicsCorrectionInfo(BigEndianReader reader)
        {
            long position = reader.Position;

            ushort italicCorrectionCoverageOffset = reader.ReadUShort();

            ushort italicsCorrectionCount = reader.ReadUShort();

            for (var i = 0; i < italicsCorrectionCount; i++)
            {
                ItalicsCorrections.Add(new MathValueRecord(reader, position));
            }

            reader.Seek(position + italicCorrectionCoverageOffset);
            Coverage = CoverageTable.Retrieve(reader);
        }
    }
}