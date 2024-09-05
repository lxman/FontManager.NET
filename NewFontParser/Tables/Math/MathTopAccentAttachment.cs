using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.CoverageFormat;

namespace NewFontParser.Tables.Math
{
    public class MathTopAccentAttachment
    {
        public ICoverageFormat TopAccentCoverage { get; }

        private List<MathValueRecord> TopAccentAttachments { get; } = new List<MathValueRecord>();

        public MathTopAccentAttachment(BigEndianReader reader)
        {
            long position = reader.Position;

            ushort topAccentCoverageOffset = reader.ReadUShort();

            ushort topAccentAttachmentCount = reader.ReadUShort();

            for (var i = 0; i < topAccentAttachmentCount; i++)
            {
                TopAccentAttachments.Add(new MathValueRecord(reader, position));
            }

            reader.Seek(position + topAccentCoverageOffset);

            byte format = reader.PeekBytes(2)[1];
            TopAccentCoverage = format switch
            {
                1 => new Format1(reader),
                2 => new Format2(reader),
                _ => TopAccentCoverage
            };
        }
    }
}