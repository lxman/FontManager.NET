using System;
using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.CoverageFormat;

namespace NewFontParser.Tables.Math
{
    public class MathItalicsCorrectionInfo
    {
        public ICoverageFormat CoverageTable { get; }

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

            int format = reader.PeekBytes(2)[1];

            CoverageTable = format switch
            {
                1 => new Format1(reader),
                2 => new Format2(reader),
                _ => throw new ArgumentException("Unknown format version")
            };
        }
    }
}