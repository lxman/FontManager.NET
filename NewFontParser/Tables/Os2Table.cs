using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables
{
    public class Os2Table : IInfoTable
    {
        public ushort Version { get; }

        public short XAvgCharWidth { get; }

        public ushort UsWeightClass { get; }

        public ushort UsWidthClass { get; }

        public ushort FsType { get; }

        public short YSubscriptXSize { get; }

        public short YSubscriptYSize { get; }

        public short YSubscriptXOffset { get; }

        public short YSubscriptYOffset { get; }

        public short YSuperscriptXSize { get; }

        public short YSuperscriptYSize { get; }

        public short YSuperscriptXOffset { get; }

        public short YSuperscriptYOffset { get; }

        public short YStrikeoutSize { get; }

        public short YStrikeoutPosition { get; }

        public short SFamilyClass { get; }

        public byte[] Panose { get; }

        public uint UlUnicodeRange1 { get; }

        public uint UlUnicodeRange2 { get; }

        public uint UlUnicodeRange3 { get; }

        public uint UlUnicodeRange4 { get; }

        public string AchVendId { get; }

        public ushort FsSelection { get; }

        public ushort UsFirstCharIndex { get; }

        public ushort UsLastCharIndex { get; }

        public short STypoAscender { get; }

        public short STypoDescender { get; }

        public short STypoLineGap { get; }

        public short SWinAscent { get; }

        public short SWinDescent { get; }

        public uint UlCodePageRange1 { get; }

        public uint UlCodePageRange2 { get; }

        public short SxHeight { get; }

        public short SCapHeight { get; }

        public ushort UsDefaultChar { get; }

        public ushort UsBreakChar { get; }

        public ushort UsMaxContext { get; }

        public ushort UsLowerOpticalPointSize { get; }

        public ushort UsUpperOpticalPointSize { get; }

        public Os2Table(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Version = reader.ReadUshort();
            XAvgCharWidth = reader.ReadShort();
            UsWeightClass = reader.ReadUshort();
            UsWidthClass = reader.ReadUshort();
            FsType = reader.ReadUshort();
            YSubscriptXSize = reader.ReadShort();
            YSubscriptYSize = reader.ReadShort();
            YSubscriptXOffset = reader.ReadShort();
            YSubscriptYOffset = reader.ReadShort();
            YSuperscriptXSize = reader.ReadShort();
            YSuperscriptYSize = reader.ReadShort();
            YSuperscriptXOffset = reader.ReadShort();
            YSuperscriptYOffset = reader.ReadShort();
            YStrikeoutSize = reader.ReadShort();
            YStrikeoutPosition = reader.ReadShort();
            SFamilyClass = reader.ReadShort();
            Panose = data[32..42];
            UlUnicodeRange1 = reader.ReadUint32();
            UlUnicodeRange2 = reader.ReadUint32();
            UlUnicodeRange3 = reader.ReadUint32();
            UlUnicodeRange4 = reader.ReadUint32();
            AchVendId = Encoding.ASCII.GetString(data[58..62]);
            FsSelection = reader.ReadUshort();
            UsFirstCharIndex = reader.ReadUshort();
            UsLastCharIndex = reader.ReadUshort();
            STypoAscender = reader.ReadShort();
            STypoDescender = reader.ReadShort();
            STypoLineGap = reader.ReadShort();
            SWinAscent = reader.ReadShort();
            SWinDescent = reader.ReadShort();
            if (Version > 0)
            {
                UlCodePageRange1 = reader.ReadUint32();
                UlCodePageRange2 = reader.ReadUint32();
            }
            if (Version > 1)
            {
                SxHeight = reader.ReadShort();
                SCapHeight = reader.ReadShort();
                UsDefaultChar = reader.ReadUshort();
                UsBreakChar = reader.ReadUshort();
                UsMaxContext = reader.ReadUshort();
            }

            if (Version <= 2) return;
            UsLowerOpticalPointSize = reader.ReadUshort();
            UsUpperOpticalPointSize = reader.ReadUshort();
        }
    }
}