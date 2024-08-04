using System.Collections.Generic;
using FontParser.AdditionalInfo;
using FontParser.Exceptions;
using FontParser.Tables;
using FontParser.Tables.AdvancedLayout;
using FontParser.Tables.AdvancedLayout.Base;
using FontParser.Tables.AdvancedLayout.FontMath;
using FontParser.Tables.AdvancedLayout.GPOS;
using FontParser.Tables.AdvancedLayout.GSUB;
using FontParser.Tables.BitmapAndSvgFonts;
using FontParser.Tables.CFF.CFF;
using FontParser.Tables.Others;
using FontParser.Tables.TrueType;
using Kern = FontParser.Tables.Others.Kern;

namespace FontParser.Typeface
{
    public class Typeface
    {
        //TODO: implement vertical metrics

        #region Public Vars

        public bool HasPrepProgramBuffer => PrepProgramBuffer != null;

        /// <summary>
        /// actual font filename (optional)
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// OS2 sTypoAscender/HheaTable.Ascent, in font designed unit
        /// </summary>
        public short Ascender => _useTypographicMertic ? OS2Table.sTypoAscender : HheaTable.Ascent;

        /// <summary>
        /// OS2 sTypoDescender, in font designed unit
        /// </summary>
        public short Descender => _useTypographicMertic ? OS2Table.sTypoDescender : HheaTable.Descent;

        /// <summary>
        /// OS2 usWinAscender
        /// </summary>
        public ushort ClippedAscender => OS2Table.usWinAscent;

        /// <summary>
        /// OS2 usWinDescender
        /// </summary>
        public ushort ClippedDescender => OS2Table.usWinDescent;

        /// <summary>
        /// OS2 Linegap
        /// </summary>
        public short LineGap => _useTypographicMertic ? OS2Table.sTypoLineGap : HheaTable.LineGap;

        //The typographic line gap for this font.
        //Remember that this is not the same as the LineGap value in the 'hhea' table,
        //which Apple defines in a far different manner.
        //The suggested usage for sTypoLineGap is
        //that it be used in conjunction with unitsPerEm
        //to compute a typographically correct default line spacing.
        //
        //Typical values average 7 - 10 % of units per em.
        //The goal is to free applications from Macintosh or Windows - specific metrics
        //which are constrained by backward compatibility requirements
        //(see chapter, “Recommendations for OpenType Fonts”).
        //These new metrics, when combined with the character design widths,
        //will allow applications to lay out documents in a typographically correct and portable fashion.
        //These metrics will be exposed through Windows APIs.
        //Macintosh applications will need to access the 'sfnt' resource and
        //parse it to extract this data from the “OS / 2” table
        //(unless Apple exposes the 'OS/2' table through a new API)
        //---------------

        public string Name => _nameEntry.FontName;
        public string FontSubFamily => _nameEntry.FontSubFamily;
        public string PostScriptName => _nameEntry.PostScriptName;
        public string VersionString => _nameEntry.VersionString;
        public string UniqueFontIden => _nameEntry.UniqueFontIden;

        public Os2Table OS2Table { get; set; }

        public int GlyphCount => _glyphs.Length;

        public Languages Languages { get; } = new Languages();

        public Bounds Bounds { get; private set; }

        public ushort UnitsPerEm { get; private set; }

        public short UnderlinePosition => PostTable.UnderlinePosition; //TODO: review here

        /// <summary>
        /// default dpi
        /// </summary>
        public static uint DefaultDpi { get; set; } = 96;

        //

        public COLR COLRTable { get; private set; }
        public CPAL CPALTable { get; private set; }
        public bool IsCffFont => _hasCffData;

        public GPOS GPOSTable { get; internal set; }
        public GSUB GSUBTable { get; internal set; }

        public Constants MathConsts => _mathTable?._mathConstTable;

        public bool IsBitmapFont => _bitmapFontGlyphSource != null;

        #endregion Public Vars

        #region Internal Vars

