using System;

namespace NewFontParser.RenderFont.Interpreter
{
    public class InstructionStream
    {
        private readonly byte[] _pgm;

        private int _position;

        public InstructionStream(byte[] pgm)
        {
            _pgm = pgm;
        }

        public byte ReadByte()
        {
            return _pgm[_position++];
        }

        public byte[] ReadBytes(int count)
        {
            var bytes = new byte[count];
            Array.Copy(_pgm, _position, bytes, 0, count);
            _position += count;
            return bytes;
        }

        public byte[] PeekBytes(int count)
        {
            var bytes = new byte[count];
            Array.Copy(_pgm, _position, bytes, 0, count);
            return bytes;
        }

        public short ReadWord()
        {
            var value = (short)(_pgm[_position] << 8 | _pgm[_position + 1]);
            _position += 2;
            return value;
        }

        public ushort ReadUShort()
        {
            var value = (ushort)(_pgm[_position] << 8 | _pgm[_position + 1]);
            _position += 2;
            return value;
        }

        public void Seek(int position)
        {
            _position = position;
        }
    }
}
