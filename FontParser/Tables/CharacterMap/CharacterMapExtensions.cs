using System.Collections.Generic;

namespace FontParser.Tables.CharacterMap
{
    public static class CharacterMapExtension
    {
        public static void CollectUnicodeChars(this CharacterMap cmap, List<uint> unicodes, List<ushort> glyphIndexList)
        {
            //temp fixed
            int count1 = unicodes.Count;
            cmap.CollectUnicodeChars(unicodes);
            int count2 = unicodes.Count;
            for (int i = count1; i < count2; ++i)
            {
                glyphIndexList.Add(cmap.GetGlyphIndex((int)unicodes[i]));
            }
        }
    }
}