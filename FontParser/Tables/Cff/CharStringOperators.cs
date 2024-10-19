using System.Collections.Generic;
// ReSharper disable StringLiteralTypo

namespace FontParser.Tables.Cff
{
    public static class CharStringOperators
    {
        public static readonly List<CharStringCodeDefinition> Definitions = new List<CharStringCodeDefinition>
        {
            new CharStringCodeDefinition { Code = 0x0001, Name = "hstem", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0003, Name = "vstem", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0004, Name = "vmoveto", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0005, Name = "rlineto", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0006, Name = "hlineto", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0007, Name = "vlineto", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0008, Name = "rrcurveto", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x000A, Name = "callsubr", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x000B, Name = "return", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x000E, Name = "endchar", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x000F, Name = "vsindex", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0010, Name = "blend", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0012, Name = "hstemhm", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0013, Name = "hintmask", ArgLength = -1 },
            new CharStringCodeDefinition { Code = 0x0014, Name = "cntrmask", ArgLength = -1 },
            new CharStringCodeDefinition { Code = 0x0015, Name = "rmoveto", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0016, Name = "hmoveto", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0017, Name = "vstemhm", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0018, Name = "rcurveline", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0019, Name = "rlinecurve", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x001A, Name = "vvcurveto", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x001B, Name = "hhcurveto", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x001C, Name = "shortint", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x001D, Name = "callgsubr", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x001E, Name = "vhcurveto", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x001F, Name = "hvcurveto", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0C03, Name = "and", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x0C04, Name = "or", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x0C05, Name = "not", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0C09, Name = "abs", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0C0A, Name = "add", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x0C0B, Name = "sub", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x0C0C, Name = "div", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x0C0E, Name = "neq", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0C0F, Name = "eq", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x0C12, Name = "drop", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0C14, Name = "put", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x0C15, Name = "get", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0C16, Name = "ifelse", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x0C17, Name = "random", ArgLength = 0 },
            new CharStringCodeDefinition { Code = 0x0C18, Name = "mul", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x0C1A, Name = "sqrt", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0C1B, Name = "dup", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0C1C, Name = "exch", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x0C1D, Name = "index", ArgLength = 1 },
            new CharStringCodeDefinition { Code = 0x0C1E, Name = "roll", ArgLength = 2 },
            new CharStringCodeDefinition { Code = 0x0C22, Name = "hflex", ArgLength = 6 },
            new CharStringCodeDefinition { Code = 0x0C23, Name = "flex", ArgLength = 12 },
            new CharStringCodeDefinition { Code = 0x0C24, Name = "hflex1", ArgLength = 9 },
            new CharStringCodeDefinition { Code = 0x0C25, Name = "flex1", ArgLength = 11 }
        };
    }
}
