using System.Text;

namespace FontParser.Tables.CFF.CFF
{
    public class Cff1GlyphData
    {
        internal Cff1GlyphData()
        {
        }

        public string Name { get; internal set; }
        public ushort SIDName { get; internal set; }
        internal Type2Instruction[] GlyphInstructions { get; set; }

#if DEBUG
        public ushort dbugGlyphIndex { get; internal set; }

        public override string ToString()
        {
            StringBuilder stbuilder = new StringBuilder();
            stbuilder.Append(dbugGlyphIndex);
            if (Name != null)
            {
                stbuilder.Append(" ");
                stbuilder.Append(Name);
            }
            return stbuilder.ToString();
        }

#endif
    }
}
