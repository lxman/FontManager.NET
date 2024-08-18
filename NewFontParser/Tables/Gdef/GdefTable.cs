using System.Buffers.Binary;

namespace NewFontParser.Tables.Gdef
{
    public class GdefTable : IInfoTable
    {
        public readonly GdefHeader Header;

        public readonly IClassDefinition? GlyphClassDef;

        public readonly AttachListTable? AttachList;

        public readonly LigCaretListTable? LigCaretList;

        public readonly IClassDefinition? MarkAttachClassDef;

        public readonly MarkGlyphSetsTable? MarkGlyphSets;

        public GdefTable(byte[] data)
        {
            Header = new GdefHeader(data);
            if (Header.GlyphClassDefOffset.HasValue && Header.GlyphClassDefOffset > 0)
            {
                byte[] gcod = data[(Header.GlyphClassDefOffset ?? 0)..];
                ushort format = BinaryPrimitives.ReadUInt16BigEndian(gcod);
                GlyphClassDef = (format switch
                {
                    1 => new ClassDefinition1(gcod),
                    2 => new ClassDefinition2(gcod),
                    _ => GlyphClassDef
                })!;
            }

            if (Header.AttachListOffset.HasValue && Header.AttachListOffset > 0)
            {
                byte[] alod = data[(Header.AttachListOffset ?? 0)..];
                AttachList = new AttachListTable(alod);
            }

            if (Header.LigCaretListOffset.HasValue && Header.LigCaretListOffset > 0)
            {
                byte[] lclod = data[(Header.LigCaretListOffset ?? 0)..];
                LigCaretList = new LigCaretListTable(lclod);
            }

            if (Header.MarkAttachClassDefOffset.HasValue && Header.MarkAttachClassDefOffset > 0)
            {
                byte[] macd = data[(Header.MarkAttachClassDefOffset ?? 0)..];
                ushort format = BinaryPrimitives.ReadUInt16BigEndian(macd);
                MarkAttachClassDef = (format switch
                {
                    1 => new ClassDefinition1(macd),
                    2 => new ClassDefinition2(macd),
                    _ => MarkAttachClassDef
                })!;
            }

            if (Header.MarkGlyphSetsDefOffset.HasValue && Header.MarkGlyphSetsDefOffset > 0)
            {
                byte[] mgso = data[(Header.MarkGlyphSetsDefOffset ?? 0)..];
                MarkGlyphSets = new MarkGlyphSetsTable(mgso);
            }
        }
    }
}
