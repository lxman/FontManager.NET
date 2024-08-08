using System;

public enum FileType
{
    UNK,
    TTF,
    OTF,
    TTC,
    OTC
}

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
    Short,
    Long
}