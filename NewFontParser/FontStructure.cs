using System.Collections.Generic;
using NewFontParser.Models;

namespace NewFontParser
{
    public class FontStructure
    {
        public FileType FileType { get; set; }

        public ushort TableCount { get; set; }

        public ushort SearchRange { get; set; }

        public ushort EntrySelector { get; set; }

        public ushort RangeShift { get; set; }

        public List<TableRecord> TableRecords { get; set; } = new List<TableRecord>();
    }
}
