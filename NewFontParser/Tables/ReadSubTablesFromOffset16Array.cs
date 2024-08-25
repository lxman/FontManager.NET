using System;
using NewFontParser.Reader;

namespace NewFontParser.Tables
{
    public class ReadSubTablesFromOffset16Array<T>
    {
        public T[] Tables { get; }

        public ReadSubTablesFromOffset16Array(BigEndianReader reader, ushort[] offsets)
        {
            Tables = new T[offsets.Length];
            for (var i = 0; i < offsets.Length; i++)
            {
                reader.Seek(offsets[i]);
                Tables[i] = (T)Activator.CreateInstance(typeof(T), reader);
            }
        }
    }
}
