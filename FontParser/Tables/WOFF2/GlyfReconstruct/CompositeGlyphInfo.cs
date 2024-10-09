using System.Collections.Generic;

namespace FontParser.Tables.WOFF2.GlyfReconstruct
{
    public class CompositeGlyphInfo : IGlyphInfo
    {
        public List<CompositeGlyphElement> Elements { get; } = new List<CompositeGlyphElement>();

        public ushort InstructionCount { get; set; }

        public List<byte> Instructions { get; } = new List<byte>();
    }
}
