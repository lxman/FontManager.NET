using NewFontParser.Tables.Common.CoverageFormat;

namespace NewFontParser.Tables.Gpos
{
    public interface ISinglePosFormatTable
    {
        ushort PosFormat { get; }

        ICoverageFormat Coverage { get; }

        ValueFormat ValueFormat { get; }
    }
}