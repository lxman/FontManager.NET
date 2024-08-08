using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Head
{
    public class HeadTable : IInfoTable
    {
        public ushort MajorVersion { get; }

        public ushort MinorVersion { get; }

        public string Version => $"{MajorVersion}.{MinorVersion}";

        public float FontRevision { get; }

        public uint CheckSumAdjustment { get; }

        public uint MagicNumber { get; }

        public ushort Flags { get; }

        public ushort UnitsPerEm { get; }

        public long Created { get; }

        public long Modified { get; }

        public short XMin { get; }

        public short YMin { get; }

        public short XMax { get; }

        public short YMax { get; }

        public ushort MacStyle { get; }

        public ushort LowestRecPpem { get; }

        public short FontDirectionHint { get; }

        public short IndexToLocFormat { get; }

        public short GlyphDataFormat { get; }

        public HeadTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            MajorVersion = reader.ReadUshort();
            MinorVersion = reader.ReadUshort();
            FontRevision = reader.ReadFixed();
            CheckSumAdjustment = reader.ReadUint32();
            MagicNumber = reader.ReadUint32();
            Flags = reader.ReadUshort();
            UnitsPerEm = reader.ReadUshort();
            Created = reader.ReadLongDateTime();
            Modified = reader.ReadLongDateTime();
            XMin = reader.ReadShort();
            YMin = reader.ReadShort();
            XMax = reader.ReadShort();
            YMax = reader.ReadShort();
            MacStyle = reader.ReadUshort();
            LowestRecPpem = reader.ReadUshort();
            FontDirectionHint = reader.ReadShort();
            IndexToLocFormat = reader.ReadShort();
            GlyphDataFormat = reader.ReadShort();
        }

        public List<HeadFlags> HeadFlags() => Enum.GetValues(typeof(HeadFlags))
                .Cast<HeadFlags>()
                .Where(flag => (Flags & (ushort)flag) == (ushort)flag)
                .ToList();

        public List<MacStyle> HeadMacStyles() => Enum.GetValues(typeof(MacStyle))
                .Cast<MacStyle>()
                .Where(style => (MacStyle & (ushort)style) == (ushort)style)
                .ToList();

        public FontDirectionHint HeadFontDirectionHint() => (FontDirectionHint)FontDirectionHint;

        public IndexToLocFormat HeadIndexToLocFormat() => (IndexToLocFormat)IndexToLocFormat;

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("HeadTable");
            builder.AppendLine($"Version: {Version}");
            builder.AppendLine($"FontRevision: {FontRevision}");
            builder.AppendLine($"CheckSumAdjustment: {CheckSumAdjustment}");
            builder.AppendLine($"MagicNumber: {MagicNumber}");
            builder.AppendLine($"Flags: {Flags}");
            builder.AppendLine($"UnitsPerEm: {UnitsPerEm}");
            builder.AppendLine($"Created: {Created}");
            builder.AppendLine($"Modified: {Modified}");
            builder.AppendLine($"XMin: {XMin}");
            builder.AppendLine($"YMin: {YMin}");
            builder.AppendLine($"XMax: {XMax}");
            builder.AppendLine($"YMax: {YMax}");
            builder.AppendLine($"MacStyle: {MacStyle}");
            builder.AppendLine($"LowestRecPPEM: {LowestRecPpem}");
            builder.AppendLine($"FontDirectionHint: {FontDirectionHint}");
            builder.AppendLine($"IndexToLocFormat: {IndexToLocFormat}");
            builder.AppendLine($"GlyphDataFormat: {GlyphDataFormat}");
            return builder.ToString();
        }
    }
}