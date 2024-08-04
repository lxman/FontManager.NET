using System.Collections.Generic;
using System.IO;
using FontParser.Exceptions;

namespace FontParser.Tables.AdvancedLayout.CoverageTable
{
    //https://docs.microsoft.com/en-us/typography/opentype/spec/chapter2

    public abstract class CoverageTable
    {
        public abstract int FindPosition(ushort glyphIndex);

        public abstract IEnumerable<ushort> GetExpandedValueIter();

#if DEBUG

#endif

        public static CoverageTable CreateFrom(BinaryReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            ushort format = reader.ReadUInt16();
            switch (format)
            {
                default: throw new OpenFontNotSupportedException();
                case 1: return CoverageFmt1.CreateFrom(reader);
                case 2: return CoverageFmt2.CreateFrom(reader);
            }
        }

        public static CoverageTable[] CreateMultipleCoverageTables(long initPos, ushort[] offsets, BinaryReader reader)
        {
            CoverageTable[] results = new CoverageTable[offsets.Length];
            for (int i = 0; i < results.Length; ++i)
            {
                results[i] = CreateFrom(reader, initPos + offsets[i]);
            }
            return results;
        }
    }
}