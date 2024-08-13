﻿using System;
using System.Collections.Generic;
using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Name
{
    public class NameTable0 : IInfoTable, INameTable
    {
        public ushort Format { get; }

        public ushort Count { get; }

        public ushort StringStorageOffset { get; }

        public List<NameRecord> NameRecords { get; } = new List<NameRecord>();

        public NameTable0(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Format = reader.ReadUShort();
            Count = reader.ReadUShort();
            StringStorageOffset = reader.ReadUShort();

            for (var i = 0; i < Count; i++)
            {
                NameRecords.Add(new NameRecord(reader.ReadBytes(NameRecord.RecordSize)));
            }

            foreach (NameRecord? nameRecord in NameRecords)
            {
                reader.Seek(Convert.ToUInt32(StringStorageOffset + nameRecord.Offset));
                switch (nameRecord.EncodingId)
                {

                }

                nameRecord.Name = Encoding.GetEncoding("UTF-16BE").GetString(reader.ReadBytes(nameRecord.Length));
            }
        }
    }
}
