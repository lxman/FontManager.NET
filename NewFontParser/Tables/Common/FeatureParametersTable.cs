using System;
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
            try
            {
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
                    if (reader.BytesRemaining < 3)
                    {
                        Console.WriteLine("Ran out of data reading USVs");
                        UnicodeScalarValues[i] = 0;
                        return;
                    }
                    UnicodeScalarValues[i] = reader.ReadUInt24();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed parsing FeatureParametersTable");
            }
        }
    }
}
