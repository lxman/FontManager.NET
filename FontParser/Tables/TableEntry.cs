using System.IO;

namespace FontParser.Tables
{
    /// <summary>
    /// this is base class of all 'top' font table
    /// </summary>
    public abstract class TableEntry
    {
        internal TableHeader Header { get; set; }

        protected abstract void ReadContentFrom(BinaryReader reader);

        public abstract string Name { get; }

        internal void LoadDataFrom(BinaryReader reader)
        {
            //ensure that we always start at the correct offset***
            reader.BaseStream.Seek(Header.Offset, SeekOrigin.Begin);
            ReadContentFrom(reader);
        }

        public uint TableLength => Header.Length;
    }
}