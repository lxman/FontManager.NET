namespace FontParser.Typeface.Os2
{
    public readonly struct Os2FsSelection
    {
        //Bit # 	macStyle bit 	C definition 	Description
        //0         bit 1           ITALIC          Font contains italic or oblique characters, otherwise they are upright.
        //1                         UNDERSCORE      Characters are underscored.
        //2                         NEGATIVE        Characters have their foreground and background reversed.
        //3                         OUTLINED        Outline(hollow) characters, otherwise they are solid.
        //4                         STRIKEOUT       Characters are overstruck.
        //5         bit 0           BOLD            Characters are emboldened.
        //6                         REGULAR         Characters are in the standard weight / style for the font.
        //7                         USE_TYPO_METRICS    If set, it is strongly recommended to use OS / 2.sTypoAscender - OS / 2.sTypoDescender + OS / 2.sTypoLineGap as a value for default line spacing for this font.
        //8                         WWS             The font has ‘name’ table strings consistent with a weight / width / slope family without requiring use of ‘name’ IDs 21 and 22. (Please see more detailed description below.)
        //9                         OBLIQUE         Font contains oblique characters.
        private readonly ushort _fsSelection;

        public Os2FsSelection(ushort fsSelection)
        {
            _fsSelection = fsSelection;
        }

        public bool IsItalic => (_fsSelection & 0x1) != 0;
        public bool IsUnderScore => ((_fsSelection >> 1) & 0x1) != 0;
        public bool IsNegative => ((_fsSelection >> 2) & 0x1) != 0;
        public bool IsOutline => ((_fsSelection >> 3) & 0x1) != 0;
        public bool IsStrikeOut => ((_fsSelection >> 4) & 0x1) != 0;
        public bool IsBold => ((_fsSelection >> 5) & 0x1) != 0;
        public bool IsRegular => ((_fsSelection >> 6) & 0x1) != 0;
        public bool USE_TYPO_METRICS => ((_fsSelection >> 7) & 0x1) != 0;
        public bool WWS => ((_fsSelection >> 8) & 0x1) != 0;
        public bool IsOblique => ((_fsSelection >> 9) & 0x1) != 0;
    }
}