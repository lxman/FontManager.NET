using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Serilog;

namespace NewFontParser.Reader
{
    public class BigEndianReader
    {
        public long BytesRemaining => _data.Length - Position;

        public long WordsRemaining => (_data.Length / 2) - (Position / 2);

        public long Position { get; private set; }

        public bool LogChanges { get; set; }

        private readonly byte[] _data;

        public BigEndianReader(byte[] data)
        {
            _data = data;
        }

        public void Seek(long position)
        {
            if (LogChanges) Log.Debug($"Position moved - now {position}");
            Position = position;
        }

        public byte[] ReadBytes(
            long count,
            [CallerMemberName] string member = "",
            [CallerFilePath] string path = "",
            [CallerLineNumber] int line = -1)
        {
            if (Position + count > _data.Length)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Source array was not long enough by {Position + count - _data.Length} bytes.");
                sb.AppendLine($"Called from {path}");
                sb.AppendLine(member);
                sb.AppendLine($"Line #{line}");
                throw new ArgumentException(sb.ToString(), nameof(_data));
            }
            var result = new byte[count];
            Array.Copy(_data, Position, result, 0, count);
            Position += count;
            if (LogChanges) Log.Debug($"Bytes read - position is now {Position}");
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
            if (LogChanges) Log.Debug($"Byte read - position is now {Position + 1}");
            return _data[Position++];
        }

        public sbyte ReadSByte()
        {
            return (sbyte)ReadByte();
        }

        public sbyte[] ReadSbytes(int count)
        {
            var result = new sbyte[count];
            Array.Copy(_data, Position, result, 0, count);
            Position += count;
            return result;
        }

        public ushort ReadUShort()
        {
            return ReadUShort16();
        }

        public short ReadShort()
        {
            return BinaryPrimitives.ReadInt16BigEndian(ReadBytes(2));
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
            return BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(4));
        }

        public long ReadLong()
        {
            return BinaryPrimitives.ReadInt64BigEndian(ReadBytes(8));
        }

        public int ReadInt32()
        {
            byte[] data = ReadBytes(4);
            return BinaryPrimitives.ReadInt32BigEndian(data);
        }

        public float ReadF16Dot16()
        {
            byte[] data = ReadBytes(4);
            return BinaryPrimitives.ReadInt32BigEndian(data) / 65536.0f;
        }

        public float ReadF2Dot14()
        {
            byte[] data = ReadBytes(2);
            return BinaryPrimitives.ReadInt16BigEndian(data) / 16384.0f;
        }

        public long ReadLongDateTime()
        {
            return ReadLong();
        }

        private ushort ReadUShort16()
        {
            return BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));
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

        public string ReadNullTerminatedString(bool isUnicode)
        {
            var data = new List<byte>();
            if (isUnicode)
            {
                while (PeekBytes(2) != new byte[] { 0, 0 })
                {
                    data.Add(ReadByte());
                    data.Add(ReadByte());
                }
                _ = ReadBytes(2);

                return Encoding.Unicode.GetString(data.ToArray());
            }
            while (PeekBytes(1)[0] != 0)
            {
                data.Add(ReadByte());
            }

            _ = ReadByte();

            return Encoding.ASCII.GetString(data.ToArray());
        }
    }
}