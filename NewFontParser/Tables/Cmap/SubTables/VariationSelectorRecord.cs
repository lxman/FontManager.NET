using System;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Cmap.SubTables
{
    public class VariationSelectorRecord
    {
        public uint VarSelector { get; }

        public uint DefaultUvsOffset { get; }

        public uint NonDefaultUvsOffset { get; }

        public DefaultUvsTableHeader? DefaultUvsTableHeader { get; }

        public NonDefaultUvsTableHeader? NonDefaultUvsTableHeader { get; }

        public VariationSelectorRecord(BigEndianReader reader)
        {
            VarSelector = reader.ReadUint24();
            DefaultUvsOffset = reader.ReadUint32();
            NonDefaultUvsOffset = reader.ReadUint32();
            if (DefaultUvsOffset > 0)
            {
                reader.Seek(Convert.ToUInt32(DefaultUvsOffset));
                DefaultUvsTableHeader = new DefaultUvsTableHeader(reader);
            }

            if (NonDefaultUvsOffset <= 0) return;
            reader.Seek(Convert.ToUInt32(NonDefaultUvsOffset));
            NonDefaultUvsTableHeader = new NonDefaultUvsTableHeader(reader);
        }
    }
}