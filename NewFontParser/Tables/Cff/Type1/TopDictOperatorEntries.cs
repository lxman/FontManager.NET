using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NewFontParser.Tables.Cff.Type1
{
    public class TopDictOperatorEntries : ReadOnlyDictionary<ushort, CffDictEntry?>
    {
        public TopDictOperatorEntries(IDictionary<ushort, CffDictEntry?> dictionary) : base(dictionary)
        {
        }
    }
}
