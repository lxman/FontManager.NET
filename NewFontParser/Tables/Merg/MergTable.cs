﻿using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.ClassDefinition;

namespace NewFontParser.Tables.Merg
{
    public class MergTable : IInfoTable
    {
        public static string Tag => "MERG";

        public ushort Version { get; }

        public List<IClassDefinition> ClassDefinitions { get; } = new List<IClassDefinition>();

        public MergTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Version = reader.ReadUShort();
            ushort mergeClassCount = reader.ReadUShort();
            ushort mergeDataOffset = reader.ReadUShort();
            ushort classDefCount = reader.ReadUShort();
            ushort offsetToClassDefOffsets = reader.ReadUShort();
            if (offsetToClassDefOffsets == 0) return;
            reader.Seek(offsetToClassDefOffsets);
            ushort[] classDefOffsets = reader.ReadUShortArray(classDefCount);
            for (var i = 0; i < classDefCount; i++)
            {
                reader.Seek(classDefOffsets[i]);
                ushort format = reader.PeekBytes(2)[1];
                switch (format)
                {
                    case 1:
                        ClassDefinitions.Add(new Format1(reader));
                        break;

                    case 2:
                        ClassDefinitions.Add(new Format2(reader));
                        break;
                }
            }
        }
    }
}