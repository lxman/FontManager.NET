namespace KaitaiTtf.Enums
{
    public enum Platforms
    {
        Unicode = 0,
        Macintosh = 1,
        Reserved2 = 2,
        Microsoft = 3,
    }

    public enum Names
    {
        Copyright = 0,
        FontFamily = 1,
        FontSubfamily = 2,
        UniqueSubfamilyId = 3,
        FullFontName = 4,
        NameTableVersion = 5,
        PostscriptFontName = 6,
        Trademark = 7,
        Manufacturer = 8,
        Designer = 9,
        Description = 10,
        UrlVendor = 11,
        UrlDesigner = 12,
        License = 13,
        UrlLicense = 14,
        Reserved15 = 15,
        PreferredFamily = 16,
        PreferredSubfamily = 17,
        CompatibleFullName = 18,
        SampleText = 19,
    }

    public enum Flags
    {
        BaselineAtY0 = 1,
        LeftSidebearingAtX0 = 2,
        FlagDependOnPointSize = 4,
        FlagForcePpem = 8,
        FlagMayAdvanceWidth = 16,
    }

    public enum FontDirectionHint
    {
        FullyMixedDirectionalGlyphs = 0,
        OnlyStronglyLeftToRight = 1,
        StronglyLeftToRightAndNeutrals = 2,
    }

    public enum WeightClass
    {
        Thin = 100,
        ExtraLight = 200,
        Light = 300,
        Normal = 400,
        Medium = 500,
        SemiBold = 600,
        Bold = 700,
        ExtraBold = 800,
        Black = 900,
    }

    public enum WidthClass
    {
        UltraCondensed = 1,
        ExtraCondensed = 2,
        Condensed = 3,
        SemiCondensed = 4,
        Normal = 5,
        SemiExpanded = 6,
        Expanded = 7,
        ExtraExpanded = 8,
        UltraExpanded = 9,
    }

    public enum FsType
    {
        RestrictedLicenseEmbedding = 2,
        PreviewAndPrintEmbedding = 4,
        EditableEmbedding = 8,
    }

    public enum FsSelection
    {
        Italic = 1,
        Underscore = 2,
        Negative = 4,
        Outlined = 8,
        Strikeout = 16,
        Bold = 32,
        Regular = 64,
    }

    public enum Weight
    {
        Any = 0,
        NoFit = 1,
        VeryLight = 2,
        Light = 3,
        Thin = 4,
        Book = 5,
        Medium = 6,
        Demi = 7,
        Bold = 8,
        Heavy = 9,
        Black = 10,
        Nord = 11,
    }

    public enum Proportion
    {
        Any = 0,
        NoFit = 1,
        OldStyle = 2,
        Modern = 3,
        EvenWidth = 4,
        Expanded = 5,
        Condensed = 6,
        VeryExpanded = 7,
        VeryCondensed = 8,
        Monospaced = 9,
    }

    public enum FamilyKind
    {
        Any = 0,
        NoFit = 1,
        TextAndDisplay = 2,
        Script = 3,
        Decorative = 4,
        Pictorial = 5,
    }

    public enum LetterForm
    {
        Any = 0,
        NoFit = 1,
        NormalContact = 2,
        NormalWeighted = 3,
        NormalBoxed = 4,
        NormalFlattened = 5,
        NormalRounded = 6,
        NormalOffCenter = 7,
        NormalSquare = 8,
        ObliqueContact = 9,
        ObliqueWeighted = 10,
        ObliqueBoxed = 11,
        ObliqueFlattened = 12,
        ObliqueRounded = 13,
        ObliqueOffCenter = 14,
        ObliqueSquare = 15,
    }

    public enum SerifStyle
    {
        Any = 0,
        NoFit = 1,
        Cove = 2,
        ObtuseCove = 3,
        SquareCove = 4,
        ObtuseSquareCove = 5,
        Square = 6,
        Thin = 7,
        Bone = 8,
        Exaggerated = 9,
        Triangle = 10,
        NormalSans = 11,
        ObtuseSans = 12,
        PerpSans = 13,
        Flared = 14,
        Rounded = 15,
    }

    public enum XHeight
    {
        Any = 0,
        NoFit = 1,
        ConstantSmall = 2,
        ConstantStandard = 3,
        ConstantLarge = 4,
        DuckingSmall = 5,
        DuckingStandard = 6,
        DuckingLarge = 7,
    }

    public enum ArmStyle
    {
        Any = 0,
        NoFit = 1,
        StraightArmsHorizontal = 2,
        StraightArmsWedge = 3,
        StraightArmsVertical = 4,
        StraightArmsSingleSerif = 5,
        StraightArmsDoubleSerif = 6,
        NonStraightArmsHorizontal = 7,
        NonStraightArmsWedge = 8,
        NonStraightArmsVertical = 9,
        NonStraightArmsSingleSerif = 10,
        NonStraightArmsDoubleSerif = 11,
    }

    public enum StrokeVariation
    {
        Any = 0,
        NoFit = 1,
        GradualDiagonal = 2,
        GradualTransitional = 3,
        GradualVertical = 4,
        GradualHorizontal = 5,
        RapidVertical = 6,
        RapidHorizontal = 7,
        InstantVertical = 8,
    }

    public enum Contrast
    {
        Any = 0,
        NoFit = 1,
        None = 2,
        VeryLow = 3,
        Low = 4,
        MediumLow = 5,
        Medium = 6,
        MediumHigh = 7,
        High = 8,
        VeryHigh = 9,
    }

    public enum Midline
    {
        Any = 0,
        NoFit = 1,
        StandardTrimmed = 2,
        StandardPointed = 3,
        StandardSerifed = 4,
        HighTrimmed = 5,
        HighPointed = 6,
        HighSerifed = 7,
        ConstantTrimmed = 8,
        ConstantPointed = 9,
        ConstantSerifed = 10,
        LowTrimmed = 11,
        LowPointed = 12,
        LowSerifed = 13,
    }

    public enum SubtableFormat
    {
        ByteEncodingTable = 0,
        HighByteMappingThroughTable = 2,
        SegmentMappingToDeltaValues = 4,
        TrimmedTableMapping = 6,
    }
}