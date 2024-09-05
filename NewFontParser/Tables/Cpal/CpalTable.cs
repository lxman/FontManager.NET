using System.Drawing;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cpal
{
    public class CpalTable : IInfoTable
    {
        public static string Tag => "CPAL";

        public ushort Version { get; }

        public Color[] Colors { get; }

        public PaletteType[]? PaletteTypeArray { get; }

        public ushort[]? PaletteLabelArray { get; }

        public ushort[]? PaletteEntryLabelArray { get; }

        public CpalTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Version = reader.ReadUShort();
            ushort numPaletteEntries = reader.ReadUShort();
            ushort numPalettes = reader.ReadUShort();
            ushort numColorRecords = reader.ReadUShort();
            uint offsetFirstColorRecord = reader.ReadUInt32();
            var paletteOffsets = new ushort[numPalettes];
            for (var i = 0; i < numPalettes; i++)
            {
                paletteOffsets[i] = reader.ReadUShort();
            }

            Colors = new Color[numColorRecords];
            reader.Seek(offsetFirstColorRecord);
            for (var i = 0; i < numColorRecords; i++)
            {
                byte blue = reader.ReadByte();
                byte green = reader.ReadByte();
                byte red = reader.ReadByte();
                byte alpha = reader.ReadByte();
                Colors[i] = Color.FromArgb(alpha, red, green, blue);
            }

            if (Version == 0) return;
            uint offsetPaletteTypeArray = reader.ReadUInt32();
            uint offsetPaletteLabelArray = reader.ReadUInt32();
            uint offsetPaletteEntryLabelArray = reader.ReadUInt32();

            PaletteTypeArray = new PaletteType[numPalettes];
            reader.Seek(offsetPaletteTypeArray);
            for (var i = 0; i < numPalettes; i++)
            {
                PaletteTypeArray[i] = (PaletteType)reader.ReadUInt32();
            }

            PaletteLabelArray = new ushort[numPalettes];
            reader.Seek(offsetPaletteLabelArray);
            for (var i = 0; i < numPalettes; i++)
            {
                PaletteLabelArray[i] = reader.ReadUShort();
            }

            PaletteEntryLabelArray = new ushort[numPaletteEntries];
            reader.Seek(offsetPaletteEntryLabelArray);
            for (var i = 0; i < numPaletteEntries; i++)
            {
                PaletteEntryLabelArray[i] = reader.ReadUShort();
            }
        }
    }
}