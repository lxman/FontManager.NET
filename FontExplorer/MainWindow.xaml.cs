using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using NewFontParser;
using NewFontParser.Tables;
using NewFontParser.Tables.Avar;
using NewFontParser.Tables.Base;
using NewFontParser.Tables.Bitmap.Cbdt;
using NewFontParser.Tables.Bitmap.Cblc;
using NewFontParser.Tables.Bitmap.Ebdt;
using NewFontParser.Tables.Bitmap.Eblc;
using NewFontParser.Tables.Bitmap.Ebsc;
using NewFontParser.Tables.Cff.Type1;
using NewFontParser.Tables.Cmap;
using NewFontParser.Tables.Cmap.SubTables;
using NewFontParser.Tables.Colr;
using NewFontParser.Tables.Cpal;
using NewFontParser.Tables.Cvar;
using NewFontParser.Tables.Fftm;
using NewFontParser.Tables.Fvar;
using NewFontParser.Tables.Gdef;
using NewFontParser.Tables.Gpos;
using NewFontParser.Tables.Gsub;
using NewFontParser.Tables.Gvar;
using NewFontParser.Tables.Head;
using NewFontParser.Tables.Hhea;
using NewFontParser.Tables.Hmtx;
using NewFontParser.Tables.Hvar;
using NewFontParser.Tables.Jstf;
using NewFontParser.Tables.Kern;
using NewFontParser.Tables.Math;
using NewFontParser.Tables.Merg;
using NewFontParser.Tables.Meta;
using NewFontParser.Tables.Mvar;
using NewFontParser.Tables.Name;
using NewFontParser.Tables.Optional;
using NewFontParser.Tables.Optional.Dsig;
using NewFontParser.Tables.Optional.Hdmx;

