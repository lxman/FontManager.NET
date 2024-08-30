using System;

namespace NewFontParser.Reader
{
    public class BigEndianReader
    {
        public long BytesRemaining => _data.Length - Position;

        public long WordsRemaining => (_data.Length / 2) - (Position / 2);

        public long Position { get; private set; }

        private readonly byte[] _data;

        public BigEndianReader(byte[] data)
        {
            _data = data;
        }

        public void Seek(long position)
        {
            Position = position;
        }

        public byte[] ReadBytes(long count)
        {
            var result = new byte[count];
            Array.Copy(_data, Position, result, 0, count);
            Position += count;
            return result;
        }

        public byte[] PeekBytes(int count)
        {
            var result = new byte[count];
            Array.Copy(_data, Position, result, 0, count);
            return result;
        }

        public byte ReadByte()
        {
            return _data[Position++];
        }

        public ushort ReadUShort()
        {
            return ReadUShort16();
        }

        public uint ReadUInt16()
        {
            return (uint)ReadUShort16();
        }

        public short ReadShort()
        {
            return (short)ReadUShort16();
        }

        public int ReadInt16()
        {
            return ReadShort();
        }

        public uint ReadUInt24()
        {
            byte[] data = ReadBytes(3);
            return (uint)((data[0] << 16) | (data[1] << 8) | data[2]);
        }

        public uint ReadUInt32()
        {
            byte[] data = ReadBytes(4);
            return (uint)((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3]);
        }

        public long ReadLong()
        {
            byte[] data = ReadBytes(8);
            long result = 0;
            result |= (long)data[0] << 56;
            result |= (long)data[1] << 48;
            result |= (long)data[2] << 40;
            result |= (long)data[3] << 32;
            result |= (long)data[4] << 24;
            result |= (long)data[5] << 16;
            result |= (long)data[6] << 8;
            result |= data[7];
            return result;
        }

        public int ReadInt32()
        {
            byte[] data = ReadBytes(4);
            return (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3];
        }

        public float ReadF16Dot16()
        {
            byte[] data = ReadBytes(4);
            return (data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3]) / 65536.0f;
        }

        public float ReadF2Dot14()
        {
            byte[] data = ReadBytes(2);
            return (data[0] << 8 | data[1]) / 16384.0f;
        }

        public long ReadLongDateTime()
        {
            byte[] data = ReadBytes(8);
            return (data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3]) * 0x100000000L + (data[4] << 24 | data[5] << 16 | data[6] << 8 | data[7]);
        }

        private ushort ReadUShort16()
        {
            byte[] data = ReadBytes(2);
            return (ushort)((data[0] << 8) | data[1]);
        }

        public ushort[] ReadUShortArray(int count)
        {
            var result = new ushort[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = ReadUShort();
            }
            return result;
        }

        public uint[] ReadUInt32Array(int count)
        {
            var result = new uint[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = ReadUInt32();
            }
            return result;
        }

        public uint ReadOffset(int offSize)
        {
            return offSize switch
            {
                1 => ReadByte(),
                2 => ReadUShort(),
                3 => ReadUInt24(),
                4 => ReadUInt32(),
                _ => 0
            };
        }
    }
}