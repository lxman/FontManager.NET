using System;

namespace FontParser.TrueTypeInterpreter
{
    internal class InvalidFontException : Exception
    {
        public InvalidFontException()
        { }

        public InvalidFontException(string msg) : base(msg)
        {
        }
    }

    internal class InvalidTrueTypeFontException : InvalidFontException
    {
        public InvalidTrueTypeFontException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTrueTypeFontException"/> class.
        /// </summary>
        public InvalidTrueTypeFontException(string msg) : base(msg) { }
    }
}