using System;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Woff
{
    public class Woff2TableDirectoryEntry : IDirectoryEntry
    {
        public byte Flags { get; }

        public byte TransformationVersion { get; }

        public string Tag { get; }

        public uint OriginalLength { get; }

        public uint TransformLength { get; }

        public Woff2TableDirectoryEntry(FileByteReader reader)
        {
            Flags = reader.ReadBytes(1)[0];
            var tableTag = Convert.ToByte(Flags & 0x3F);
            TransformationVersion = Convert.ToByte((Flags & 0xC0) >> 6);
            Tag = tableTag <= 62
                ? Woff2KnownTableTags.Values[tableTag]
                : reader.ReadString(4);
            OriginalLength = reader.ReadUintBase128();
            TransformLength = reader.ReadUintBase128();
        }
    }
}
