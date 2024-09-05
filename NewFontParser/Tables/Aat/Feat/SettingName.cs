using NewFontParser.Reader;

namespace NewFontParser.Tables.Aat.Feat
{
    public class SettingName
    {
        public ushort Setting { get; }

        public short NameIndex { get; }

        public SettingName(BigEndianReader reader)
        {
            Setting = reader.ReadUShort();
            NameIndex = reader.ReadShort();
        }
    }
}