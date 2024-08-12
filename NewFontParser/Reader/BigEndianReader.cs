using System;

namespace NewFontParser.Reader
{
    public class BigEndianReader
    {
        private readonly byte[] _data;
        private long _position;

        public BigEndianReader(byte[] data)
        {
            _data = data;
        }

        public void Seek(int position)
        {
            _position = position;
        }

        public byte[] ReadBytes(long count)
        {
            var result = new byte[count];
            Array.Copy(_data, _position, result, 0, count);
            _position += count;
            return result;
        }

        public byte[] PeekBytes(int count)
        {
            var result = new byte[count];
            Array.Copy(_data, _position, result, 0, count);
            return result;
        }

        public byte ReadByte()
        {
            return _data[_position++];
        }

        public ushort ReadUShort()
        {
            byte[] data = ReadBytes(2);
            return (ushort)((data[0] << 8) | data[1]);
        }

        public uint ReadUint16()
        {
            byte[] data = ReadBytes(2);
            return (uint)((data[0] << 8) | data[1]);
        }

        public short ReadShort()
        {
            byte[] data = ReadBytes(2);
            return (short)((data[0] << 8) | data[1]);
        }

        public int ReadInt16()
        {
            byte[] data = ReadBytes(2);
            return (short)((data[0] << 8) | data[1]);
        }

        public uint ReadUint24()
        {
            byte[] data = ReadBytes(3);
            return (uint)((data[0] << 16) | (data[1] << 8) | data[2]);
        }

        public uint ReadUint32()
        {
            byte[] data = ReadBytes(4);
            return (uint)((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3]);
        }

        public int ReadInt32()
        {
            byte[] data = ReadBytes(4);
            return (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3];
        }

        public float ReadFixed()
        {
            byte[] data = ReadBytes(4);
            return (data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3]) / 65536.0f;
        }

        public long ReadLongDateTime()
        {
            byte[] data = ReadBytes(8);
            return (data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3]) * 0x100000000L + (data[4] << 24 | data[5] << 16 | data[6] << 8 | data[7]);
        }
    }
}