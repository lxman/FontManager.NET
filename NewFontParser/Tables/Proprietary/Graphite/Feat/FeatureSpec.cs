﻿using System.Text;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Proprietary.Graphite.Feat
{
    public class FeatureSpec
    {
        public string FeatureName { get; }

        public ushort SettingCount { get; }

        public FeatureSpec(BigEndianReader reader)
        {
            FeatureName = Encoding.ASCII.GetString(reader.ReadBytes(4));
            SettingCount = reader.ReadUShort();
            _ = reader.ReadUShort(); // Reserved
            uint settingOffset = reader.ReadUInt32();
            ushort flags = reader.ReadUShort();
            ushort nameIndex = reader.ReadUShort();
        }
    }
}