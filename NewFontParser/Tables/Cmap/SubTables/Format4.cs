using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class Format4 : ICmapSubtable
    {
        public uint Format { get; }

        public uint Length { get; }

        public int Language { get; }

        public uint SegCountX2 { get; }

        public uint SearchRange { get; }

        public uint EntrySelector { get; }

        public uint RangeShift { get; }

        public List<ushort> EndCodes { get; } = new List<ushort>();

        public ushort ReservedPad { get; }

        public List<ushort> StartCodes { get; } = new List<ushort>();

        public List<short> IdDeltas { get; } = new List<short>();

        public List<ushort> IdRangeOffsets { get; } = new List<ushort>();

        public List<ushort> GlyphIdArray { get; } = new List<ushort>();

        public Format4(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            Length = reader.ReadUShort();
            Language = reader.ReadInt16();
            SegCountX2 = reader.ReadUShort();
            SearchRange = reader.ReadUShort();
            EntrySelector = reader.ReadUShort();
            RangeShift = reader.ReadUShort();

            for (var i = 0; i < SegCountX2 / 2; i++)
            {
                EndCodes.Add(reader.ReadUShort());
            }

            ReservedPad = reader.ReadUShort();

            for (var i = 0; i < SegCountX2 / 2; i++)
            {
                StartCodes.Add(reader.ReadUShort());
            }

            for (var i = 0; i < SegCountX2 / 2; i++)
            {
                IdDeltas.Add(reader.ReadShort());
            }

            for (var i = 0; i < SegCountX2 / 2; i++)
            {
                IdRangeOffsets.Add(reader.ReadUShort());
            }

            uint remainingBytes = Length - 16 - (SegCountX2 * 4);
            for (var i = 0; i < remainingBytes / 2; i++)
            {
                GlyphIdArray.Add(reader.ReadUShort());
            }
        }
    }
}