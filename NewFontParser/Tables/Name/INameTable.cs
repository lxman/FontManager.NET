using System.Collections.Generic;

namespace NewFontParser.Tables.Name
{
    public interface INameTable
    {
        ushort Format { get; }

        ushort Count { get; }

        ushort StringOffset { get; }

        List<NameRecord> NameRecords { get; }
    }
}