        internal TrimMode _typefaceTrimMode;
        internal int _typefaceKey;
        internal Head Head { get; set; }
        internal bool _useTypographicMertic;

        /// <summary>
        /// control values in Font unit
        /// </summary>
        internal int[] ControlValues { get; set; }

        internal byte[] PrepProgramBuffer { get; set; }
        internal byte[] FpgmProgramBuffer { get; set; }

        internal MaxProfile MaxProfile { get; set; }
        internal Cmap CmapTable { get; set; }
        internal Kern KernTable { get; set; }
        internal Gasp GaspTable { get; set; }
        internal HorizontalHeader HheaTable { get; set; }

        internal NameEntry NameEntry => _nameEntry;

        internal BASE BaseTable { get; set; }
        internal GDEF GDEFTable { get; set; }

        internal bool HasColorAndPal { get; private set; }

        internal PostTable PostTable { get; set; }
        internal bool _evalCffGlyphBounds;
        internal MathTable _mathTable;
        internal bool HasSvgTable { get; private set; }
        internal BitmapFontGlyphSource _bitmapFontGlyphSource;

        #endregion Internal Vars

        #region Private Vars

        private NameEntry _nameEntry;
        private Glyph[] _glyphs;
        private Cff1FontSet _cff1FontSet;
        private TableHeader[] _tblHeaders;
        private bool _hasTtfOutline;
        private bool _hasCffData;
        private const int s_pointsPerInch = 72;//point per inch, fix?
        private GlyphInfo[] _mathGlyphInfos;
        private Dictionary<string, ushort> _cachedGlyphDicByName;
        private HorizontalMetrics _hMetrics;

        //-------------------------
        //svg and bitmap font
        private SvgTable _svgTable;

#if DEBUG
        private static int s_dbugTotalId;
        public readonly int dbugId = ++s_dbugTotalId;
#endif

        #endregion Private Vars

        internal Typeface()
        {
            //blank typefaces
#if DEBUG
            if (dbugId == 5)
            {
            }
#endif
        }

        #region Public Methods

        /// <summary>
        /// find glyph index by codepoint
        /// </summary>
        /// <param name="codepoint"></param>
        /// <param name="nextCodepoint"></param>
        /// <returns></returns>

        public ushort GetGlyphIndex(int codepoint, int nextCodepoint, out bool skipNextCodepoint)
        {
            return CmapTable.GetGlyphIndex(codepoint, nextCodepoint, out skipNextCodepoint);
        }

        public ushort GetGlyphIndex(int codepoint)
        {
            return CmapTable.GetGlyphIndex(codepoint, 0, out bool skipNextCodepoint);
        }

        public void CollectUnicode(List<uint> unicodes)
        {
            CmapTable.CollectUnicode(unicodes);
        }

        public void CollectUnicode(int platform, List<uint> unicodes, List<ushort> glyphIndexList)
        {
            CmapTable.CollectUnicode(platform, unicodes, glyphIndexList);
        }

        public Glyph GetGlyphByName(string glyphName) => GetGlyph(GetGlyphIndexByName(glyphName));

        public ushort GetGlyphIndexByName(string glyphName)
        {
            if (glyphName == null) return 0;

            if (_cff1FontSet != null && _cachedGlyphDicByName == null)
            {
                //we create a dictionary
                //create cache data
                _cachedGlyphDicByName = new Dictionary<string, ushort>();
                for (int i = 1; i < _glyphs.Length; ++i)
                {
                    Glyph glyph = _glyphs[i];
                    if (glyph._cff1GlyphData.Name != null)
                    {
                        _cachedGlyphDicByName.Add(glyph._cff1GlyphData.Name, (ushort)i);
                    }
                    else
                    {
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("Cff unknown glyphname");
#endif
                    }
                }
                return _cachedGlyphDicByName.TryGetValue(glyphName, out ushort glyphIndex) ? glyphIndex : (ushort)0;
            }
            else if (PostTable != null)
            {
                if (PostTable.Version == 2)
                {
                    return PostTable.GetGlyphIndex(glyphName);
                }
                else
                {
                    //check data from adobe glyph list
                    //from the unicode value
                    //select glyph index

                    //we use AdobeGlyphList
                    //from https://github.com/adobe-type-tools/agl-aglfn/blob/master/glyphlist.txt

                    //but user can provide their own map here...

                    return GetGlyphIndex(AdobeGlyphList.GetUnicodeValueByGlyphName(glyphName));
                }
            }
            return 0;
        }

