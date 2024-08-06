using System;
using System.IO;
using System.Text;

namespace NewFontParser.Reader
{
    internal class ByteReader
    {
        private readonly byte[] _data;
        private int _position;

        public ByteReader(string file)
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

        public byte ReadByte()
        {
            return _data[_position++];
        }

        public byte[] ReadBytes(int count)
        {
            var result = new byte[count];
            Array.Copy(_data, _position, result, 0, count);
            _position += count;
            return result;
        }

        public ushort ReadUInt16()
        {
            var result = BitConverter.ToUInt16(_data, _position);
            _position += 2;
            return result;
        }

        public uint ReadUInt32()
        {
            var result = BitConverter.ToUInt32(_data, _position);
            _position += 4;
            return result;
        }

        public string ReadString(int count)
        {
            var result = Encoding.ASCII.GetString(_data, _position, count);
            _position += count;
            return result;
        }
    }
}
