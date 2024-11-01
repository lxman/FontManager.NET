using System;

namespace FontParser.RenderFont.Interpreter
{
    public class InstructionStream
    {
        public int Position { get; private set; }

        private readonly byte[] _pgm;

        public InstructionStream(byte[] pgm)
        {
            _pgm = pgm;
        }

        public byte ReadByte()
        {
            return _pgm[Position++];
        }

        public bool EndOfProgram => Position >= _pgm.Length;

        public byte[] ReadBytes(int count)
        {
            var bytes = new byte[count];
            Array.Copy(_pgm, Position, bytes, 0, count);
            Position += count;
            return bytes;
        }

        public short ReadWord()
        {
            var value = (short)(_pgm[Position] << 8 | _pgm[Position + 1]);
            Position += 2;
            return value;
        }

        public short[] ReadWords(int count)
        {
            var words = new short[count];
            for (var i = 0; i < count; i++)
            {
                words[i] = ReadWord();
            }
            return words;
        }

        public void Seek(int position)
        {
            Position = position;
        }
    }
}