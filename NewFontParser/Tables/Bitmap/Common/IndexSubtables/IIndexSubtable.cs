namespace NewFontParser.Tables.Bitmap.Common.IndexSubtables
{
    public interface IIndexSubtable
    {
        ushort IndexFormat { get; }

        ushort ImageFormat { get; }

        uint ImageDataOffset { get; }
    }
}