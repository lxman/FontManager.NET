using System;
using System.IO;

namespace FontParser.Tables
{
    internal class UnreadTableEntry : TableEntry
    {
        public UnreadTableEntry(TableHeader header)
        {
            Header = header;
        }

        public override string Name => Header.Tag;

        //
        protected override sealed void ReadContentFrom(BinaryReader reader)
        {
            //intend ***
            throw new NotImplementedException();
        }

        public bool HasCustomContentReader { get; protected set; }

        public virtual T CreateTableEntry<T>(BinaryReader reader, T expectedResult)
            where T : TableEntry
        {
            throw new NotImplementedException();
        }

#if DEBUG

        public override string ToString()
        {
            return Name;
        }

#endif
    }
}