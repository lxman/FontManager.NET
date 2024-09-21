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
using NewFontParser.Tables.Common.ClassDefinition;
using NewFontParser.Tables.Common.CoverageFormat;
using NewFontParser.Tables.Common.GlyphClassDef;
using NewFontParser.Tables.Cpal;
using NewFontParser.Tables.Cvar;
using NewFontParser.Tables.Fftm;
using NewFontParser.Tables.Fvar;
using NewFontParser.Tables.Gdef;
using NewFontParser.Tables.Gpos;
using NewFontParser.Tables.Gpos.LookupSubtables;
using NewFontParser.Tables.Gpos.LookupSubtables.PairPos;
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
using IClassDefinition = NewFontParser.Tables.Common.ClassDefinition.IClassDefinition;

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
            MWindow.Title = dialog.FileName.Split('\\').Last();
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
                                    var languageItem = new TreeViewItem { Header = $"Language: {format0.Language}" };
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
                                    languageItem = new TreeViewItem { Header = $"Language: {format2.Language}" };
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
                                        var firstCode = new TreeViewItem { Header = $"First Code: {sh.FirstCode}" };
                                        var entryCount = new TreeViewItem { Header = $"Entry Count: {sh.EntryCount}" };
                                        var idDelta = new TreeViewItem { Header = $"Id Delta: {sh.IdDelta}" };
                                        var idRangeOffset = new TreeViewItem { Header = $"Id Range Offset: {sh.IdRangeOffset}" };
                                        subHeader.Items.Add(firstCode);
                                        subHeader.Items.Add(entryCount);
                                        subHeader.Items.Add(idDelta);
                                        subHeader.Items.Add(idRangeOffset);
                                    });
                                    subtableItem.Items.Add(format2Item);
                                    break;

                                case CmapSubtablesFormat4 format4:
                                    var format4Item = new TreeViewItem { Header = "Format 4" };
                                    languageItem = new TreeViewItem { Header = $"Language: {format4.Language}" };
                                    format4Item.Items.Add(languageItem);
                                    var entrySelectorItem = new TreeViewItem { Header = $"Entry Selector: {format4.EntrySelector}" };
                                    format4Item.Items.Add(entrySelectorItem);
                                    var rangeShiftItem = new TreeViewItem { Header = $"Range Shift: {format4.RangeShift}" };
                                    format4Item.Items.Add(rangeShiftItem);
                                    var searchRangeItem = new TreeViewItem { Header = $"Search Range: {format4.SearchRange}" };
                                    format4Item.Items.Add(searchRangeItem);
                                    var segCountX2Item = new TreeViewItem { Header = $"Seg Count X2: {format4.SegCountX2}" };
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
                                        var startCode = new TreeViewItem { Header = $"Start Code: {format4.StartCodes[i]}" };
                                        var endCode = new TreeViewItem { Header = $"End Code: {format4.EndCodes[i]}" };
                                        var idDelta = new TreeViewItem { Header = $"Id Delta: {format4.IdDeltas[i]}" };
                                        var idRangeOffset = new TreeViewItem { Header = $"Id Range Offset: {format4.IdRangeOffsets[i]}" };
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
                                    languageItem = new TreeViewItem { Header = $"Language: {format6.Language}" };
                                    format6Item.Items.Add(languageItem);
                                    var firstCode = new TreeViewItem { Header = $"First Code: {format6.FirstCode}" };
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
                                    languageItem = new TreeViewItem { Header = $"Language: {format8.Language}" };
                                    format8Item.Items.Add(languageItem);
                                    var is32Item = new TreeViewItem { Header = "Is 32" };
                                    format8Item.Items.Add(is32Item);
                                    var is32Data = new TreeViewItem { Header = string.Join(", ", format8.Is32) };
                                    is32Item.Items.Add(is32Data);
                                    format8.SequentialMapGroups.ForEach(g =>
                                    {
                                        var sequentialMapGroup = new TreeViewItem { Header = "Sequential Map Group" };
                                        format8Item.Items.Add(sequentialMapGroup);
                                        var glyphId = new TreeViewItem { Header = $"Start Glyph Id: {g.StartGlyphId}" };
                                        var startCharCode = new TreeViewItem { Header = $"Start Char Code: {g.StartCharCode}" };
                                        var endCharCode = new TreeViewItem { Header = $"End Char Code: {g.EndCharCode}" };
                                        sequentialMapGroup.Items.Add(glyphId);
                                        sequentialMapGroup.Items.Add(startCharCode);
                                        sequentialMapGroup.Items.Add(endCharCode);
                                    });
                                    subtableItem.Items.Add(format8Item);
                                    break;

                                case CmapSubtablesFormat10 format10:
                                    var format10Item = new TreeViewItem { Header = "Format 10" };
                                    languageItem = new TreeViewItem { Header = $"Language: {format10.Language}" };
                                    format10Item.Items.Add(languageItem);
                                    var startCharItem = new TreeViewItem { Header = $"Start Char: {format10.StartChar}" };
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
                                    languageItem = new TreeViewItem { Header = $"Language: {format12.Language}" };
                                    format12Item.Items.Add(languageItem);
                                    format12.Groups.ForEach(g =>
                                    {
                                        var sequentialMapGroup = new TreeViewItem { Header = "Sequential Map Group" };
                                        format12Item.Items.Add(sequentialMapGroup);
                                        var glyphId = new TreeViewItem
                                        { Header = $"Start Glyph Id: {g.StartGlyphId}" };
                                        var startCharCode = new TreeViewItem
                                        { Header = $"Start Char Code: {g.StartCharCode}" };
                                        var endCharCode = new TreeViewItem
                                        { Header = $"End Char Code: {g.EndCharCode}" };
                                        sequentialMapGroup.Items.Add(glyphId);
                                        sequentialMapGroup.Items.Add(startCharCode);
                                        sequentialMapGroup.Items.Add(endCharCode);
                                    });
                                    subtableItem.Items.Add(format12Item);
                                    break;

                                case CmapSubtablesFormat13 format13:
                                    var format13Item = new TreeViewItem { Header = "Format 13" };
                                    languageItem = new TreeViewItem { Header = $"Language: {format13.Language}" };
                                    format13Item.Items.Add(languageItem);
                                    format13.Groups.ForEach(g =>
                                    {
                                        var constantMapGroup = new TreeViewItem { Header = "Constant Map Group" };
                                        format13Item.Items.Add(constantMapGroup);
                                        var glyphId = new TreeViewItem { Header = $"Glyph Id: {g.GlyphId}" };
                                        var startCharCode = new TreeViewItem { Header = $"Start Char Code: {g.StartCharCode}" };
                                        var endCharCode = new TreeViewItem { Header = $"End Char Code: {g.EndCharCode}" };
                                        constantMapGroup.Items.Add(glyphId);
                                        constantMapGroup.Items.Add(startCharCode);
                                        constantMapGroup.Items.Add(endCharCode);
                                    });
                                    subtableItem.Items.Add(format13Item);
                                    break;

                                case CmapSubtablesFormat14 format14:
                                    var format14Item = new TreeViewItem { Header = "Format 14" };
                                    languageItem = new TreeViewItem { Header = $"Language: {format14.Language}" };
                                    format14Item.Items.Add(languageItem);
                                    format14.VarSelectorRecords.ForEach(v =>
                                    {
                                        var varSelectorRecord = new TreeViewItem { Header = "Var Selector Record" };
                                        format14Item.Items.Add(varSelectorRecord);
                                        var varSelector = new TreeViewItem { Header = $"Var Selector: {v.VarSelector}" };
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
                                                    var startUnicodeValue = new TreeViewItem { Header = $"Start Unicode Value: {u.StartUnicodeValue}" };
                                                    var additionalCount = new TreeViewItem { Header = $"Additional Count: {u.AdditionalCount}" };
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
                                                var unicodeValue = new TreeViewItem { Header = $"Unicode Value: {u.UnicodeValue}" };
                                                var glyphId = new TreeViewItem { Header = $"Glyph Id: {u.GlyphId}" };
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
                        var gposRoot = new TreeViewItem { Header = "gpos" };
                        ResultView.Items.Add(gposRoot);
                        gposTable.FeatureList.FeatureRecords.ForEach(fr =>
                        {
                            var featureRecord = new TreeViewItem { Header = "Feature Record" };
                            gposRoot.Items.Add(featureRecord);
                            var featureTag = new TreeViewItem { Header = $"Feature Tag: {fr.FeatureTag}" };
                            featureRecord.Items.Add(featureTag);
                            var lookupListIndexes = new TreeViewItem { Header = "Lookup List Indexes" };
                            featureRecord.Items.Add(lookupListIndexes);
                            var lookupListIndexesData = new TreeViewItem { Header = string.Join(", ", fr.FeatureTable.LookupListIndexes) };
                            lookupListIndexes.Items.Add(lookupListIndexesData);
                            if (fr.FeatureTable.FeatureParametersTable is null) return;
                            var featureParametersTable = new TreeViewItem { Header = "Feature Parameters Table" };
                            featureRecord.Items.Add(featureParametersTable);
                            var format = new TreeViewItem { Header = $"Format: {fr.FeatureTable.FeatureParametersTable.Format}" };
                            var numNamedParameters = new TreeViewItem { Header = $"Number of Named Parameters: {fr.FeatureTable.FeatureParametersTable.NumNamedParameters}" };
                            var featureUILabelNamedId = new TreeViewItem { Header = $"Feature UI Named ID: {fr.FeatureTable.FeatureParametersTable.FeatureUILabelNameId}" };
                            var featureUITooltipTextNameId = new TreeViewItem { Header = $"Feature UI Tool Text Name ID: {fr.FeatureTable.FeatureParametersTable.FeatureUITooltipTextNameId}" };
                            var firstParamUILabelNameId = new TreeViewItem { Header = $"First Param UI Label Name ID: {fr.FeatureTable.FeatureParametersTable.FirstParamUILabelNameId}" };
                            var sampleTextNameId = new TreeViewItem { Header = $"Sample Text Name ID: {fr.FeatureTable.FeatureParametersTable.SampleTextNameId}" };
                            var unicodeScalarValues = new TreeViewItem { Header = string.Join(", ", fr.FeatureTable.FeatureParametersTable.UnicodeScalarValues) };
                            featureParametersTable.Items.Add(format);
                            featureParametersTable.Items.Add(numNamedParameters);
                            featureParametersTable.Items.Add(featureUILabelNamedId);
                            featureParametersTable.Items.Add(featureUITooltipTextNameId);
                            featureParametersTable.Items.Add(firstParamUILabelNameId);
                            featureParametersTable.Items.Add(sampleTextNameId);
                            featureParametersTable.Items.Add(unicodeScalarValues);
                        });
                        gposTable.GposLookupList.LookupTables.ForEach(lt =>
                        {
                            lt.SubTables.ForEach(st =>
                            {
                                var subTable = new TreeViewItem { Header = "Lookup SubTable" };
                                gposRoot.Items.Add(subTable);
                                switch (st)
                                {
                                    case SinglePos singlePos:
                                        var format1Item = new TreeViewItem { Header = "Single Pos Format 1" };
                                        subTable.Items.Add(format1Item);
                                        var valueFormat = new TreeViewItem { Header = $"Value Format: {singlePos.ValueFormat}" };
                                        format1Item.Items.Add(valueFormat);
                                        if (singlePos.ValueRecord is not null)
                                        {
                                            format1Item.Items.Add(BuildValueRecord(singlePos.ValueRecord, singlePos.ValueFormat));
                                        }

                                        if (singlePos.ValueRecords is not null)
                                        {
                                            var valueRecords = new TreeViewItem { Header = "Value Records" };
                                            singlePos.ValueRecords.ForEach(vr =>
                                            {
                                                valueRecords.Items.Add(BuildValueRecord(vr, singlePos.ValueFormat));
                                            });
                                        }
                                        format1Item.Items.Add(BuildCoverageItem(singlePos.Coverage));
                                        break;
                                    case PairPosFormat1 pairPosFormat1:
                                        var pairPosFormat1Item = new TreeViewItem { Header = "Pair Pos Format 1" };
                                        subTable.Items.Add(pairPosFormat1Item);
                                        pairPosFormat1Item.FormChild(nameof(pairPosFormat1.ValueFormat1),
                                            pairPosFormat1.ValueFormat1);
                                        pairPosFormat1Item.FormChild(nameof(pairPosFormat1.ValueFormat2),
                                            pairPosFormat1.ValueFormat2);
                                        pairPosFormat1.PairSets.ToList().ForEach(ps =>
                                        {
                                            var pairSet = new TreeViewItem { Header = "Pair Set" };
                                            pairPosFormat1Item.Items.Add(pairSet);
                                            ps.PairValueRecords.ToList().ForEach(pv =>
                                            {
                                                var pairValueRecord = new TreeViewItem { Header = "Pair Value Record" };
                                                pairSet.Items.Add(pairValueRecord);
                                                pairValueRecord.FormChild(nameof(pv.SecondGlyph), pv.SecondGlyph);
                                                pairValueRecord.FormChild(nameof(pv.Value1), pv.Value1);
                                                pairValueRecord.FormChild(nameof(pv.Value2), pv.Value2);
                                            });
                                        });
                                        pairPosFormat1Item.Items.Add(BuildCoverageItem(pairPosFormat1.Coverage));
                                        break;
                                }
                            });
                        });
                        break;

                    case GdefTable gdefTable:
                        var gdefRoot = new TreeViewItem { Header = "gdef" };
                        ResultView.Items.Add(gdefRoot);
                        if (gdefTable.GlyphClassDef is not null)
                        {
                            gdefRoot.Items.Add(BuildGdefClassDefinition(gdefTable.GlyphClassDef));
                        }

                        if (gdefTable.MarkAttachClassDef is not null)
                        {
                            gdefRoot.Items.Add(BuildGdefClassDefinition(gdefTable.MarkAttachClassDef));
                        }

                        if (gdefTable.AttachListTable is not null)
                        {
                            gdefRoot.FormChild(nameof(gdefTable.AttachListTable.AttachPointOffsets),
                                string.Join(", ", gdefTable.AttachListTable.AttachPointOffsets));
                            gdefRoot.Items.Add(BuildCoverageItem(gdefTable.AttachListTable.Coverage));
                        }
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
                        headRoot.FormChild(nameof(headTable.Version), headTable.Version);
                        headRoot.FormChild("Version2", $"{headTable.MajorVersion}.{headTable.MinorVersion}");
                        headRoot.FormChild(nameof(headTable.MagicNumber), headTable.MagicNumber);
                        headRoot.FormChild(nameof(headTable.Flags), headTable.Flags);
                        headRoot.FormChild(nameof(headTable.CheckSumAdjustment), headTable.CheckSumAdjustment);
                        headRoot.FormChild(nameof(headTable.Created), headTable.Created);
                        headRoot.FormChild(nameof(headTable.Modified), headTable.Modified);
                        headRoot.FormChild(nameof(headTable.FontDirectionHint), headTable.FontDirectionHint);
                        headRoot.FormChild(nameof(headTable.FontRevision), headTable.FontRevision);
                        headRoot.FormChild(nameof(headTable.GlyphDataFormat), headTable.GlyphDataFormat);
                        headRoot.FormChild(nameof(headTable.IndexToLocFormat), headTable.IndexToLocFormat);
                        headRoot.FormChild(nameof(headTable.LowestRecPpem), headTable.LowestRecPpem);
                        headRoot.FormChild(nameof(headTable.MacStyle), headTable.MacStyle);
                        headRoot.FormChild(nameof(headTable.UnitsPerEm), headTable.UnitsPerEm);
                        headRoot.FormChild("Bounds", $"{headTable.XMin}, {headTable.YMin} {headTable.XMax}, {headTable.YMax}");
                        break;

                    case HmtxTable hmtxTable:
                        break;

                    case HheaTable hheaTable:
                        break;

                    case HvarTable hvarTable:
                        break;

                    case JstfTable jstfTable:
                        var jstfRoot = new TreeViewItem { Header = "jstf" };
                        ResultView.Items.Add(jstfRoot);
                        jstfTable.ScriptRecords.ForEach(sr =>
                        {
                            TreeViewItem scriptRecord = jstfRoot.FormChild("Script Record");
                            scriptRecord.FormChild("Tag", sr.Tag);
                            TreeViewItem jstfScript = scriptRecord.FormChild("Jstf Script");
                            if (sr.JstfScript.ExtenderGlyph is not null)
                            {
                                TreeViewItem extenderGlyph = jstfScript.FormChild("Extender Glyph");
                                extenderGlyph.FormChild(nameof(sr.JstfScript.ExtenderGlyph.ExtenderGlyphs),
                                    string.Join(", ", sr.JstfScript.ExtenderGlyph.ExtenderGlyphs));
                            }

                            TreeViewItem prioritiesHeader = scriptRecord.FormChild("Priorities");
                            sr.JstfScript.DefaultJstfLangSys?.JstfPriorities.ForEach(p =>
                            {
                                if (p.GposExtensionDisable is not null)
                                {
                                    TreeViewItem gposExtensionDisable = prioritiesHeader.FormChild("Gpos Extension Disable", p.GposExtensionDisable);
                                    gposExtensionDisable.FormChild("Lookup Indices",
                                        string.Join(", ", p.GposExtensionDisable.GsubLookupIndices));
                                }

                                if (p.GposExtensionEnable is not null)
                                {
                                    TreeViewItem gposExtensionEnable = prioritiesHeader.FormChild("Gpos Extension Enable", p.GposExtensionEnable);
                                    gposExtensionEnable.FormChild("Lookup Indices",
                                        string.Join(", ", p.GposExtensionEnable.GsubLookupIndices));
                                }

                                if (p.ExtensionJstfMax is not null)
                                {
                                    prioritiesHeader.FormChild(nameof(p.ExtensionJstfMax), string.Join(", ", p.ExtensionJstfMax));
                                }

                                if (p.GposShrinkageDisable is not null)
                                {
                                    TreeViewItem gposShrinkageDisable = prioritiesHeader.FormChild("Gpos Shrinkage Disable", p.GposShrinkageDisable);
                                    gposShrinkageDisable.FormChild("Lookup Indices", string.Join(", ", p.GposShrinkageDisable.GsubLookupIndices));
                                }

                                if (p.GposShrinkageEnable is not null)
                                {
                                    TreeViewItem gposShrinkageEnable = prioritiesHeader.FormChild("Gpos Shrinkage Enable", p.GposShrinkageEnable);
                                    gposShrinkageEnable.FormChild("Lookup Indices", string.Join(", ", p.GposShrinkageEnable.GsubLookupIndices));
                                }
                            });

                            sr.JstfScript.JstfLangSysRecords.ForEach(ls =>
                            {
                                TreeViewItem langSysRecord = jstfScript.FormChild("LangSys Record", ls.LangSysTag);
                                langSysRecord.FormChild("Tag", ls.LangSysTag);
                                langSysRecord.FormChild("Priorities", string.Join(", ", ls.JstfLangSys.JstfPriorities));
                            });
                        });
                        break;

                    case KernTable kernTable:
                        var kernRoot = new TreeViewItem { Header = "kern" };
                        ResultView.Items.Add(kernRoot);
                        kernRoot.FormChild(nameof(kernTable.Version), kernTable.Version);
                        kernTable.Subtables.ForEach(st =>
                        {
                            switch (st)
                            {
                                case KernSubtableFormat0 format0:
                                    var format0Item = new TreeViewItem { Header = "Format 0" };
                                    format0Item.FormChild(nameof(format0.Coverage), format0.Coverage);
                                    var kerningPairs = new TreeViewItem { Header = "Kerning Pairs" };
                                    format0Item.Items.Add(kerningPairs);
                                    format0.KernPairs.ForEach(kp =>
                                    {
                                        var kerningPair = new TreeViewItem { Header = "Kerning Pair" };
                                        kerningPairs.Items.Add(kerningPair);
                                        kerningPair.FormChild(nameof(kp.Left), kp.Left);
                                        kerningPair.FormChild(nameof(kp.Right), kp.Right);
                                        kerningPair.FormChild(nameof(kp.Value), kp.Value);
                                    });
                                    kernRoot.Items.Add(format0Item);
                                    break;
                                case KernSubtableFormat2 format2:
                                    var format2Item = new TreeViewItem { Header = "Format 2" };
                                    format2Item.FormChild(nameof(format2.Version), format2.Version);
                                    format2Item.FormChild(nameof(format2.Coverage), format2.Coverage);
                                    kernRoot.Items.Add(format2Item);
                                    break;
                            }
                        });
                        break;

                    case MathTable mathTable:
                        break;

                    case MaxPTable maxPTable:
                        var maxPRoot = new TreeViewItem { Header = "maxp" };
                        ResultView.Items.Add(maxPRoot);
                        maxPRoot.FormChild(nameof(maxPTable.Version), maxPTable.Version);
                        maxPRoot.FormChild(nameof(maxPTable.MaxStorage), maxPTable.MaxStorage);
                        maxPRoot.FormChild(nameof(maxPTable.MaxPoints), maxPTable.MaxPoints);
                        maxPRoot.FormChild(nameof(maxPTable.MaxContours), maxPTable.MaxContours);
                        maxPRoot.FormChild(nameof(maxPTable.MaxSizeOfInstructions), maxPTable.MaxSizeOfInstructions);
                        maxPRoot.FormChild(nameof(maxPTable.MaxInstructionDefs), maxPTable.MaxInstructionDefs);
                        maxPRoot.FormChild(nameof(maxPTable.MaxFunctionDefs), maxPTable.MaxFunctionDefs);
                        maxPRoot.FormChild(nameof(maxPTable.MaxTwilightPoints), maxPTable.MaxTwilightPoints);
                        maxPRoot.FormChild(nameof(maxPTable.MaxComponentDepth), maxPTable.MaxComponentDepth);
                        maxPRoot.FormChild(nameof(maxPTable.MaxComponentElements), maxPTable.MaxComponentElements);
                        maxPRoot.FormChild(nameof(maxPTable.MaxCompositeContours), maxPTable.MaxCompositeContours);
                        maxPRoot.FormChild(nameof(maxPTable.MaxCompositePoints), maxPTable.MaxCompositePoints);
                        maxPRoot.FormChild(nameof(maxPTable.MaxStackElements), maxPTable.MaxStackElements);
                        maxPRoot.FormChild(nameof(maxPTable.MaxZones), maxPTable.MaxZones);
                        maxPRoot.FormChild(nameof(maxPTable.NumGlyphs), maxPTable.NumGlyphs);
                        break;

                    case MergTable mergTable:
                        var mergRoot = new TreeViewItem { Header = "merg" };
                        ResultView.Items.Add(mergRoot);
                        var version = new TreeViewItem { Header = $"Version: {mergTable.Version}" };
                        mergRoot.Items.Add(version);
                        var classDefinitions = new TreeViewItem { Header = "Class Definitions" };
                        mergRoot.Items.Add(classDefinitions);
                        mergTable.ClassDefinitions.ForEach(cd =>
                        {
                            classDefinitions.Items.Add(BuildCommonClassDefinition(cd));
                        });
                        break;

                    case MetaTable metaTable:
                        var metaRoot = new TreeViewItem { Header = "meta" };
                        ResultView.Items.Add(metaRoot);
                        var dataMaps = new TreeViewItem { Header = "Data Maps" };
                        metaRoot.Items.Add(dataMaps);
                        metaTable.DataMaps.ForEach(dm =>
                        {
                            var dataMap = new TreeViewItem { Header = "Data Map" };
                            var tag = new TreeViewItem { Header = $"Tag: {dm.Tag}" };
                            var data = new TreeViewItem { Header = $"Data: {dm.Data}" };
                            dataMaps.Items.Add(dataMap);
                            dataMap.Items.Add(tag);
                            dataMap.Items.Add(data);
                        });
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

        private static TreeViewItem BuildCoverageItem(ICoverageFormat coverage)
        {
            var coverageItem = new TreeViewItem { Header = "Coverage" };
            switch (coverage)
            {
                case CoverageFormat1 cf1:
                    coverageItem.Header = "Coverage Format 1";
                    TreeViewItem glyphArrayHeader = coverageItem.FormChild("Glyph Array");
                    glyphArrayHeader.FormChild(string.Join(", ", cf1.GlyphArray));
                    break;
                case CoverageFormat2 cf2:
                    coverageItem.Header = "Coverage Format 2";
                    cf2.RangeRecords.ToList().ForEach(rr =>
                    {
                        TreeViewItem rangeRecordHeader = coverageItem.FormChild("Range Record");
                        rangeRecordHeader.FormChild(nameof(rr.Start), rr.Start);
                        rangeRecordHeader.FormChild(nameof(rr.End), rr.End);
                        rangeRecordHeader.FormChild(nameof(rr.StartCoverageIndex), rr.StartCoverageIndex);
                    });
                    break;
            }
            return coverageItem;
        }

        private static TreeViewItem BuildCommonClassDefinition(IClassDefinition classDefinition)
        {
            var classDefinitionItem = new TreeViewItem { Header = "Class Definition" };
            switch (classDefinition)
            {
                case ClassDefinitionFormat1 format1:
                    var format1Item = new TreeViewItem { Header = "Format 1" };
                    format1Item.FormChild(nameof(format1.StartGlyph), format1.StartGlyph);
                    format1Item.FormChild(nameof(format1.ClassValues), string.Join(", ", format1.ClassValues));
                    format1.ClassValues.ForEach(cv =>
                    {
                        var classValue = new TreeViewItem { Header = $"Class Value: {cv}" };
                        format1Item.Items.Add(classValue);
                    });
                    classDefinitionItem.Items.Add(format1Item);
                    break;
                case ClassDefinitionFormat2 format2:
                    var format2Item = new TreeViewItem { Header = "Format 2" };
                    var classRangeRecords = new TreeViewItem { Header = "Class Range Records" };
                    format2Item.Items.Add(classRangeRecords);
                    format2.ClassRanges.ForEach(crr =>
                    {
                        var classRangeRecord = new TreeViewItem { Header = "Class Range Record" };
                        classRangeRecords.Items.Add(classRangeRecord);
                        var startClass = new TreeViewItem { Header = $"Start Class: {crr.Class}" };
                        var beginGlyph = new TreeViewItem { Header = $"Start Glyph: {crr.StartGlyphID}" };
                        var endGlyph = new TreeViewItem { Header = $"End Glyph: {crr.EndGlyphID}" };
                        classRangeRecord.Items.Add(startClass);
                        classRangeRecord.Items.Add(beginGlyph);
                        classRangeRecord.Items.Add(endGlyph);
                    });
                    classDefinitionItem.Items.Add(format2Item);
                    break;
            }
            return classDefinitionItem;
        }

        private static TreeViewItem BuildGdefClassDefinition(NewFontParser.Tables.Gdef.IClassDefinition classDefinition)
        {
            var classDefinitionItem = new TreeViewItem { Header = "Class Definition" };
            switch (classDefinition)
            {
                case ClassDefinition1 def1:
                    classDefinitionItem.Header = "Class Definition Format 1";
                    classDefinitionItem.FormChild(nameof(def1.StartGlyph), def1.StartGlyph);
                    classDefinitionItem.FormChild(nameof(def1.Classes), string.Join(", ", def1.Classes));
                    break;
                case ClassDefinition2 def2:
                    classDefinitionItem.Header = "Class Definition Format 2";
                    def2.ClassRangeRecords.ForEach(cr =>
                    {
                        var classRange = new TreeViewItem { Header = "Class Range" };
                        classRange.FormChild(nameof(cr.StartGlyphId), cr.StartGlyphId);
                        classRange.FormChild(nameof(cr.EndGlyphId), cr.EndGlyphId);
                        classRange.FormChild(nameof(cr.GlyphClass), cr.GlyphClass);
                        classDefinitionItem.Items.Add(classRange);
                    });
                    break;
            }

            return classDefinitionItem;
        }

        private static TreeViewItem BuildValueRecord(NewFontParser.Tables.Common.ValueRecord valueRecord, ValueFormat format)
        {
            var valueRecordItem = new TreeViewItem { Header = "Value Record" };
            if (format.HasFlag(ValueFormat.XAdvance)) valueRecordItem.FormChild(nameof(valueRecord.XAdvance), valueRecord.XAdvance!.Value);
            if (format.HasFlag(ValueFormat.YAdvance)) valueRecordItem.FormChild(nameof(valueRecord.YAdvance), valueRecord.YAdvance!.Value);
            if (format.HasFlag(ValueFormat.XPlacement)) valueRecordItem.FormChild(nameof(valueRecord.XPlacement), valueRecord.XPlacement!.Value);
            if (format.HasFlag(ValueFormat.YPlacement)) valueRecordItem.FormChild(nameof(valueRecord.YPlacement), valueRecord.YPlacement!.Value);
            if (format.HasFlag(ValueFormat.XPlacementDevice)) valueRecordItem.FormChild(nameof(valueRecord.XPlaDeviceOffset), valueRecord.XPlaDeviceOffset!.Value);
            if (format.HasFlag(ValueFormat.YPlacementDevice)) valueRecordItem.FormChild(nameof(valueRecord.YPlaDeviceLength), valueRecord.YPlaDeviceLength!.Value);
            if (format.HasFlag(ValueFormat.XAdvanceDevice)) valueRecordItem.FormChild(nameof(valueRecord.XAdvDeviceOffset), valueRecord.XAdvDeviceOffset!.Value);
            if (format.HasFlag(ValueFormat.YAdvanceDevice)) valueRecordItem.FormChild(nameof(valueRecord.YAdvDeviceLength), valueRecord.YAdvDeviceLength!.Value);
            return valueRecordItem;
        }
    }
}