using System;
using System.IO;

namespace NewFontParser.Reader
{
    internal class FileByteReader
    {
        public uint BytesRemaining => (uint)_data.Length - _position;

        private readonly byte[] _data;
        private uint _position;

        public FileByteReader(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("File not found", file);
            }
            _data = File.ReadAllBytes(file);
        }

        public void Seek(uint position)
        {
            _position = position;
        }

        public byte[] ReadBytes(uint count)
        {
            var result = new byte[count];
            Array.Copy(_data, _position, result, 0, count);
            _position += count;
            return result;
        }
    }
}