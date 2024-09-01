using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Aat.Feat
{
    public class FeatureName
    {
        public ushort Feature { get; }

        public int NameIndex { get; }

        public List<SettingName> Settings { get; } = new List<SettingName>();

        private readonly ushort _settingCount;
        private readonly uint _settingTableOffset;
        private readonly long _startOffset;

        public FeatureName(BigEndianReader reader)
        {
            _startOffset = reader.Position;

            Feature = reader.ReadUShort();
            _settingCount = reader.ReadUShort();
            _settingTableOffset = reader.ReadUInt32();
            NameIndex = reader.ReadInt32();
        }

        public void ReadSettings(BigEndianReader reader)
        {
            reader.Seek(_startOffset + _settingTableOffset);
            for (var i = 0; i < _settingCount; i++)
            {
                Settings.Add(new SettingName(reader));
            }
        }
    }
}