        public IEnumerable<GlyphNameMap> GetGlyphNameIter()
        {
            if (_cachedGlyphDicByName == null && _cff1FontSet != null)
            {
                UpdateCff1FontSetNamesCache();
            }

            if (_cachedGlyphDicByName != null)
            {
                //iter from here
                foreach (var kv in _cachedGlyphDicByName)
                {
                    yield return new GlyphNameMap(kv.Value, kv.Key);
                }
            }
            else if (PostTable.Version == 2)
            {
                foreach (var kp in PostTable.GlyphNames)
                {
                    yield return new GlyphNameMap(kp.Key, kp.Value);
                }
            }
        }

        public Glyph GetGlyph(ushort glyphIndex)
        {
            if (glyphIndex < _glyphs.Length)
            {
                return _glyphs[glyphIndex];
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("found unknown glyph:" + glyphIndex);
#endif
                return _glyphs[0]; //return empty glyph?;
            }
        }

        public ushort GetAdvanceWidthFromGlyphIndex(ushort glyphIndex) => _hMetrics.GetAdvanceWidth(glyphIndex);

        public short GetLeftSideBearing(ushort glyphIndex) => _hMetrics.GetLeftSideBearing(glyphIndex);

        public short GetKernDistance(ushort leftGlyphIndex, ushort rightGlyphIndex)
        {
            //DEPRECATED -> use OpenFont layout instead
            return KernTable.GetKerningDistance(leftGlyphIndex, rightGlyphIndex);
        }

        /// <summary>
        /// convert from point-unit value to pixel value
        /// </summary>
        /// <param name="targetPointSize"></param>
        /// <param name="resolution">dpi</param>
        /// <returns></returns>
        public static float ConvPointsToPixels(float targetPointSize, int resolution = -1)
        {
            //http://stackoverflow.com/questions/139655/convert-pixels-to-points
            //points = pixels * 72 / 96
            //------------------------------------------------
            //pixels = targetPointSize * 96 /72
            //pixels = targetPointSize * resolution / pointPerInch

            if (resolution < 0)
            {
                //use current DefaultDPI
                resolution = (int)DefaultDpi;
            }

            return targetPointSize * resolution / s_pointsPerInch;
        }

        /// <summary>
        /// calculate scale to target pixel size based on current typeface's UnitsPerEm
        /// </summary>
        /// <param name="targetPixelSize">target font size in point unit</param>
        /// <returns></returns>
        public float CalculateScaleToPixel(float targetPixelSize)
        {
            //1. return targetPixelSize / UnitsPerEm
            return targetPixelSize / UnitsPerEm;
        }

        /// <summary>
        ///  calculate scale to target pixel size based on current typeface's UnitsPerEm
        /// </summary>
        /// <param name="targetPointSize">target font size in point unit</param>
        /// <param name="resolution">dpi</param>
        /// <returns></returns>
        public float CalculateScaleToPixelFromPointSize(float targetPointSize, int resolution = -1)
        {
            //1. var sizeInPixels = ConvPointsToPixels(sizeInPointUnit);
            //2. return sizeInPixels / UnitsPerEm

            if (resolution < 0)
            {
                //use current DefaultDPI
                resolution = (int)DefaultDpi;
            }
            return (targetPointSize * resolution / s_pointsPerInch) / UnitsPerEm;
        }

        public GlyphInfo GetMathGlyphInfo(ushort glyphIndex) => _mathGlyphInfos[glyphIndex];

