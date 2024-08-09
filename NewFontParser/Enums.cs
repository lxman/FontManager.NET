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
    Short,
    Long
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
    YIsSameOrPositiveYShortVector = 1 << 5
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

#endregion