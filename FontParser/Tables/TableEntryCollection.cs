using System.Collections.Generic;

namespace FontParser.Tables
{
    public class TableEntryCollection
    {
        private readonly Dictionary<string, TableEntry> _tables = new Dictionary<string, TableEntry>();

        public TableEntryCollection()
        { }

        public void AddEntry(TableEntry en) => _tables.Add(en.Name, en);

        public bool TryGetTable(string tableName, out TableEntry entry) => _tables.TryGetValue(tableName, out entry);

        public void ReplaceTable(TableEntry table) => _tables[table.Name] = table;

        public TableHeader[] CloneTableHeaders()
        {
            TableHeader[] clones = new TableHeader[_tables.Count];
            var i = 0;
            foreach (TableEntry en in _tables.Values)
            {
                clones[i] = en.Header.Clone();
                i++;
            }
            return clones;
        }
    }
}