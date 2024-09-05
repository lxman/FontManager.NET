namespace NewFontParser.Tables.Gpos
{
    public interface ISinglePosFormatTable
    {
        ushort PosFormat { get; }

        ushort CoverageOffset { get; }

        ValueFormat ValueFormat { get; }
    }
}