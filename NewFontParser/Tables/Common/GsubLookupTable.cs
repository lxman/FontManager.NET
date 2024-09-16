using System;
using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.ChainedSequenceContext.Format1;
using NewFontParser.Tables.Common.ChainedSequenceContext.Format2;
using NewFontParser.Tables.Common.ChainedSequenceContext.Format3;
using NewFontParser.Tables.Common.SequenceContext.Format1;
using NewFontParser.Tables.Common.SequenceContext.Format2;
using NewFontParser.Tables.Common.SequenceContext.Format3;

namespace NewFontParser.Tables.Common
{
    public class GsubLookupTable
    {
        public List<ILookupSubTable> SubTables { get; } = new List<ILookupSubTable>();

        public GsubLookupTable(BigEndianReader reader)
        {
            long startOfTable = reader.Position;
            var lookupType = (GsubLookupType)reader.ReadUShort();
            var lookupFlags = (LookupFlag)reader.ReadUShort();
            ushort subTableCount = reader.ReadUShort();
            ushort[] subTableOffsets = reader.ReadUShortArray(subTableCount);
            ushort? markFilteringSet = null;
            if (lookupFlags.HasFlag(LookupFlag.UseMarkFilteringSet))
            {
                markFilteringSet = reader.ReadUShort();
            }
            for (var i = 0; i < subTableCount; i++)
            {
                reader.Seek(startOfTable + subTableOffsets[i]);
                switch (lookupType)
                {
                    case GsubLookupType.SingleSubstitution:
                        byte subType = reader.PeekBytes(2)[1];
                        switch (subType)
                        {
                            case 1:
                                SubTables.Add(new Gsub.LookupSubTables.SingleSubstitution.Format1(reader));
                                break;
                            case 2:
                                SubTables.Add(new Gsub.LookupSubTables.SingleSubstitution.Format2(reader));
                                break;
                        }
                        break;
                    case GsubLookupType.MultipleSubstitution:
                        SubTables.Add(new Gsub.LookupSubTables.MultipleSubstitution.Format1(reader));
                        break;
                    case GsubLookupType.AlternateSubstitution:
                        SubTables.Add(new Gsub.LookupSubTables.AlternateSubstitution.Format1(reader));
                        break;
                    case GsubLookupType.LigatureSubstitution:
                        SubTables.Add(new Gsub.LookupSubTables.LigatureSubstitution.Format1(reader));
                        break;
                    case GsubLookupType.ContextSubstitution:
                        subType = reader.PeekBytes(2)[1];
                        switch (subType)
                        {
                            case 1:
                                SubTables.Add(new SequenceContextFormat1(reader));
                                break;
                            case 2:
                                SubTables.Add(new SequenceContextFormat2(reader));
                                break;
                            case 3:
                                SubTables.Add(new SequenceContextFormat3(reader));
                                break;
                        }
                        break;
                    case GsubLookupType.ChainedContextSubstitution:
                        subType = reader.PeekBytes(2)[1];
                        switch (subType)
                        {
                            case 1:
                                SubTables.Add(new ChainedSequenceContextFormat1(reader));
                                break;
                            case 2:
                                SubTables.Add(new ChainedSequenceContextFormat2(reader));
                                break;
                            case 3:
                                SubTables.Add(new ChainedSequenceContextFormat3(reader));
                                break;
                        }
                        break;
                    case GsubLookupType.SubstitutionExtension:
                        SubTables.Add(new Gsub.LookupSubTables.SubstitutionExtension.Format1(reader));
                        break;
                    case GsubLookupType.ReverseChainedContexts:
                        SubTables.Add(new Gsub.LookupSubTables.ReverseChainSingleSubstitution.Format1(reader));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}