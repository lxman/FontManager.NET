using FontParser.AdditionalInfo;
using FontParser.Tables;
using FontParser.Typeface;
using FontParser.Typeface.Os2;

namespace FontParser
{
    public class PreviewFontInfo
    {
        public readonly string Name;
        public readonly string SubFamilyName;
        public readonly TranslatedOs2FontStyle Os2TranslatedStyle;
        public readonly Os2FsSelection Os2FsSelection;

        private readonly PreviewFontInfo[] _ttcfMembers;

        public Languages Languages { get; }
        public NameEntry NameEntry { get; }
        public Os2Table Os2Table { get; }

        internal PreviewFontInfo(
            NameEntry nameEntry,
            Os2Table os2Table,
            Languages langs)
        {
            NameEntry = nameEntry;
            Os2Table = os2Table;
            Languages = langs;

            Name = nameEntry.FontName;
            SubFamilyName = nameEntry.FontSubFamily;
            Os2TranslatedStyle = TypefaceExtensions.TranslateOs2FontStyle(os2Table);
            Os2FsSelection = TypefaceExtensions.TranslateOs2FsSelection(os2Table);
        }

        internal PreviewFontInfo(string fontName, PreviewFontInfo[] ttcfMembers)
        {
            Name = fontName;
            SubFamilyName = "";
            _ttcfMembers = ttcfMembers;
            Languages = new Languages();
        }

        public string TypographicFamilyName => (NameEntry?.TypographicFamilyName) ?? string.Empty;
        public string TypographicSubFamilyName => (NameEntry?.TypographyicSubfamilyName) ?? string.Empty;
        public string PostScriptName => (NameEntry?.PostScriptName) ?? string.Empty;
        public string UniqueFontIden => (NameEntry?.UniqueFontIden) ?? string.Empty;
        public string VersionString => (NameEntry?.VersionString) ?? string.Empty;
        public ushort WeightClass => (Os2Table != null) ? Os2Table.usWeightClass : ushort.MinValue;
        public ushort WidthClass => (Os2Table != null) ? Os2Table.usWidthClass : ushort.MinValue;

        public int ActualStreamOffset { get; internal set; }
        public bool IsWebFont { get; internal set; }
        public bool IsFontCollection => _ttcfMembers != null;

        /// <summary>
        /// get font collection's member count
        /// </summary>
        public int MemberCount => _ttcfMembers.Length;

        /// <summary>
        /// get font collection's member
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PreviewFontInfo GetMember(int index) => _ttcfMembers[index];

#if DEBUG

        public override string ToString()
        {
            return (IsFontCollection) ? Name : Name + ", " + SubFamilyName + ", " + Os2TranslatedStyle;
        }

#endif
    }
}