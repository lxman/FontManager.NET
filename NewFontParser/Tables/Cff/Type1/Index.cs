using System.Collections.Generic;
using NewFontParser.Reader;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NewFontParser.Tables.Cff.Type1
{
    public class Index
    {
        public List<List<byte>> Data { get; } = new List<List<byte>>();

        public Index(BigEndianReader reader)
        {
            ushort count = reader.ReadUShort();
            if (count == 0) return;

            byte offSize = reader.ReadByte();
            var offsets = new uint[count + 1];
            for (var i = 0; i < count + 1; i++)
            {
                offsets[i] = reader.ReadOffset(offSize);
            }

            for (var i = 0; i < count; i++)
            {
                uint length = offsets[i + 1] - offsets[i];
                Data.Add(new List<byte>(reader.ReadBytes((int)length)));
            }
        }
    }
}