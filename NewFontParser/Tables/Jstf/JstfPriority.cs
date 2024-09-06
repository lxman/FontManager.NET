using NewFontParser.Reader;

namespace NewFontParser.Tables.Jstf
{
    public class JstfPriority
    {
        public JstfModList? GsubShrinkageEnable { get; }

        public JstfModList? GsubShrinkageDisable { get; }

        public JstfModList? GposShrinkageEnable { get; }

        public JstfModList? GposShrinkageDisable { get; }

        public JstfMax? ShrinkageJstfMax { get; }

        public JstfModList? GsubExtensionEnable { get; }

        public JstfModList? GsubExtensionDisable { get; }

        public JstfModList? GposExtensionEnable { get; }

        public JstfModList? GposExtensionDisable { get; }

        public JstfMax? ExtensionJstfMax { get; }

        public JstfPriority(BigEndianReader reader)
        {
            long position = reader.Position;

            ushort gsubShrinkageEnableOffset = reader.ReadUShort();
            ushort gsubShrinkageDisableOffset = reader.ReadUShort();
            ushort gposShrinkageEnableOffset = reader.ReadUShort();
            ushort gposShrinkageDisableOffset = reader.ReadUShort();
            ushort shrinkageJstfMaxOffset = reader.ReadUShort();
            ushort gsubExtensionEnableOffset = reader.ReadUShort();
            ushort gsubExtensionDisableOffset = reader.ReadUShort();
            ushort gposExtensionEnableOffset = reader.ReadUShort();
            ushort gposExtensionDisableOffset = reader.ReadUShort();
            ushort extensionJstfMaxOffset = reader.ReadUShort();
            if (gsubShrinkageEnableOffset != 0)
            {
                reader.Seek(position + gsubShrinkageEnableOffset);
                GsubShrinkageEnable = new JstfModList(reader);
            }
            if (gsubShrinkageDisableOffset != 0)
            {
                reader.Seek(position + gsubShrinkageDisableOffset);
                GsubShrinkageDisable = new JstfModList(reader);
            }
            if (gposShrinkageEnableOffset != 0)
            {
                reader.Seek(position + gposShrinkageEnableOffset);
                GposShrinkageEnable = new JstfModList(reader);
            }
            if (gposShrinkageDisableOffset != 0)
            {
                reader.Seek(position + gposShrinkageDisableOffset);
                GposShrinkageDisable = new JstfModList(reader);
            }
            if (shrinkageJstfMaxOffset != 0)
            {
                reader.Seek(position + shrinkageJstfMaxOffset);
                ShrinkageJstfMax = new JstfMax(reader);
            }
            if (gsubExtensionEnableOffset != 0)
            {
                reader.Seek(position + gsubExtensionEnableOffset);
                GsubExtensionEnable = new JstfModList(reader);
            }
            if (gsubExtensionDisableOffset != 0)
            {
                reader.Seek(position + gsubExtensionDisableOffset);
                GsubExtensionDisable = new JstfModList(reader);
            }
            if (gposExtensionEnableOffset != 0)
            {
                reader.Seek(position + gposExtensionEnableOffset);
                GposExtensionEnable = new JstfModList(reader);
            }
            if (gposExtensionDisableOffset != 0)
            {
                reader.Seek(position + gposExtensionDisableOffset);
                GposExtensionDisable = new JstfModList(reader);
            }
            if (extensionJstfMaxOffset == 0) return;
            reader.Seek(position + extensionJstfMaxOffset);
            ExtensionJstfMax = new JstfMax(reader);
        }
    }
}