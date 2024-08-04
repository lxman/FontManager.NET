using System.IO;
using FontParser.Exceptions;
using FontParser.Tables;

namespace FontParser
{
    internal readonly struct EntriesReaderHelper
    {
        //a simple helper class
        private readonly TableEntryCollection _tables;

        private readonly BinaryReader _input;

        public EntriesReaderHelper(TableEntryCollection tables, BinaryReader input)
        {
            _tables = tables;
            _input = input;
        }

        /// <summary>
        /// read table if exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="tables"></param>
        /// <param name="reader"></param>
        /// <param name="newTableDel"></param>
        /// <returns></returns>
        public T Read<T>(T resultTable) where T : TableEntry
        {
            if (_tables.TryGetTable(resultTable.Name, out TableEntry found))
            {
                //found table name
                //check if we have read this table or not
                if (found is UnreadTableEntry unreadTableEntry)
                {
                    //set header before actal read
                    resultTable.Header = found.Header;
                    if (unreadTableEntry.HasCustomContentReader)
                    {
                        resultTable = unreadTableEntry.CreateTableEntry(_input, resultTable);
                    }
                    else
                    {
                        resultTable.LoadDataFrom(_input);
                    }
                    //then replace
                    _tables.ReplaceTable(resultTable);
                    return resultTable;
                }
                else
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("this table is already loaded");
                    if (!(found is T entry))
                    {
                        throw new OpenFontNotSupportedException();
                    }
#endif
                    return entry;
                }
            }
            //not found
            return null;
        }
    }
}