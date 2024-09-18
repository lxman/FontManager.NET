using System;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Woff
{
    public class Woff2TableDirectoryEntry : IDirectoryEntry
    {
        public byte Flags { get; }

        public GlyfTransform GlyfTransform { get; }

        public LocaTransform LocaTransform { get; }

        public HmtxTransform HmtxTransform { get; }

        public string Tag { get; }

        public uint OriginalLength { get; }

        public uint TransformLength { get; }

        public Woff2TableDirectoryEntry(FileByteReader reader)
        {
            Flags = reader.ReadBytes(1)[0];
            var tableTag = Convert.ToByte(Flags & 0x3F);
            var transformationVersion = Convert.ToByte((Flags & 0xC0) >> 6);
            GlyfTransform = (GlyfTransform)transformationVersion;
            LocaTransform = (LocaTransform)transformationVersion;
            HmtxTransform = (HmtxTransform)transformationVersion;
            Tag = tableTag <= 62
                ? Woff2KnownTableTags.Values[tableTag]
                : reader.ReadString(4);
            OriginalLength = reader.ReadUintBase128();
            TransformLength = reader.ReadUintBase128();
        }
    }
}
