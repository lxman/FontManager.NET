using System.Collections.Generic;
using NewFontParser.Reader;

namespace NewFontParser.Tables.Jstf
{
    public class JstfLangSys
    {
        public List<JstfPriority> JstfPriorities { get; } = new List<JstfPriority>();

        public JstfLangSys(BigEndianReader reader)
        {
            var jstfPriorityCount = reader.ReadUShort();
            for (var i = 0; i < jstfPriorityCount; i++)
            {
                JstfPriorities.Add(new JstfPriority(reader));
            }
        }
    }
}