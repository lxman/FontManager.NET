using System;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Woff
{
    public class Woff2TableDirectoryEntry : IDirectoryEntry
    {
        public string Tag { get; }

        public GlyfTransform GlyfTransform { get; }

        public LocaTransform LocaTransform { get; }

        public HmtxTransform HmtxTransform { get; }

        public uint OriginalLength { get; }

        public uint? TransformLength { get; }

        public Woff2TableDirectoryEntry(FileByteReader reader)
        {
            byte flags = reader.ReadBytes(1)[0];
            var tableTag = Convert.ToByte(flags & 0x3F);
            var transformationVersion = Convert.ToByte((flags & 0xC0) >> 6);
            GlyfTransform = (GlyfTransform)transformationVersion;
            LocaTransform = (LocaTransform)transformationVersion;
            HmtxTransform = (HmtxTransform)transformationVersion;
            var newFlag = 0;
            Tag = tableTag <= 62
                ? Woff2KnownTableTags.Values[tableTag]
                : reader.ReadString(4);
            OriginalLength = reader.ReadUintBase128();
            if (Tag == "glyf" || Tag == "loca")
            {
                if (transformationVersion == 0)
                {
                    newFlag |= 0x100;
                }
            }
            else if (transformationVersion != 0)
            {
                newFlag |= 0x100;
            }

            newFlag |= transformationVersion;
            if ((newFlag & 0x100) == 0)
            {
                return;
            }
            TransformLength = reader.ReadUintBase128();
        }
    }
}