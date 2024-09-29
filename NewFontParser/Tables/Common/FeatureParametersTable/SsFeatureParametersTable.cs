using NewFontParser.Reader;

namespace NewFontParser.Tables.Common.FeatureParametersTable
{
    public class SsFeatureParametersTable : IFeatureParametersTable
    {
        public ushort Format { get; }
        
        public ushort UILabelNameId { get; }

        public SsFeatureParametersTable(BigEndianReader reader)
        {
            Format = reader.ReadUShort();
            UILabelNameId = reader.ReadUShort();
        }
    }
}