        public void ReadSvgContent(ushort glyphIndex, System.Text.StringBuilder output) => _svgTable?.ReadSvgContent(glyphIndex, output);

        public void ReadBitmapContent(Glyph glyph, System.IO.Stream output)
        {
            _bitmapFontGlyphSource.CopyBitmapContent(glyph, output);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void LoadOpenFontLayoutInfo(GDEF gdefTable, GSUB gsubTable, GPOS gposTable, BASE baseTable, COLR colrTable, CPAL cpalTable)
        {
            //***
            GDEFTable = gdefTable;
            GSUBTable = gsubTable;
            GPOSTable = gposTable;
            BaseTable = baseTable;
            COLRTable = colrTable;
            CPALTable = cpalTable;
            //---------------------------
            //fill glyph definition
            if (gdefTable != null)
            {
                gdefTable.FillGlyphData(_glyphs);
            }
        }

        internal void SetColorAndPalTable(COLR colr, CPAL cpal)
        {
            COLRTable = colr;
            CPALTable = cpal;
            HasColorAndPal = colr != null;
        }

        internal void SetTableEntryCollection(TableHeader[] headers) => _tblHeaders = headers;

        internal void SetBasicTypefaceTables(Os2Table os2Table,
             NameEntry nameEntry,
             Head head,
             HorizontalMetrics horizontalMetrics)
        {
            OS2Table = os2Table;
            _nameEntry = nameEntry;
            Head = head;
            Bounds = head.Bounds;
            UnitsPerEm = head.UnitsPerEm;
            _hMetrics = horizontalMetrics;
        }

        internal void SetTtfGlyphs(Glyph[] glyphs)
        {
            _glyphs = glyphs;
            _hasTtfOutline = true;
        }

        internal void SetBitmapGlyphs(Glyph[] glyphs, BitmapFontGlyphSource bitmapFontGlyphSource)
        {
            _glyphs = glyphs;
            _bitmapFontGlyphSource = bitmapFontGlyphSource;
        }

        internal void SetCffFontSet(Cff1FontSet cff1FontSet)
        {
            _cff1FontSet = cff1FontSet;
            _hasCffData = true;

            Glyph[] exisitingGlyphs = _glyphs;

            _glyphs = cff1FontSet._fonts[0]._glyphs; //TODO: review _fonts[0]

            if (exisitingGlyphs != null)
            {
                //
#if DEBUG
                if (_glyphs.Length != exisitingGlyphs.Length)
                {
                    throw new OpenFontNotSupportedException();
                }
#endif
                for (int i = 0; i < exisitingGlyphs.Length; ++i)
                {
                    Glyph.CopyExistingGlyphInfo(exisitingGlyphs[i], _glyphs[i]);
                }
            }
        }

        internal void LoadMathGlyphInfos(GlyphInfo[] mathGlyphInfos)
        {
            _mathGlyphInfos = mathGlyphInfos;
            if (mathGlyphInfos != null)
            {
                //fill to original glyph?
                for (int glyphIndex = 0; glyphIndex < _glyphs.Length; ++glyphIndex)
                {
                    _glyphs[glyphIndex].GlyphInfo = mathGlyphInfos[glyphIndex];
                }
            }
        }

        internal void SetSvgTable(SvgTable svgTable)
        {
            HasSvgTable = (_svgTable = svgTable) != null;
        }

        /// <summary>
        /// undate lang info
        /// </summary>
        /// <param name="metaTable"></param>
        internal void UpdateLangs(Meta metaTable) => Languages.Update(OS2Table, metaTable, CmapTable, GSUBTable, GPOSTable);

        internal ushort _whitespaceWidth; //common used value

        internal void UpdateFrequentlyUsedValues()
        {
            //whitespace
            ushort whitespace_glyphIndex = GetGlyphIndex(' ');
            if (whitespace_glyphIndex > 0)
            {
                _whitespaceWidth = GetAdvanceWidthFromGlyphIndex(whitespace_glyphIndex);
            }
        }

        internal RestoreTicket TrimDownAndRemoveGlyphBuildingDetail()
        {
            switch (_typefaceTrimMode)
            {
                default: throw new OpenFontNotSupportedException();
                case TrimMode.EssentialLayoutInfo: return null;//same mode
                case TrimMode.Restored:
                case TrimMode.No:
                    {
                        RestoreTicket ticket = new RestoreTicket
                        {
                            TypefaceName = Name,
                            Headers = _tblHeaders, //a copy
                            //FROM:GlyphLoadingMode.Full => TO: GlyphLoadingMode.EssentialLayoutInfo
                            HasTtf = _hasTtfOutline
                        };

                        //cache glyph name before unload
                        if (_cff1FontSet != null)
                        {
                            ticket.HasCff = true;
                            UpdateCff1FontSetNamesCache();//***
                            _cff1FontSet = null;
                        }

                        //1.Ttf and Otf => clone each glyphs in NO building
                        Glyph[] newClones = new Glyph[_glyphs.Length];
                        for (int i = 0; i < newClones.Length; ++i)
                        {
                            newClones[i] = Glyph.Clone_NO_BuildingInstructions(_glyphs[i]);
                        }
                        _glyphs = newClones;

                        //and since glyph has no building instructions in this mode
                        //so  ...

                        ticket.ControlValues = ControlValues != null;
                        ControlValues = null;

                        ticket.PrepProgramBuffer = PrepProgramBuffer != null;
                        PrepProgramBuffer = null;

                        ticket.FpgmProgramBuffer = FpgmProgramBuffer != null;
                        FpgmProgramBuffer = null;

                        ticket.CPALTable = CPALTable != null;
                        CPALTable = null;

                        ticket.COLRTable = COLRTable != null;
                        COLRTable = null;

                        ticket.GaspTable = GaspTable != null;
                        GaspTable = null;

                        //
                        //3. Svg=> remove SvgTable
                        if (_svgTable != null)
                        {
                            ticket.HasSvg = true;
                            _svgTable.UnloadSvgData();
                            _svgTable = null;
                        }

                        //4. Bitmap Font => remove embeded bitmap data
                        if (_bitmapFontGlyphSource != null)
                        {
                            ticket.HasBitmapSource = true;
                            _bitmapFontGlyphSource.UnloadCBDT();
                        }

                        _typefaceTrimMode = TrimMode.EssentialLayoutInfo;

                        return ticket;
                    }
            }
        }

        internal bool CompareOriginalHeadersWithNewlyLoadOne(TableHeader[] others)
        {
            if (_tblHeaders != null && others != null &&
                _tblHeaders.Length == others.Length)
            {
                for (int i = 0; i < _tblHeaders.Length; ++i)
                {
                    TableHeader a = _tblHeaders[i];
                    TableHeader b = others[i];

                    if (a.Tag != b.Tag ||
                        a.Offset != b.Offset ||
                        a.Length != b.Length ||
                        a.CheckSum != b.CheckSum)
                    {
#if DEBUG
                        System.Diagnostics.Debugger.Break();
#endif

                        return false;
                    }
                }
                //pass all
                return true;
            }
            return false;
        }

        #endregion Internal Methods

        #region Private Methods

        private void UpdateCff1FontSetNamesCache()
        {
            if (_cff1FontSet != null && _cachedGlyphDicByName == null)
            {
                //create cache data
                _cachedGlyphDicByName = new Dictionary<string, ushort>();
                for (int i = 1; i < _glyphs.Length; ++i)
                {
                    Glyph glyph = _glyphs[i];

                    if (glyph._cff1GlyphData != null && glyph._cff1GlyphData.Name != null)
                    {
                        _cachedGlyphDicByName.Add(glyph._cff1GlyphData.Name, (ushort)i);
                    }
                    else
                    {
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("Cff unknown glyphname");
#endif
                    }
                }
            }
        }

#if DEBUG

        public override string ToString() => Name;

#endif
    }

    #endregion Private Methods
}