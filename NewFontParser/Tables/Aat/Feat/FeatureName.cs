using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Aat.Feat
{
    public class FeatureName
    {
        public ushort Feature { get; }

        public short NameIndex { get; }

        public List<SettingName> Settings { get; } = new List<SettingName>();

        private readonly ushort _settingCount;
        private readonly uint _settingTableOffset;

        public FeatureName(BigEndianReader reader)
        {
            Feature = reader.ReadUShort();
            _settingCount = reader.ReadUShort();
            _settingTableOffset = reader.ReadUInt32();
            ushort featureFlags = reader.ReadUShort();
            NameIndex = reader.ReadShort();
        }

        public void ReadSettings(BigEndianReader reader)
        {
            reader.Seek(_settingTableOffset);
            for (var i = 0; i < _settingCount; i++)
            {
                Settings.Add(new SettingName(reader));
            }
        }
    }
}