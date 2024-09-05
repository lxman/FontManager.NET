using NewFontParser.Reader;

namespace NewFontParser.Tables.Math
{
    public class MathTable : IInfoTable
    {
        public static string Tag => "MATH";

        public MathHeader Header { get; }

        public MathConstantsTable Constants { get; }

        public MathGlyphInfoTable GlyphInfo { get; }

        public MathVariantsTable Variants { get; }

        public MathTable(byte[] data)
        {
            var reader = new BigEndianReader(data);

            Header = new MathHeader(reader);

            reader.Seek(Header.MathConstantsOffset);
            Constants = new MathConstantsTable(reader);

            reader.Seek(Header.MathGlyphInfoOffset);
            GlyphInfo = new MathGlyphInfoTable(reader);

            reader.Seek(Header.MathVariantsOffset);
            Variants = new MathVariantsTable(reader);
        }
    }
}