namespace FontExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFontClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "TrueType Font (*.ttf)|*.ttf|OpenType Font (*.otf)|*.otf|TrueType Collection (*.ttc)|*.ttc"
            };
            bool ofdResult = dialog.ShowDialog() ?? false;
            if (!ofdResult)
            {
                return;
            }
            ResultView.Items.Clear();
            var reader = new FontReader();
            List<(string, List<IFontTable>)> result2 = reader.GetTables(dialog.FileName);
            (string, List<IFontTable>) firstItem = result2.FirstOrDefault();
            List<IFontTable>? tables = firstItem.Item2;
            tables.ForEach(t =>
            {
                switch (t)
                {
                    case CmapTable cmapTable:
                        var cmapRoot = new TreeViewItem { Header = "cmap" };
                        ResultView.Items.Add(cmapRoot);
                        var encodingRecords = new TreeViewItem { Header = "Encoding Records" };
                        cmapRoot.Items.Add(encodingRecords);
                        cmapTable.EncodingRecords.ForEach(er =>
                        {
                            var platformId = new TreeViewItem { Header = er.PlatformId.ToString() };
                            encodingRecords.Items.Add(platformId);
                            if (er.EncodingId0 is not null)
                            {
                                var encodingId0 = new TreeViewItem { Header = er.EncodingId0.ToString() };
                                platformId.Items.Add(encodingId0);
                            }
                            if (er.EncodingId1 is not null)
                            {
                                var encodingId1 = new TreeViewItem { Header = er.EncodingId1.ToString() };
                                platformId.Items.Add(encodingId1);
                            }
                            if (er.EncodingId2 is not null)
                            {
                                var encodingId2 = new TreeViewItem { Header = er.EncodingId2.ToString() };
                                platformId.Items.Add(encodingId2);
                            }
                            if (er.EncodingId3 is null) return;
                            var encodingId3 = new TreeViewItem { Header = er.EncodingId3.ToString() };
                            platformId.Items.Add(encodingId3);
                        });
                        var subtableItem = new TreeViewItem { Header = "Subtables" };
                        cmapRoot.Items.Add(subtableItem);
                        cmapTable.SubTables.ForEach(s =>
                        {
                            var glyphIdArray = new TreeViewItem { Header = "Glyph Index Array" };
                            switch (s)
                            {
                                case CmapSubtableFormat0 format0:
                                    var format0Item = new TreeViewItem { Header = "Format 0" };
                                    var languageItem = new TreeViewItem { Header = $"Language - {format0.Language}" };
                                    format0Item.Items.Add(languageItem);
                                    glyphIdArray.Items.Clear();
                                    glyphIdArray = new TreeViewItem { Header = "Glyph Index Array" };
                                    format0Item.Items.Add(glyphIdArray);
                                    var glyphIdArrayData = new TreeViewItem { Header = string.Join(", ", format0.GlyphIndexArray) };
                                    glyphIdArray.Items.Add(glyphIdArrayData);
                                    subtableItem.Items.Add(format0Item);
                                    break;
                                case CmapSubtablesFormat2 format2:
                                    var format2Item = new TreeViewItem { Header = "Format 2" };
                                    languageItem = new TreeViewItem { Header = $"Language - {format2.Language}" };
                                    format2Item.Items.Add(languageItem);
                                    glyphIdArray.Items.Clear();
                                    glyphIdArray = new TreeViewItem { Header = "Glyph Index Array" };
                                    format2Item.Items.Add(glyphIdArray);
                                    glyphIdArrayData = new TreeViewItem { Header = string.Join(", ", format2.GlyphIndexArray) };
                                    glyphIdArray.Items.Add(glyphIdArrayData);
                                    var subHeaderKeys = new TreeViewItem { Header = "Sub Header Keys" };
                                    format2Item.Items.Add(subHeaderKeys);
                                    var subHeaderKeysData = new TreeViewItem { Header = string.Join(", ", format2.SubHeaderKeys) };
                                    subHeaderKeys.Items.Add(subHeaderKeysData);
                                    format2.SubHeaders.ForEach(sh =>
                                    {
                                        var subHeader = new TreeViewItem { Header = "Sub Header" };
                                        format2Item.Items.Add(subHeader);
                                        var firstCode = new TreeViewItem { Header = $"First Code - {sh.FirstCode}" };
                                        var entryCount = new TreeViewItem { Header = $"Entry Count - {sh.EntryCount}" };
                                        var idDelta = new TreeViewItem { Header = $"Id Delta - {sh.IdDelta}" };
                                        var idRangeOffset = new TreeViewItem { Header = $"Id Range Offset - {sh.IdRangeOffset}" };
                                        subHeader.Items.Add(firstCode);
                                        subHeader.Items.Add(entryCount);
                                        subHeader.Items.Add(idDelta);
                                        subHeader.Items.Add(idRangeOffset);
                                    });
                                    subtableItem.Items.Add(format2Item);
                                    break;
                                case CmapSubtablesFormat4 format4:
                                    var format4Item = new TreeViewItem { Header = "Format 4" };
                                    languageItem = new TreeViewItem { Header = $"Language - {format4.Language}" };
                                    format4Item.Items.Add(languageItem);
                                    var entrySelectorItem = new TreeViewItem { Header = $"Entry Selector - {format4.EntrySelector}" };
                                    format4Item.Items.Add(entrySelectorItem);
                                    var rangeShiftItem = new TreeViewItem { Header = $"Range Shift - {format4.RangeShift}" };
                                    format4Item.Items.Add(rangeShiftItem);
                                    var searchRangeItem = new TreeViewItem { Header = $"Search Range - {format4.SearchRange}" };
                                    format4Item.Items.Add(searchRangeItem);
                                    var segCountX2Item = new TreeViewItem { Header = $"Seg Count X2 - {format4.SegCountX2}" };
                                    format4Item.Items.Add(segCountX2Item);
                                    if (format4.GlyphIdArray.Count > 0)
                                    {
                                        glyphIdArray.Items.Clear();
                                        glyphIdArray = new TreeViewItem { Header = "Glyph Index Array" };
                                        format4Item.Items.Add(glyphIdArray);
                                        glyphIdArrayData = new TreeViewItem { Header = string.Join(", ", format4.GlyphIdArray) };
                                        glyphIdArray.Items.Add(glyphIdArrayData);
                                    }
                                    var rangesItem = new TreeViewItem { Header = "Ranges" };
                                    for (var i = 0; i < format4.StartCodes.Count; i++)
                                    {
                                        var rangeItem = new TreeViewItem { Header = "Range" };
                                        rangesItem.Items.Add(rangeItem);
                                        var startCode = new TreeViewItem { Header = $"Start Code - {format4.StartCodes[i]}" };
                                        var endCode = new TreeViewItem { Header = $"End Code - {format4.EndCodes[i]}" };
                                        var idDelta = new TreeViewItem { Header = $"Id Delta - {format4.IdDeltas[i]}" };
                                        var idRangeOffset = new TreeViewItem { Header = $"Id Range Offset - {format4.IdRangeOffsets[i]}" };
                                        rangeItem.Items.Add(startCode);
                                        rangeItem.Items.Add(endCode);
                                        rangeItem.Items.Add(idDelta);
                                        rangeItem.Items.Add(idRangeOffset);
                                    }
                                    format4Item.Items.Add(rangesItem);
                                    subtableItem.Items.Add(format4Item);
                                    break;
                                case CmapSubtablesFormat6 format6:
                                    var format6Item = new TreeViewItem { Header = "Format 6" };
                                    languageItem = new TreeViewItem { Header = $"Language - {format6.Language}" };
                                    format6Item.Items.Add(languageItem);
                                    var firstCode = new TreeViewItem { Header = $"First Code - {format6.FirstCode}" };
                                    format6Item.Items.Add(firstCode);
                                    glyphIdArray.Items.Clear();
                                    glyphIdArray = new TreeViewItem { Header = "Glyph Index Array" };
                                    format6Item.Items.Add(glyphIdArray);
                                    var glyphIndexArrayData = new TreeViewItem { Header = string.Join(", ", format6.GlyphIndexArray) };
                                    glyphIdArray.Items.Add(glyphIndexArrayData);
                                    subtableItem.Items.Add(format6Item);
                                    break;
                                case CmapSubtablesFormat8 format8:
                                    var format8Item = new TreeViewItem { Header = "Format 8" };
                                    languageItem = new TreeViewItem { Header = $"Language - {format8.Language}" };
                                    format8Item.Items.Add(languageItem);
                                    var is32Item = new TreeViewItem { Header = "Is 32" };
                                    format8Item.Items.Add(is32Item);
                                    var is32Data = new TreeViewItem { Header = string.Join(", ", format8.Is32) };
                                    is32Item.Items.Add(is32Data);
                                    format8.SequentialMapGroups.ForEach(g =>
                                    {
                                        var sequentialMapGroup = new TreeViewItem { Header = "Sequential Map Group" };
                                        format8Item.Items.Add(sequentialMapGroup);
                                        var glyphId = new TreeViewItem { Header = $"Start Glyph Id - {g.StartGlyphId}" };
                                        var startCharCode = new TreeViewItem { Header = $"Start Char Code - {g.StartCharCode}" };
                                        var endCharCode = new TreeViewItem { Header = $"End Char Code - {g.EndCharCode}" };
                                        sequentialMapGroup.Items.Add(glyphId);
                                        sequentialMapGroup.Items.Add(startCharCode);
                                        sequentialMapGroup.Items.Add(endCharCode);
                                    });
                                    subtableItem.Items.Add(format8Item);
                                    break;
                                case CmapSubtablesFormat10 format10:
                                    var format10Item = new TreeViewItem { Header = "Format 10" };
                                    languageItem = new TreeViewItem { Header = $"Language - {format10.Language}" };
                                    format10Item.Items.Add(languageItem);
                                    var startCharItem = new TreeViewItem { Header = $"Start Char - {format10.StartChar}" };
                                    format10Item.Items.Add(startCharItem);
                                    glyphIdArray.Items.Clear();
                                    glyphIdArray = new TreeViewItem { Header = "Glyph Index Array" };
                                    format10Item.Items.Add(glyphIdArray);
                                    glyphIndexArrayData = new TreeViewItem { Header = string.Join(", ", format10.GlyphIndexArray) };
                                    glyphIdArray.Items.Add(glyphIndexArrayData);
                                    subtableItem.Items.Add(format10Item);
                                    break;
                                case CmapSubtablesFormat12 format12:
                                    var format12Item = new TreeViewItem { Header = "Format 12" };
                                    languageItem = new TreeViewItem { Header = $"Language - {format12.Language}" };
                                    format12Item.Items.Add(languageItem);
                                    format12.Groups.ForEach(g =>
                                    {
                                        var sequentialMapGroup = new TreeViewItem { Header = "Sequential Map Group" };
                                        format12Item.Items.Add(sequentialMapGroup);
                                        var glyphId = new TreeViewItem
                                        { Header = $"Start Glyph Id - {g.StartGlyphId}" };
                                        var startCharCode = new TreeViewItem
                                        { Header = $"Start Char Code - {g.StartCharCode}" };
                                        var endCharCode = new TreeViewItem
                                        { Header = $"End Char Code - {g.EndCharCode}" };
                                        sequentialMapGroup.Items.Add(glyphId);
                                        sequentialMapGroup.Items.Add(startCharCode);
                                        sequentialMapGroup.Items.Add(endCharCode);
                                    });
                                    subtableItem.Items.Add(format12Item);
                                    break;
                                case CmapSubtablesFormat13 format13:
                                    var format13Item = new TreeViewItem { Header = "Format 13" };
                                    languageItem = new TreeViewItem { Header = $"Language - {format13.Language}" };
                                    format13Item.Items.Add(languageItem);
                                    format13.Groups.ForEach(g =>
                                    {
                                        var constantMapGroup = new TreeViewItem { Header = "Constant Map Group" };
                                        format13Item.Items.Add(constantMapGroup);
                                        var glyphId = new TreeViewItem { Header = $"Glyph Id - {g.GlyphId}" };
                                        var startCharCode = new TreeViewItem { Header = $"Start Char Code - {g.StartCharCode}" };
                                        var endCharCode = new TreeViewItem { Header = $"End Char Code - {g.EndCharCode}" };
                                        constantMapGroup.Items.Add(glyphId);
                                        constantMapGroup.Items.Add(startCharCode);
                                        constantMapGroup.Items.Add(endCharCode);
                                    });
                                    subtableItem.Items.Add(format13Item);
                                    break;
                                case CmapSubtablesFormat14 format14:
                                    var format14Item = new TreeViewItem { Header = "Format 14" };
                                    languageItem = new TreeViewItem { Header = $"Language - {format14.Language}" };
                                    format14Item.Items.Add(languageItem);
                                    format14.VarSelectorRecords.ForEach(v =>
                                    {
                                        var varSelectorRecord = new TreeViewItem { Header = "Var Selector Record" };
                                        format14Item.Items.Add(varSelectorRecord);
                                        var varSelector = new TreeViewItem { Header = $"Var Selector - {v.VarSelector}" };
                                        format14.VarSelectorRecords.ForEach(r =>
                                        {
                                            if (r.DefaultUvsTableHeader is not null)
                                            {
                                                var defaultUvsTableHeader = new TreeViewItem { Header = "Default UVS Table Header" };
                                                var unicodeRangeRecords = new TreeViewItem { Header = "Unicode Range Records" };
                                                defaultUvsTableHeader.Items.Add(unicodeRangeRecords);
                                                r.DefaultUvsTableHeader.UnicodeRangeRecords.ForEach(u =>
                                                {
                                                    var unicodeRangeRecord = new TreeViewItem { Header = "Unicode Range Record" };
                                                    var startUnicodeValue = new TreeViewItem { Header = $"Start Unicode Value - {u.StartUnicodeValue}" };
                                                    var additionalCount = new TreeViewItem { Header = $"Additional Count - {u.AdditionalCount}" };
                                                    unicodeRangeRecord.Items.Add(startUnicodeValue);
                                                    unicodeRangeRecord.Items.Add(additionalCount);
                                                    unicodeRangeRecords.Items.Add(unicodeRangeRecord);
                                                });
                                                varSelector.Items.Add(defaultUvsTableHeader);
                                            }

                                            if (r.NonDefaultUvsTableHeader is null) return;
                                            var nonDefaultUvsTableHeader = new TreeViewItem { Header = "Non Default UVS Table Header" };
                                            var uvsMappings = new TreeViewItem { Header = "UVS Mappings" };
                                            nonDefaultUvsTableHeader.Items.Add(uvsMappings);
                                            r.NonDefaultUvsTableHeader.UvsMappings.ForEach(u =>
                                            {
                                                var uvsMapping = new TreeViewItem { Header = "UVS Mapping" };
                                                var unicodeValue = new TreeViewItem { Header = $"Unicode Value - {u.UnicodeValue}" };
                                                var glyphId = new TreeViewItem { Header = $"Glyph Id - {u.GlyphId}" };
                                                uvsMapping.Items.Add(unicodeValue);
                                                uvsMapping.Items.Add(glyphId);
                                                uvsMappings.Items.Add(uvsMapping);
                                            });
                                            varSelector.Items.Add(nonDefaultUvsTableHeader);
                                        });
                                        varSelectorRecord.Items.Add(varSelector);
                                    });
                                    subtableItem.Items.Add(format14Item);
                                    break;
                            }
                        });
                        break;
                    case NameTable nameTable:
                        var nameRoot = new TreeViewItem { Header = "name" };
                        ResultView.Items.Add(nameRoot);
                        if (nameTable.LangTagRecords is not null && nameTable.LangTagRecords.Count > 0)
                        {
                            var langTagRecords = new TreeViewItem { Header = "Lang Tag Records" };
                            nameRoot.Items.Add(langTagRecords);
                            nameTable.LangTagRecords.ForEach(l =>
                            {
                                var langTagRecord = new TreeViewItem { Header = "Language Tag Record" };
                                langTagRecords.Items.Add(langTagRecord);
                                var langTag = new TreeViewItem { Header = l.LanguageTag };
                                langTagRecord.Items.Add(langTag);
                            });
                        }
                        var nameRecords = new TreeViewItem { Header = "Name Records" };
                        nameRoot.Items.Add(nameRecords);
                        List<IGrouping<string, NameRecord>> groupedRecords = nameTable.NameRecords.GroupBy(n => n.LanguageId).ToList();
                        groupedRecords.ForEach(r =>
                        {
                            var languageGroup = new TreeViewItem { Header = r.Key };
                            r.ToList().ForEach(n =>
                            {
                                var nameRecord = new TreeViewItem { Header = $"{n.NameId}: {n.Name}" };
                                languageGroup.Items.Add(nameRecord);
                            });
                            nameRecords.Items.Add(languageGroup);
                        });
                        break;
                    case GposTable gposTable:
                        break;
                    case GdefTable gdefTable:
                        break;
                    case GvarTable gvarTable:
                        break;
                    case AvarTable avarTable:
                        break;
                    case BaseTable baseTable:
                        break;
                    case CbdtTable cbdtTable:
                        break;
                    case CblcTable cblcTable:
                        break;
                    case EbdtTable ebdtTable:
                        break;
                    case EbscTable ebscTable:
                        break;
                    case EblcTable eblcTable:
                        break;
                    case Type1Table type1Table:
                        break;
                    case ColrTable colrTable:
                        break;
                    case CpalTable cpalTable:
                        break;
                    case CvarTable cvarTable:
                        break;
                    case FftmTable fftmTable:
                        break;
                    case FvarTable fvarTable:
                        break;
                    case GsubTable gsubTable:
                        break;
                    case HeadTable headTable:
                        var headRoot = new TreeViewItem { Header = "head" };
                        ResultView.Items.Add(headRoot);
                        var headVersion = new TreeViewItem { Header = $"Version: {headTable.Version}" };
                        var headVersion2 = new TreeViewItem { Header = $"Version2: {headTable.MajorVersion}.{headTable.MinorVersion}" };
                        var headMagicNumber = new TreeViewItem { Header = $"Magic Number: {headTable.MagicNumber}" };
                        var headFlags = new TreeViewItem { Header = $"Flags: {headTable.Flags}" };
                        var headCheckSumAdjustment = new TreeViewItem { Header = $"CheckSum Adjustment: {headTable.CheckSumAdjustment}" };
                        var headCreated = new TreeViewItem { Header = $"Created: {headTable.Created}" };
                        var headModified = new TreeViewItem { Header = $"Modified: {headTable.Modified}" };
                        var fontDirectionHint = new TreeViewItem { Header = $"Font Direction Hint: {headTable.FontDirectionHint}" };
                        var fontRevision = new TreeViewItem { Header = $"Font Revision: {headTable.FontRevision}" };
                        var headGlyphDataFormat = new TreeViewItem { Header = $"Glyph Data Format: {headTable.GlyphDataFormat}" };
                        var headIndexToLocFormat = new TreeViewItem { Header = $"Index To Loc Format: {headTable.IndexToLocFormat}" };
                        var headLowestRecPpem = new TreeViewItem { Header = $"Lowest Rec Ppem: {headTable.LowestRecPpem}" };
                        var headMacStyle = new TreeViewItem { Header = $"Mac Style: {headTable.MacStyle}" };
                        var headUnitsPerEm = new TreeViewItem { Header = $"Units Per EM: {headTable.UnitsPerEm}" };
                        var headBounds = new TreeViewItem { Header = $"Bounds: {headTable.XMin}, {headTable.YMin} {headTable.XMax}, {headTable.YMax}" };
                        headRoot.Items.Add(headVersion);
                        headRoot.Items.Add(headVersion2);
                        headRoot.Items.Add(headMagicNumber);
                        headRoot.Items.Add(headFlags);
                        headRoot.Items.Add(headCheckSumAdjustment);
                        headRoot.Items.Add(headCreated);
                        headRoot.Items.Add(headModified);
                        headRoot.Items.Add(fontDirectionHint);
                        headRoot.Items.Add(fontRevision);
                        headRoot.Items.Add(headGlyphDataFormat);
                        headRoot.Items.Add(headIndexToLocFormat);
                        headRoot.Items.Add(headLowestRecPpem);
                        headRoot.Items.Add(headMacStyle);
                        headRoot.Items.Add(headUnitsPerEm);
                        headRoot.Items.Add(headBounds);
                        break;
                    case HmtxTable hmtxTable:
                        break;
                    case HheaTable hheaTable:
                        break;
                    case HvarTable hvarTable:
                        break;
                    case JstfTable jstfTable:
                        break;
                    case KernTable kernTable:
                        break;
                    case MathTable mathTable:
                        break;
                    case MaxPTable maxPTable:
                        break;
                    case MergTable mergTable:
                        break;
                    case MetaTable metaTable:
                        break;
                    case MvarTable mvarTable:
                        break;
                    case DsigTable dsigTable:
                        break;
                    case HdmxTable hdmxTable:
                        break;
                    case LtshTable ltshTable:
                        break;
                }
            });
        }
    }
}