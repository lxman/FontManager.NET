using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Jstf
{
    public class JstfScript
    {
        public ExtenderGlyph? ExtenderGlyph { get; }

        public JstfLangSys? DefaultJstfLangSys { get; }

        public List<JstfLangSysRecord> JstfLangSysRecords { get; } = new List<JstfLangSysRecord>();

        public JstfScript(BigEndianReader reader)
        {
            long start = reader.Position;
            ushort extenderGlyphOffset = reader.ReadUShort();
            if (extenderGlyphOffset > 0)
            {
                reader.Seek(start + extenderGlyphOffset);
                ExtenderGlyph = new ExtenderGlyph(reader);
            }
            ushort defaultJstfLangSysOffset = reader.ReadUShort();
            if (defaultJstfLangSysOffset > 0)
            {
                reader.Seek(start + defaultJstfLangSysOffset);
                DefaultJstfLangSys = new JstfLangSys(reader);
            }
            ushort jstfLangSysCount = reader.ReadUShort();
            for (var i = 0; i < jstfLangSysCount; i++)
            {
                JstfLangSysRecords.Add(new JstfLangSysRecord(reader, start));
            }
        }
    }
}