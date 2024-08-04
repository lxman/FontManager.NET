using System;

namespace FontParser.Exceptions
{
    public class OpenFontNotSupportedException : Exception
    {
        public OpenFontNotSupportedException()
        { }

        public OpenFontNotSupportedException(string msg) : base(msg)
        {
        }
    }
}