using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FontParser.Tables.Cff.Type1
{
    public class PrivateDictOperatorEntries : ReadOnlyDictionary<ushort, CffDictEntry?>
    {
        public PrivateDictOperatorEntries(IDictionary<ushort, CffDictEntry?> dictionary) : base(dictionary)
        {
        }
    }
}
