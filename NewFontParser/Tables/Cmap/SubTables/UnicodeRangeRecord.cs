﻿using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class UnicodeRangeRecord
    {
        public static long RecordSize => 4;

        public uint StartUnicodeValue { get; set; }

        public byte AdditionalCount { get; set; }

        public UnicodeRangeRecord(byte[] data)
        {
            var reader = new BigEndianReader(data);
            StartUnicodeValue = reader.ReadUint24();
            AdditionalCount = reader.ReadByte();
        }
    }
}