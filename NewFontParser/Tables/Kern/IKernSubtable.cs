namespace NewFontParser.Tables.Kern
{
    public interface IKernSubtable
    {
        ushort Version { get; }

        ushort Length { get; }

        ushort Coverage { get; }
    }
}