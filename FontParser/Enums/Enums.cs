﻿// ReSharper disable CheckNamespace

using System;

public enum LineSpacingChoice
{
    TypoMetric,
    Windows,
    Mac
}

[Flags]
public enum TranslatedOs2FontStyle : ushort
{
    //@prepare's note, please note:=> this is not real value, this is 'translated' value from OS2.fsSelection

    UNSET = 0,

    ITALIC = 1,
    BOLD = 1 << 1,
    REGULAR = 1 << 2,
    OBLIQUE = 1 << 3,
}

public enum Os2WidthClass : byte
{
    //from https://docs.microsoft.com/en-us/typography/opentype/spec/os2#uswidthclass

    //
    //Value 	Description 	C Definition 	        % of normal
    //1 	Ultra-condensed 	FWIDTH_ULTRA_CONDENSED 	50
    //2 	Extra-condensed 	FWIDTH_EXTRA_CONDENSED 	62.5
    //3 	Condensed 	        FWIDTH_CONDENSED 	    75
    //4 	Semi-condensed 	    FWIDTH_SEMI_CONDENSED 	87.5
    //5 	Medium (normal) 	FWIDTH_NORMAL 	        100
    //6 	Semi-expanded 	    FWIDTH_SEMI_EXPANDED 	112.5
    //7 	Expanded 	        FWIDTH_EXPANDED 	    125
    //8 	Extra-expanded 	    FWIDTH_EXTRA_EXPANDED 	150
    //9 	Ultra-expanded      FWIDTH_ULTRA_EXPANDED 	200

    Unknown,//@prepare's => my custom
    UltraCondensed,
    ExtraCondensed,
    Condensed,
    SemiCondensed,
    Medium = 5,
    Normal = 5,
    SemiExpanded = 6,
    Expanded = 7,
    ExtraExpanded = 8,
    UltraExpanded = 9
}

public enum TrimMode
{
    /// <summary>
    /// No trim, full glyph instruction
    /// </summary>
    No, //default

    /// <summary>
    /// only essential info for glyph layout
    /// </summary>
    EssentialLayoutInfo,

    /// <summary>
    /// restore again
    /// </summary>
    Restored,
}

//https://docs.microsoft.com/en-us/typography/opentype/spec/gdef
public enum GlyphClassKind : byte
{
    //1 	Base glyph (single character, spacing glyph)
    //2 	Ligature glyph (multiple character, spacing glyph)
    //3 	Mark glyph (non-spacing combining glyph)
    //4 	Component glyph (part of single character, spacing glyph)
    //
    // The font developer does not have to classify every glyph in the font,
    //but any glyph not assigned a class value falls into Class zero (0).
    //For instance, class values might be useful for the Arabic glyphs in a font, but not for the Latin glyphs.
    //Then the GlyphClassDef table will list only Arabic glyphs, and-by default-the Latin glyphs will be assigned to Class 0.
    //Component glyphs can be put together to generate ligatures.
    //A ligature can be generated by creating a glyph in the font that references the component glyphs,
    //or outputting the component glyphs in the desired sequence.
    //Component glyphs are not used in defining any GSUB or GPOS formats.
    //
    Zero = 0,//class0, classZero

    /// <summary>
    /// Base glyph (single character, spacing glyph)
    /// </summary>
    Base,

    /// <summary>
    /// Ligature glyph (multiple character, spacing glyph)
    /// </summary>
    Ligature,

    /// <summary>
    /// Mark glyph (non-spacing combining glyph)
    /// </summary>
    Mark,

    /// <summary>
    /// Component glyph (part of single character, spacing glyph)
    /// </summary>
    Component
}

[Flags]
public enum ReadFlags
{
    Full = 0,
    Name = 1,
    Metrics = 1 << 2,
    AdvancedLayout = 1 << 3,
    Variation = 1 << 4
}

public enum NameIdKind
{
    //...
    //[A] The key information for this table for Microsoft platforms
    //relates to the use of strings 1, 2, 4, 16 and 17.
    //...

    CopyRightNotice, //0
    FontFamilyName, //1 , [A]
    FontSubfamilyName,//2, [A]
    UniqueFontIden, //3
    FullFontName, //4, [A]
    VersionString,//5
    PostScriptName,//6
    Trademark,//7
    ManufacturerName,//8
    Designer,//9
    Description, //10
    UrlVendor, //11
    UrlDesigner,//12
    LicenseDescription, //13
    LicenseInfoUrl,//14
    Reserved,//15
    TypographicFamilyName,//16 , [A]
    TypographicSubfamilyName,//17, [A]
    CompatibleFull,//18
    SampleText,//19
    PostScriptCID_FindfontName,//20

    //------------------
    WWSFamilyName,//21

    WWSSubfamilyName,//22

    //------------------
    LightBackgroundPalette,//23, CPAL

    DarkBackgroundPalette,//24, CPAL

    //------------------
    VariationsPostScriptNamePrefix,//25
}

public enum OperandKind
{
    IntNumber,
    RealNumber
}

public enum OperatorOperandKind
{
    SID,
    Boolean,
    Number,
    Array,
    Delta,

    //compound
    NumberNumber,

    SID_SID_Number,
}

public enum CompactRange
{
    None,

    //
    SByte,

    Short,
}