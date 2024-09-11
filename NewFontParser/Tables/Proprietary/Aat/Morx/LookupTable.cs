using System;
using NewFontParser.Reader;
using NewFontParser.Tables.Proprietary.Aat.Morx.LookupTables;

namespace NewFontParser.Tables.Proprietary.Aat.Morx
{
    public class LookupTable
    {
        public ushort Format { get; }

        public IFsHeader FsHeader { get; }

        public LookupTable(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            FsHeader = Format switch
            {
                0 => new Format0(reader),
                2 => new Format2(reader),
                4 => new Format4(reader),
                6 => new Format6(reader),
                8 => new Format8(reader),
                10 => new Format10(reader),
                _ => throw new NotSupportedException($"LookupType {Format} is not supported.")
            };
        }
    }
}