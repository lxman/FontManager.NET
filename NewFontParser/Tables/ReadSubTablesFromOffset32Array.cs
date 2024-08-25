using System;
using NewFontParser.Reader;

namespace NewFontParser.Tables
{
    public class ReadSubTablesFromOffset32Array<T>
    {
        public T[] Tables { get; }

        public ReadSubTablesFromOffset32Array(BigEndianReader reader, uint[] offsets)
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
