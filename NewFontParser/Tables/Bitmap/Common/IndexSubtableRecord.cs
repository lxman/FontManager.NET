﻿using System;
using System.Buffers.Binary;
using NewFontParser.Reader;
using NewFontParser.Tables.Bitmap.Common.IndexSubtables;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.

namespace NewFontParser.Tables.Bitmap.Common
{
    public class IndexSubtableRecord
    {
        public ushort FirstGlyphIndex { get; }

        public ushort LastGlyphIndex { get; }

        public IIndexSubtable Subtable { get; private set; }

        private readonly long _readerStart;

        public IndexSubtableRecord(BigEndianReader reader, long start)
        {
            FirstGlyphIndex = reader.ReadUShort();
            LastGlyphIndex = reader.ReadUShort();
            uint offset = reader.ReadUInt32();
            _readerStart = start + offset;
        }

        public void ReadSubtable(BigEndianReader reader)
        {
            reader.Seek(_readerStart);
            ushort format = BinaryPrimitives.ReadUInt16BigEndian(reader.PeekBytes(2));
            var numOffsets = Convert.ToUInt16(LastGlyphIndex - FirstGlyphIndex + 2);
            Subtable = format switch
            {
                1 => new Format1(reader, numOffsets),
                2 => new Format2(reader),
                3 => new Format3(reader, numOffsets),
                4 => new Format4(reader),
                5 => new Format5(reader),
                _ => Subtable
            };
        }
    }
}