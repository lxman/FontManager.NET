using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Morx.LookupTables
{
    public class Format4 : IFsHeader
    {
        public BinarySearchHeader BinarySearchHeader { get; }

        public List<LookupSegment> Segments { get; } = new List<LookupSegment>();

        public Format4(BigEndianReader reader)
        {
            BinarySearchHeader = new BinarySearchHeader(reader);
            for (var i = 0; i < BinarySearchHeader.NUnits; i++)
            {
                Segments.Add(new LookupSegment(reader));
            }
        }
    }
}
