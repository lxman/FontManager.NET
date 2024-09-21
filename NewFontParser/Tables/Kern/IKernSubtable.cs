namespace NewFontParser.Tables.Kern
{
    public interface IKernSubtable
    {
        ushort Version { get; }

        KernCoverage Coverage { get; }
    }
}