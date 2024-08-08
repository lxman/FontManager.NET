namespace NewFontParser.Tables.Cmap.SubTables
{
    public interface ICmapSubtable
    {
        uint Format { get; }

        uint Length { get; }

        int Language { get; }
    }
}