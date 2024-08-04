using System.IO;

namespace FontParser.Tables.CFF.CFF
{
    internal class Cff2Parser
    {
        //https://docs.microsoft.com/en-us/typography/opentype/spec/cff2
        //Table 1: CFF2 Data Layout
        //Entry         Comments
        //Header        Fixed location
        //Top DICT      Fixed location
        //Global Subr   INDEX Fixed location
        //VariationStore
        //FDSelect Present only if there is more than one Font DICT in the Font DICT INDEX.
        //Font DICT INDEX
        //Array of Font DICT  Included in Font DICT INDEX.
        //Private DICT    One per Font DICT.
        public void ParseAfterHeader(BinaryReader reader)
        {
        }
    }
}
