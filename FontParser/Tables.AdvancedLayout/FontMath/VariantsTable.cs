namespace FontParser.Tables.AdvancedLayout.FontMath
{
    public class VariantsTable
    {
        public ushort MinConnectorOverlap;
        public CoverageTable.CoverageTable vertCoverage;
        public CoverageTable.CoverageTable horizCoverage;
        public GlyphConstruction[] vertConstructionTables;
        public GlyphConstruction[] horizConstructionTables;
    }
}
