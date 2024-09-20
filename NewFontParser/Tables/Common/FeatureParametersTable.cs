using NewFontParser.Reader;

namespace NewFontParser.Tables.Common
{
    public class FeatureParametersTable
    {
        public ushort Format { get; }

        public ushort FeatureUILabelNameId { get; }

        public ushort FeatureUITooltipTextNameId { get; }

        public ushort SampleTextNameId { get; }

        public ushort NumNamedParameters { get; }

        public ushort FirstParamUILabelNameId { get; }

        public uint[] UnicodeScalarValues { get; }

        public FeatureParametersTable(BigEndianReader reader)
        {
            // TODO: Come back and fix this
            return;
            Format = reader.ReadUShort();
            FeatureUILabelNameId = reader.ReadUShort();
            FeatureUITooltipTextNameId = reader.ReadUShort();
            SampleTextNameId = reader.ReadUShort();
            NumNamedParameters = reader.ReadUShort();
            FirstParamUILabelNameId = reader.ReadUShort();
            ushort charCount = reader.ReadUShort();
            UnicodeScalarValues = new uint[charCount];
            for (var i = 0; i < charCount; i++)
            {
                // TODO: Come back and fix this
                if (reader.BytesRemaining < 3)
                {
                    UnicodeScalarValues[i] = 0;
                    continue;
                }
                UnicodeScalarValues[i] = reader.ReadUInt24();
            }
        }
    }
}
