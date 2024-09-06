using NewFontParser.Reader;

namespace NewFontParser.Tables.Kern
{
    public class KernSubtableFormat2 : IKernSubtable
    {
        public ushort Version { get; }

        public ushort Length { get; }

        public ushort Coverage { get; }

        public KernSubtableFormat2(BigEndianReader reader)
        {
            Version = reader.ReadUShort();
            Length = reader.ReadUShort();
            Coverage = reader.ReadUShort();
            ushort rowWidth = reader.ReadUShort();
            ushort leftClassOffset = reader.ReadUShort();
            ushort rightClassOffset = reader.ReadUShort();
            ushort kerningArrayOffset = reader.ReadUShort();
        }
    }
}