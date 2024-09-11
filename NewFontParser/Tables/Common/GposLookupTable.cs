using System;
using System.Collections.Generic;
using NewFontParser.Reader;
using NewFontParser.Tables.Common.ChainedSequenceContext.Format1;
using NewFontParser.Tables.Common.ChainedSequenceContext.Format2;
using NewFontParser.Tables.Common.ChainedSequenceContext.Format3;
using NewFontParser.Tables.Common.SequenceContext.Format1;
using NewFontParser.Tables.Common.SequenceContext.Format2;
using NewFontParser.Tables.Common.SequenceContext.Format3;
using NewFontParser.Tables.Gpos.LookupSubtables;
using NewFontParser.Tables.Gpos.LookupSubtables.PairPos;

namespace NewFontParser.Tables.Common
{
    public class GposLookupTable
    {
        public GposLookupType LookupType { get; }

        public LookupFlag LookupFlags { get; }

        public ushort? MarkFilteringSet { get; }

        public List<ILookupSubTable> SubTables { get; } = new List<ILookupSubTable>();

        public GposLookupTable(BigEndianReader reader)
        {
            long position = reader.Position;
            LookupType = (GposLookupType)reader.ReadUShort();
            LookupFlags = (LookupFlag)reader.ReadUShort();
            ushort subTableCount = reader.ReadUShort();
            ushort[] subTableOffsets = reader.ReadUShortArray(subTableCount);
            if (LookupFlags.HasFlag(LookupFlag.UseMarkFilteringSet))
            {
                MarkFilteringSet = reader.ReadUShort();
            }
            for (var i = 0; i < subTableCount; i++)
            {
                reader.Seek(subTableOffsets[i] + position);
                switch (LookupType)
                {
                    case GposLookupType.SingleAdjustment:
                        SubTables.Add(new SinglePos(reader));
                        break;

                    case GposLookupType.PairAdjustment:
                        byte version = reader.PeekBytes(2)[1];
                        switch (version)
                        {
                            case 1:
                                SubTables.Add(new Format1(reader));
                                break;

                            case 2:
                                SubTables.Add(new Format2(reader));
                                break;
                        }
                        break;

                    case GposLookupType.CursiveAttachment:
                        SubTables.Add(new Gpos.LookupSubtables.CursivePos.Format1(reader));
                        break;

                    case GposLookupType.MarkToBaseAttachment:
                        SubTables.Add(new Gpos.LookupSubtables.MarkBasePos.Format1(reader));
                        break;

                    case GposLookupType.MarkToLigatureAttachment:
                        SubTables.Add(new Gpos.LookupSubtables.MarkLigPos.Format1(reader));
                        break;

                    case GposLookupType.MarkToMarkAttachment:
                        SubTables.Add(new Gpos.LookupSubtables.MarkMarkPos.Format1(reader));
                        break;

                    case GposLookupType.ContextPositioning:
                        byte format = reader.PeekBytes(2)[1];
                        switch (format)
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

                    case GposLookupType.ChainedContextPositioning:
                        format = reader.PeekBytes(2)[1];
                        switch (format)
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

                    case GposLookupType.PositioningExtension:
                        SubTables.Add(new PosExtensionFormat1(reader));
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}