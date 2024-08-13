using System;

// ReSharper disable CheckNamespace

#region Enums

public enum FileType
{
    Unk,
    Ttf,
    Otf,
    Ttc,
    Otc
}

public enum FontDirectionHint : short
{
    FullyMixed = 0,
    OnlyStrongLtr = 1,
    StrongLtrAndNeutral = 2,
    OnlyStrongRtl = -1,
    StrongRtlAndNeutral = -2
}

public enum IndexToLocFormat
{
    Offset16,
    Offset32
}

public enum InstructionSet
{
    TrueType,
    Cff
}

public enum InstructionType
{
    ByteCode,
    Function
}

public enum MacintoshEncodingId : ushort
{
    Roman = 0,
    Japanese = 1,
    ChineseTraditional = 2,
    Korean = 3,
    Arabic = 4,
    Hebrew = 5,
    Greek = 6,
    Russian = 7,
    RSymbol = 8,
    Devanagari = 9,
    Gurmukhi = 10,
    Gujarati = 11,
    Oriya = 12,
    Bengali = 13,
    Tamil = 14,
    Telugu = 15,
    Kannada = 16,
    Malayalam = 17,
    Sinhalese = 18,
    Burmese = 19,
    Khmer = 20,
    Thai = 21,
    Laotian = 22,
    Georgian = 23,
    Armenian = 24,
    ChineseSimplified = 25,
    Tibetan = 26,
    Mongolian = 27,
    Geez = 28,
    Slavic = 29,
    Vietnamese = 30,
    Sindhi = 31,
    Uninterpreted = 32
}

public enum PlatformId : ushort
{
    Unicode = 0,
    Macintosh = 1,
    Iso = 2,
    Windows = 3,
    Custom = 4
}

public enum Platform0EncodingId : ushort
{
    Unicode1 = 0,
    Unicode11 = 1,
    Iso10646 = 2,
    Unicode20 = 3,
    Unicode21 = 4,
    Unicode22 = 5,
    Unicode30 = 6
}

public enum Platform1EncodingId : ushort
{
    Roman = 0,
    Japanese = 1,
    ChineseTraditional = 2,
    Korean = 3,
    Arabic = 4,
    Hebrew = 5,
    Greek = 6,
    Russian = 7,
    RSymbol = 8,
    Devanagari = 9,
    Gurmukhi = 10,
    Gujarati = 11,
    Oriya = 12,
    Bengali = 13,
    Tamil = 14,
    Telugu = 15,
    Kannada = 16,
    Malayalam = 17,
    Sinhalese = 18,
    Burmese = 19,
    Khmer = 20,
    Thai = 21,
    Laotian = 22,
    Georgian = 23,
    Armenian = 24,
    ChineseSimplified = 25,
    Tibetan = 26,
    Mongolian = 27,
    Geez = 28,
    Slavic = 29,
    Vietnamese = 30,
    Sindhi = 31,
    Uninterpreted = 32
}

public enum Platform2EncodingId : ushort
{
    Ascii7Bit = 0,
    Iso10646 = 1,
    Iso8859_1 = 2
}

public enum Platform3EncodingId : ushort
{
    UnicodeCsm = 0,
    UnicodeBmp = 1,
    ShiftJis = 2,
    Prc = 3,
    Big5 = 4,
    Wansung = 5,
    Johab = 6,
    UnicodeUCS4 = 10
}

#endregion

#region Flags

[Flags]
public enum HeadFlags : ushort
{
    BaselineAtY0 = 1 << 0,
    LeftSidebearingAtX0 = 1 << 1,
    InstructionsDependOnPointSize = 1 << 2,
    ForcePpemToInteger = 1 << 3,
    InstructionsAlterAdvanceWidth = 1 << 4,
    UseIntegerScaling = 1 << 5,
    InstructionsAlterAdvanceHeight = 1 << 6,
    UseLinearMetrics = 1 << 7,
    UsePpm = 1 << 8,
    UseIntegerPpm = 1 << 9,
    UsePPem = 1 << 10,
    UseIntegerPPem = 1 << 11,
    UseDoubleShift = 1 << 12,
    UseFullHinting = 1 << 13,
    UseGridfit = 1 << 14,
    UseBitmaps = 1 << 15
}

[Flags]
public enum MacStyle : ushort
{
    Bold = 1 << 0,
    Italic = 1 << 1,
    Underline = 1 << 2,
    Outline = 1 << 3,
    Shadow = 1 << 4,
    Condensed = 1 << 5,
    Extended = 1 << 6
}

[Flags]
public enum SimpleGlyphFlags : byte
{
    OnCurve = 1 << 0,
    XShortVector = 1 << 1,
    YShortVector = 1 << 2,
    Repeat = 1 << 3,
    XIsSameOrPositiveXShortVector = 1 << 4,
    YIsSameOrPositiveYShortVector = 1 << 5,
    OverlapSimple = 1 << 6
}

[Flags]
public enum CompositeGlyphFlags : ushort
{
    Arg1And2AreWords = 1 << 0,
    ArgsAreXyValues = 1 << 1,
    RoundXyToGrid = 1 << 2,
    WeHaveAScale = 1 << 3,
    MoreComponents = 1 << 5,
    WeHaveAnXAndYScale = 1 << 6,
    WeHaveATwoByTwo = 1 << 7,
    WeHaveInstructions = 1 << 8,
    UseMyMetrics = 1 << 9,
    OverlapCompound = 1 << 10,
    ScaledComponentOffset = 1 << 11,
    UnscaledComponentOffset = 1 << 12
}

[Flags]
public enum RangeGaspBehavior : ushort
{
    Gridfit = 1 << 0,
    DoGray = 1 << 1,
    SymmetricSmoothing = 1 << 2,
    SymmetricGridfit = 1 << 3
}

[Flags]
public enum MergeEntryFlags : byte
{
    Merge = 1 << 0,
    UseMyMetrics = 1 << 1,
    OverlapCompound = 1 << 2,
    ScaledComponentOffset = 1 << 3,
    UnscaledComponentOffset = 1 << 4
}

#endregion