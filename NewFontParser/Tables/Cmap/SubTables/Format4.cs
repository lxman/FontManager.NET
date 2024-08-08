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
            Format = reader.ReadUint16();
            Length = reader.ReadUint16();
            Language = reader.ReadInt16();
            SegCountX2 = reader.ReadUint16();
            SearchRange = reader.ReadUint16();
            EntrySelector = reader.ReadUint16();
            RangeShift = reader.ReadUint16();

            for (var i = 0; i < SegCountX2 / 2; i++)
            {
                EndCodes.Add(reader.ReadUshort());
            }

            ReservedPad = reader.ReadUshort();

            for (var i = 0; i < SegCountX2 / 2; i++)
            {
                StartCodes.Add(reader.ReadUshort());
            }

            for (var i = 0; i < SegCountX2 / 2; i++)
            {
                IdDeltas.Add(reader.ReadShort());
            }

            for (var i = 0; i < SegCountX2 / 2; i++)
            {
                IdRangeOffsets.Add(reader.ReadUshort());
            }

            uint remainingBytes = Length - 16 - (SegCountX2 * 4);
            for (var i = 0; i < remainingBytes / 2; i++)
            {
                GlyphIdArray.Add(reader.ReadUshort());
            }
        }
    }
}