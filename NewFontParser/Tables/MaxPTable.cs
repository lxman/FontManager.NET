using NewFontParser.Reader;

namespace NewFontParser.Tables
{
    public class MaxPTable : IInfoTable
    {
        public ushort Version { get; private set; }

        public ushort NumGlyphs { get; private set; }

        public ushort MaxPoints { get; private set; }

        public ushort MaxContours { get; private set; }

        public ushort MaxCompositePoints { get; private set; }

        public ushort MaxCompositeContours { get; private set; }

        public ushort MaxZones { get; private set; }

        public ushort MaxTwilightPoints { get; private set; }

        public ushort MaxStorage { get; private set; }

        public ushort MaxFunctionDefs { get; private set; }

        public ushort MaxInstructionDefs { get; private set; }

        public ushort MaxStackElements { get; private set; }

        public ushort MaxSizeOfInstructions { get; private set; }

        public ushort MaxComponentElements { get; private set; }

        public ushort MaxComponentDepth { get; private set; }

        public MaxPTable(byte[] data)
        {
            var reader = new BigEndianReader(data);
            Version = reader.ReadUshort();
            NumGlyphs = reader.ReadUshort();
            MaxPoints = reader.ReadUshort();
            MaxContours = reader.ReadUshort();
            MaxCompositePoints = reader.ReadUshort();
            MaxCompositeContours = reader.ReadUshort();
            MaxZones = reader.ReadUshort();
            MaxTwilightPoints = reader.ReadUshort();
            MaxStorage = reader.ReadUshort();
            MaxFunctionDefs = reader.ReadUshort();
            MaxInstructionDefs = reader.ReadUshort();
            MaxStackElements = reader.ReadUshort();
            MaxSizeOfInstructions = reader.ReadUshort();
            MaxComponentElements = reader.ReadUshort();
            MaxComponentDepth = reader.ReadUshort();
        }
    }
}
