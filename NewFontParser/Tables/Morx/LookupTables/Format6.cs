using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx.LookupTables
{
    public class Format6 : IFsHeader
    {
        public BinarySearchHeader BinarySearchHeader { get; }

        public List<LookupSingle> Entries { get; } = new List<LookupSingle>();

        public Format6(BigEndianReader reader)
        {
            BinarySearchHeader = new BinarySearchHeader(reader);
            for (var i = 0; i < BinarySearchHeader.NUnits; i++)
            {
                Entries.Add(new LookupSingle(reader, BinarySearchHeader.UnitSize));
            }
        }
    }
}