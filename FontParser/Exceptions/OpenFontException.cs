using System;

namespace FontParser.Exceptions
{
    public class OpenFontException : Exception
    {
        public OpenFontException()
        { }

        public OpenFontException(string msg) : base(msg)
        {
        }
    }
}