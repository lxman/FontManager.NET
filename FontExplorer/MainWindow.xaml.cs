﻿using System.Text;
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
using NewFontParser.Tables.Common.ChainedSequenceContext;
using NewFontParser.Tables.Common.ChainedSequenceContext.Format1;
using NewFontParser.Tables.Common.ChainedSequenceContext.Format2;
using NewFontParser.Tables.Common.ChainedSequenceContext.Format3;
using NewFontParser.Tables.Common.SequenceContext;
using NewFontParser.Tables.Common.SequenceContext.Format1;
using NewFontParser.Tables.Common.SequenceContext.Format2;
using NewFontParser.Tables.Common.SequenceContext.Format3;
using NewFontParser.Tables.Cpal;
using NewFontParser.Tables.Cvar;
using NewFontParser.Tables.Fftm;
using NewFontParser.Tables.Fvar;
using NewFontParser.Tables.Gdef;
using NewFontParser.Tables.Gpos;
using NewFontParser.Tables.Gpos.LookupSubtables;
using NewFontParser.Tables.Gpos.LookupSubtables.CursivePos;
using NewFontParser.Tables.Gpos.LookupSubtables.MarkBasePos;
using NewFontParser.Tables.Gpos.LookupSubtables.MarkLigPos;
using NewFontParser.Tables.Gpos.LookupSubtables.MarkMarkPos;
using NewFontParser.Tables.Gpos.LookupSubtables.PairPos;
using NewFontParser.Tables.Gsub;
using NewFontParser.Tables.Gsub.LookupSubTables.AlternateSubstitution;
using NewFontParser.Tables.Gsub.LookupSubTables.LigatureSubstitution;
using NewFontParser.Tables.Gsub.LookupSubTables.MultipleSubstitution;
using NewFontParser.Tables.Gsub.LookupSubTables.ReverseChainSingleSubstitution;
using NewFontParser.Tables.Gsub.LookupSubTables.SingleSubstitution;
using NewFontParser.Tables.Gsub.LookupSubTables.SubstitutionExtension;
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

namespace FontExplorer;

/// <summary>
///     Interaction logic for MainWindow.xaml
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
        if (!ofdResult) return;
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
                                var glyphIdArrayData = new TreeViewItem
                                    { Header = string.Join(", ", format0.GlyphIndexArray) };
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
                                glyphIdArrayData = new TreeViewItem
                                    { Header = string.Join(", ", format2.GlyphIndexArray) };
                                glyphIdArray.Items.Add(glyphIdArrayData);
                                var subHeaderKeys = new TreeViewItem { Header = "Sub Header Keys" };
                                format2Item.Items.Add(subHeaderKeys);
                                var subHeaderKeysData = new TreeViewItem
                                    { Header = string.Join(", ", format2.SubHeaderKeys) };
                                subHeaderKeys.Items.Add(subHeaderKeysData);
                                format2.SubHeaders.ForEach(sh =>
                                {
                                    var subHeader = new TreeViewItem { Header = "Sub Header" };
                                    format2Item.Items.Add(subHeader);
                                    var firstCode = new TreeViewItem { Header = $"First Code: {sh.FirstCode}" };
                                    var entryCount = new TreeViewItem { Header = $"Entry Count: {sh.EntryCount}" };
                                    var idDelta = new TreeViewItem { Header = $"Id Delta: {sh.IdDelta}" };
                                    var idRangeOffset = new TreeViewItem
                                        { Header = $"Id Range Offset: {sh.IdRangeOffset}" };
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
                                var entrySelectorItem = new TreeViewItem
                                    { Header = $"Entry Selector: {format4.EntrySelector}" };
                                format4Item.Items.Add(entrySelectorItem);
                                var rangeShiftItem = new TreeViewItem { Header = $"Range Shift: {format4.RangeShift}" };
                                format4Item.Items.Add(rangeShiftItem);
                                var searchRangeItem = new TreeViewItem
                                    { Header = $"Search Range: {format4.SearchRange}" };
                                format4Item.Items.Add(searchRangeItem);
                                var segCountX2Item = new TreeViewItem
                                    { Header = $"Seg Count X2: {format4.SegCountX2}" };
                                format4Item.Items.Add(segCountX2Item);
                                if (format4.GlyphIdArray.Count > 0)
                                {
                                    glyphIdArray.Items.Clear();
                                    glyphIdArray = new TreeViewItem { Header = "Glyph Index Array" };
                                    format4Item.Items.Add(glyphIdArray);
                                    glyphIdArrayData = new TreeViewItem
                                        { Header = string.Join(", ", format4.GlyphIdArray) };
                                    glyphIdArray.Items.Add(glyphIdArrayData);
                                }

                                var rangesItem = new TreeViewItem { Header = "Ranges" };
                                for (var i = 0; i < format4.StartCodes.Count; i++)
                                {
                                    var rangeItem = new TreeViewItem { Header = "Range" };
                                    rangesItem.Items.Add(rangeItem);
                                    var startCode = new TreeViewItem
                                        { Header = $"Start Code: {format4.StartCodes[i]}" };
                                    var endCode = new TreeViewItem { Header = $"End Code: {format4.EndCodes[i]}" };
                                    var idDelta = new TreeViewItem { Header = $"Id Delta: {format4.IdDeltas[i]}" };
                                    var idRangeOffset = new TreeViewItem
                                        { Header = $"Id Range Offset: {format4.IdRangeOffsets[i]}" };
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
                                var glyphIndexArrayData = new TreeViewItem
                                    { Header = string.Join(", ", format6.GlyphIndexArray) };
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
                                    var startCharCode = new TreeViewItem
                                        { Header = $"Start Char Code: {g.StartCharCode}" };
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
                                glyphIndexArrayData = new TreeViewItem
                                    { Header = string.Join(", ", format10.GlyphIndexArray) };
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
                                    var startCharCode = new TreeViewItem
                                        { Header = $"Start Char Code: {g.StartCharCode}" };
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
                                            var defaultUvsTableHeader = new TreeViewItem
                                                { Header = "Default UVS Table Header" };
                                            var unicodeRangeRecords = new TreeViewItem
                                                { Header = "Unicode Range Records" };
                                            defaultUvsTableHeader.Items.Add(unicodeRangeRecords);
                                            r.DefaultUvsTableHeader.UnicodeRangeRecords.ForEach(u =>
                                            {
                                                var unicodeRangeRecord = new TreeViewItem
                                                    { Header = "Unicode Range Record" };
                                                var startUnicodeValue = new TreeViewItem
                                                    { Header = $"Start Unicode Value: {u.StartUnicodeValue}" };
                                                var additionalCount = new TreeViewItem
                                                    { Header = $"Additional Count: {u.AdditionalCount}" };
                                                unicodeRangeRecord.Items.Add(startUnicodeValue);
                                                unicodeRangeRecord.Items.Add(additionalCount);
                                                unicodeRangeRecords.Items.Add(unicodeRangeRecord);
                                            });
                                            varSelector.Items.Add(defaultUvsTableHeader);
                                        }

                                        if (r.NonDefaultUvsTableHeader is null) return;
                                        var nonDefaultUvsTableHeader = new TreeViewItem
                                            { Header = "Non Default UVS Table Header" };
                                        var uvsMappings = new TreeViewItem { Header = "UVS Mappings" };
                                        nonDefaultUvsTableHeader.Items.Add(uvsMappings);
                                        r.NonDefaultUvsTableHeader.UvsMappings.ForEach(u =>
                                        {
                                            var uvsMapping = new TreeViewItem { Header = "UVS Mapping" };
                                            var unicodeValue = new TreeViewItem
                                                { Header = $"Unicode Value: {u.UnicodeValue}" };
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
                    List<IGrouping<string, NameRecord>> groupedRecords =
                        nameTable.NameRecords.GroupBy(n => n.LanguageId).ToList();
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
                    TreeViewItem frRoot = gposRoot.FormChild("Feature Records");
                    gposTable.FeatureList.FeatureRecords.ForEach(fr =>
                    {
                        TreeViewItem featureRecord = frRoot.FormChild("Feature Record");
                        var featureTag = new TreeViewItem { Header = $"Feature Tag: {fr.FeatureTag}" };
                        featureRecord.Items.Add(featureTag);
                        var lookupListIndexes = new TreeViewItem { Header = "Lookup List Indexes" };
                        featureRecord.Items.Add(lookupListIndexes);
                        var lookupListIndexesData = new TreeViewItem
                            { Header = string.Join(", ", fr.FeatureTable.LookupListIndexes) };
                        lookupListIndexes.Items.Add(lookupListIndexesData);
                        if (fr.FeatureTable.FeatureParametersTable is null) return;
                        var featureParametersTable = new TreeViewItem { Header = "Feature Parameters Table" };
                        featureRecord.Items.Add(featureParametersTable);
                        var format = new TreeViewItem
                            { Header = $"Format: {fr.FeatureTable.FeatureParametersTable.Format}" };
                        var numNamedParameters = new TreeViewItem
                        {
                            Header =
                                $"Number of Named Parameters: {fr.FeatureTable.FeatureParametersTable.NumNamedParameters}"
                        };
                        var featureUILabelNamedId = new TreeViewItem
                        {
                            Header =
                                $"Feature UI Named ID: {fr.FeatureTable.FeatureParametersTable.FeatureUILabelNameId}"
                        };
                        var featureUITooltipTextNameId = new TreeViewItem
                        {
                            Header =
                                $"Feature UI Tool Text Name ID: {fr.FeatureTable.FeatureParametersTable.FeatureUITooltipTextNameId}"
                        };
                        var firstParamUILabelNameId = new TreeViewItem
                        {
                            Header =
                                $"First Param UI Label Name ID: {fr.FeatureTable.FeatureParametersTable.FirstParamUILabelNameId}"
                        };
                        var sampleTextNameId = new TreeViewItem
                        {
                            Header = $"Sample Text Name ID: {fr.FeatureTable.FeatureParametersTable.SampleTextNameId}"
                        };
                        var unicodeScalarValues = new TreeViewItem
                            { Header = string.Join(", ", fr.FeatureTable.FeatureParametersTable.UnicodeScalarValues) };
                        featureParametersTable.Items.Add(format);
                        featureParametersTable.Items.Add(numNamedParameters);
                        featureParametersTable.Items.Add(featureUILabelNamedId);
                        featureParametersTable.Items.Add(featureUITooltipTextNameId);
                        featureParametersTable.Items.Add(firstParamUILabelNameId);
                        featureParametersTable.Items.Add(sampleTextNameId);
                        featureParametersTable.Items.Add(unicodeScalarValues);
                    });
                    TreeViewItem subTables = gposRoot.FormChild("Subtables");
                    gposTable.GposLookupList.LookupTables.ForEach(lt =>
                    {
                        lt.SubTables.ForEach(st =>
                        {
                            switch (st)
                            {
                                case SinglePos singlePos:
                                    TreeViewItem spHeader = subTables.FormChild("Single Pos Format 1");
                                    if (singlePos.ValueRecord is not null)
                                        spHeader.Items.Add(Utilities.BuildCommonValueRecord(singlePos.ValueRecord,
                                            singlePos.ValueFormat));

                                    if (singlePos.ValueRecords is not null)
                                    {
                                        var valueRecords = new TreeViewItem { Header = "Value Records" };
                                        singlePos.ValueRecords.ForEach(vr =>
                                        {
                                            valueRecords.Items.Add(
                                                Utilities.BuildCommonValueRecord(vr, singlePos.ValueFormat));
                                        });
                                    }

                                    if (singlePos.Coverage is not null)
                                        spHeader.Items.Add(Utilities.BuildCommonCoverageItem(singlePos.Coverage));
                                    break;
                                case PairPosFormat1 pairPosFormat1:
                                    TreeViewItem ppf1Header = subTables.FormChild("Pair Pos Format 1");
                                    ppf1Header.FormChild(nameof(pairPosFormat1.ValueFormat1),
                                        pairPosFormat1.ValueFormat1);
                                    ppf1Header.FormChild(nameof(pairPosFormat1.ValueFormat2),
                                        pairPosFormat1.ValueFormat2);
                                    pairPosFormat1.PairSets.ToList().ForEach(ps =>
                                    {
                                        var pairSet = new TreeViewItem { Header = "Pair Set" };
                                        ppf1Header.Items.Add(pairSet);
                                        ps.PairValueRecords.ToList().ForEach(pv =>
                                        {
                                            var pairValueRecord = new TreeViewItem { Header = "Pair Value Record" };
                                            pairSet.Items.Add(pairValueRecord);
                                            pairValueRecord.FormChild(nameof(pv.SecondGlyph), pv.SecondGlyph);
                                            if (pairPosFormat1.ValueFormat1 != 0)
                                            {
                                                TreeViewItem pairValueRecord1 = Utilities.BuildCommonValueRecord(pv.Value1, pairPosFormat1.ValueFormat1);
                                                pairValueRecord1.Header = "Pair Value Record 1";
                                                pairValueRecord.Items.Add(pairValueRecord1);
                                            }
                                            if (pairPosFormat1.ValueFormat2 == 0) return;
                                            TreeViewItem pairValueRecord2 = Utilities.BuildCommonValueRecord(pv.Value2, pairPosFormat1.ValueFormat2);
                                            pairValueRecord2.Header = "Pair Value Record 2";
                                            pairValueRecord.Items.Add(pairValueRecord2);
                                        });
                                    });
                                    ppf1Header.Items.Add(Utilities.BuildCommonCoverageItem(pairPosFormat1.Coverage));
                                    break;
                                case PairPosFormat2 pairPosFormat2:
                                    TreeViewItem ppf2Header = subTables.FormChild("Pair Pos Format 2");
                                    ppf2Header.FormChild(nameof(pairPosFormat2.ValueFormat1), pairPosFormat2.ValueFormat1);
                                    ppf2Header.FormChild(nameof(pairPosFormat2.ValueFormat2), pairPosFormat2.ValueFormat2);
                                    ppf2Header.Items.Add(Utilities.BuildCommonCoverageItem(pairPosFormat2.Coverage));
                                    ppf2Header.Items.Add(Utilities.BuildCommonClassDefinition(pairPosFormat2.ClassDef1));
                                    ppf2Header.Items.Add(Utilities.BuildGdefClassDefinition(pairPosFormat2.ClassDef2));
                                    TreeViewItem c1Records = ppf2Header.FormChild("Class 1 Records");
                                    pairPosFormat2.Class1Records.ForEach(c1R =>
                                    {
                                        TreeViewItem c2Records = c1Records.FormChild("Class 2 Records");
                                        c1R.Class2Records.ForEach(c2R =>
                                        {
                                            c2Records.Items.Add(Utilities.BuildCommonValueRecord(c2R.ValueRecord1, pairPosFormat2.ValueFormat1));
                                            c2Records.Items.Add(Utilities.BuildCommonValueRecord(c2R.ValueRecord2, pairPosFormat2.ValueFormat2));
                                        });
                                    });
                                    break;
                                case CursivePosFormat1 cpf1:
                                    TreeViewItem cpf1Header = subTables.FormChild("Cursive Pos Format 1");
                                    cpf1Header.Items.Add(Utilities.BuildCommonCoverageItem(cpf1.Coverage));
                                    TreeViewItem entryExitRecords = cpf1Header.FormChild("Entry Exit Records");
                                    cpf1.EntryExitRecords.ForEach(r =>
                                    {
                                        if (r.EntryAnchorTable is not null)
                                        {
                                            entryExitRecords.Items.Add(Utilities.BuildAnchorTable(r.EntryAnchorTable));
                                        }

                                        if (r.ExitAnchorTable is not null)
                                        {
                                            entryExitRecords.Items.Add(Utilities.BuildAnchorTable(r.ExitAnchorTable));
                                        }
                                    });
                                    break;
                                case MarkBasePosFormat1 mbp1:
                                    TreeViewItem mbp1Header = subTables.FormChild("Mark Base Pos Format 1");
                                    mbp1Header.Items.Add(Utilities.BuildCommonCoverageItem(mbp1.BaseCoverage));
                                    TreeViewItem baseArray = mbp1Header.FormChild("Base Array");
                                    mbp1.BaseArray.BaseRecords.ToList().ForEach(br =>
                                    {
                                        baseArray.FormChild(nameof(br.BaseAnchorOffsets), string.Join(", ", br.BaseAnchorOffsets));
                                    });
                                    mbp1Header.Items.Add(Utilities.BuildCommonCoverageItem(mbp1.MarkCoverage));
                                    mbp1Header.Items.Add(Utilities.BuildMarkArray(mbp1.MarkArray));
                                    break;
                                case MarkLigPosFormat1 mlp1:
                                    TreeViewItem mlp1Header = subTables.FormChild("Mark Lig Pos Format 1");
                                    mlp1Header.Items.Add(Utilities.BuildCommonCoverageItem(mlp1.MarkCoverage));
                                    mlp1Header.Items.Add(Utilities.BuildMarkArray(mlp1.MarkArray));
                                    mlp1Header.Items.Add(Utilities.BuildCommonCoverageItem(mlp1.LigatureCoverage));
                                    TreeViewItem latHeader = mlp1Header.FormChild("Ligature Array");
                                    if (mlp1.LigatureArrayTable is not null)
                                    {
                                        TreeViewItem larrtHeader = latHeader.FormChild("Ligature Attach Tables");
                                        mlp1.LigatureArrayTable.LigatureAttachTables.ForEach(la =>
                                        {
                                            TreeViewItem laHeader = larrtHeader.FormChild("Ligature Attach Table");
                                            la.LigatureAnchors.ForEach(cr =>
                                            {
                                                laHeader.FormChild(nameof(cr.LigatureAnchorOffsets), string.Join(", ", cr.LigatureAnchorOffsets));
                                            });
                                        });
                                    }
                                    break;
                                case MarkMarkPosFormat1 mmpf1:
                                    TreeViewItem mmpf1Header = subTables.FormChild("Mark Mark Pos Format 1");
                                    mmpf1Header.Items.Add(Utilities.BuildCommonCoverageItem(mmpf1.MarkCoverage));
                                    mmpf1Header.Items.Add(Utilities.BuildMarkArray(mmpf1.MarkArray));
                                    mmpf1Header.Items.Add(Utilities.BuildCommonCoverageItem(mmpf1.Mark2Coverage));
                                    mmpf1Header.Items.Add(Utilities.BuildMark2Array(mmpf1.Mark2Array));
                                    break;
                                case ISequenceContext sc:
                                    TreeViewItem scHeader = subTables.FormChild("Sequence Context");
                                    switch (sc)
                                    {
                                        case SequenceContextFormat1 scf1:
                                            scHeader.Items.Add(Utilities.BuildCommonCoverageItem(scf1.Coverage));
                                            TreeViewItem srsHeader = scHeader.FormChild(nameof(scf1.SequenceRuleSets));
                                            scf1.SequenceRuleSets.ToList().ForEach(srs =>
                                            {
                                                srs.SequenceRules.ToList().ForEach(sr =>
                                                {
                                                    srsHeader.FormChild(nameof(sr.GlyphIds), string.Join(", ", sr.GlyphIds));
                                                });
                                            });
                                            break;
                                        case SequenceContextFormat2 scf2:
                                            scHeader.Items.Add(Utilities.BuildCommonCoverageItem(scf2.Coverage));
                                            scHeader.Items.Add(Utilities.BuildCommonClassDefinition(scf2.ClassDef));
                                            TreeViewItem csrsHeader = scHeader.FormChild(nameof(scf2.ClassSequenceRuleSets));
                                            scf2.ClassSequenceRuleSets.ForEach(csrs =>
                                            {
                                                TreeViewItem csrHeader = csrsHeader.FormChild("Class Sequence Rules");
                                                csrs.ClassSeqRules.ToList().ForEach(csr =>
                                                {
                                                    if (csr.InputSequences is null && csr.SequenceLookups.Count == 0) return;
                                                    if (csr.InputSequences is not null)
                                                    {
                                                        TreeViewItem csRuleHeader = csrHeader.FormChild("Class Sequence Rule");
                                                        csRuleHeader.FormChild(nameof(csr.InputSequences), string.Join(", ", csr.InputSequences));
                                                    }
                                                    if (csr.SequenceLookups.Count == 0) return;
                                                    TreeViewItem slHeader = csrHeader.FormChild("Sequence Lookups");
                                                    csr.SequenceLookups?.ToList().ForEach(sl =>
                                                    {
                                                        slHeader.FormChild(
                                                            $"Sequence index: {sl.SequenceIndex}, Lookup list index: {sl.LookupListIndex}");
                                                    });
                                                });
                                            });
                                            break;
                                        case SequenceContextFormat3 scf3:
                                            TreeViewItem cfHeader = scHeader.FormChild("Coverage Formats");
                                            scf3.CoverageFormats.ForEach(cf =>
                                            {
                                                cfHeader.Items.Add(Utilities.BuildCommonCoverageItem(cf));
                                            });
                                            TreeViewItem slHeader = scHeader.FormChild("Sequence Lookups");
                                            scf3.SequenceLookups.ToList().ForEach(sl =>
                                            {
                                                slHeader.FormChild($"Sequence index: {sl.SequenceIndex}, Lookup list index: {sl.LookupListIndex}");
                                            });
                                            break;
                                    }
                                    break;
                                case IChainedSequenceContext csc:
                                    TreeViewItem cscHeader = subTables.FormChild("Chained Sequence Context");
                                    switch (csc)
                                    {
                                        case ChainedSequenceContextFormat1 cscf1:
                                            cscHeader.Items.Add(Utilities.BuildCommonCoverageItem(cscf1.CoverageFormat));
                                            TreeViewItem csrsHeader = cscHeader.FormChild("Chained Sequence Rule Sets");
                                            cscf1.ChainedSequenceRuleSets.ForEach(csrs =>
                                            {
                                                TreeViewItem csrHeader = csrsHeader.FormChild("Chained Sequence Rules");
                                                csrs.ChainedSequenceRules.ForEach(csr =>
                                                {
                                                    csr.SequenceLookups.ForEach(sl =>
                                                    {
                                                        csrHeader.FormChild($"Sequence index: {sl.SequenceIndex}, Lookup list index: {sl.LookupListIndex}");
                                                    });
                                                });
                                            });
                                            break;
                                        case ChainedSequenceContextFormat2 cscf2:
                                            cscHeader.Items.Add(Utilities.BuildCommonCoverageItem(cscf2.Coverage));
                                            TreeViewItem bcdHeader = cscHeader.FormChild("Backtrack Class Definition");
                                            bcdHeader.Items.Add(Utilities.BuildCommonClassDefinition(cscf2.BacktrackClassDef));
                                            TreeViewItem inpHeader = cscHeader.FormChild("Input Class Definition");
                                            inpHeader.Items.Add(Utilities.BuildCommonClassDefinition(cscf2.InputClassDef));
                                            TreeViewItem laHeader = cscHeader.FormChild("Lookahead Class Definition");
                                            laHeader.Items.Add(Utilities.BuildCommonClassDefinition(cscf2.LookaheadClassDef));
                                            TreeViewItem ccsrsHeader = cscHeader.FormChild("Chained Class Sequence Rule Sets");
                                            cscf2.ChainedClassSequenceRuleSets.ForEach(ccsrs =>
                                            {
                                                TreeViewItem ccsrHeader = ccsrsHeader.FormChild("Chained Class Sequence Rule");
                                                ccsrs.ChainedClassSequenceRules.ForEach(ccsr =>
                                                {
                                                    ccsrHeader.FormChild(nameof(ccsr.InputSequences), string.Join(", ", ccsr.InputSequences));
                                                    ccsrHeader.FormChild(nameof(ccsr.LookaheadSequences), string.Join(", ", ccsr.LookaheadSequences));
                                                    ccsrHeader.FormChild(nameof(ccsr.BacktrackSequences), string.Join(", ", ccsr.BacktrackSequences));
                                                    TreeViewItem slHeader = ccsrHeader.FormChild("Sequence Lookups");
                                                    ccsr.SequenceLookups.ForEach(sl =>
                                                    {
                                                        slHeader.FormChild($"Sequence index: {sl.SequenceIndex}, Lookup list index: {sl.LookupListIndex}");
                                                    });
                                                });
                                            });
                                            break;
                                        case ChainedSequenceContextFormat3 cscf3:
                                            TreeViewItem icHeader = cscHeader.FormChild("Input Coverages");
                                            cscf3.InputCoverages.ForEach(ic =>
                                            {
                                                icHeader.Items.Add(Utilities.BuildCommonCoverageItem(ic));
                                            });
                                            TreeViewItem lookaheadHeader = cscHeader.FormChild("Lookahead Coverages");
                                            cscf3.LookaheadCoverages.ForEach(lc =>
                                            {
                                                lookaheadHeader.Items.Add(Utilities.BuildCommonCoverageItem(lc));
                                            });
                                            TreeViewItem backtrackHeader = cscHeader.FormChild("Backtrack Coverages");
                                            cscf3.BacktrackCoverages.ForEach(bc =>
                                            {
                                                backtrackHeader.Items.Add(Utilities.BuildCommonCoverageItem(bc));
                                            });
                                            TreeViewItem slHeader = cscHeader.FormChild("Sequence Lookups");
                                            cscf3.SequenceLookups.ForEach(sl =>
                                            {
                                                slHeader.FormChild($"Sequence index: {sl.SequenceIndex}, Lookup list index: {sl.LookupListIndex}");
                                            });
                                            break;
                                    }
                                    break;
                            }
                        });
                    });
                    break;

                case GdefTable gdefTable:
                    var gdefRoot = new TreeViewItem { Header = "gdef" };
                    ResultView.Items.Add(gdefRoot);
                    if (gdefTable.GlyphClassDef is not null)
                        gdefRoot.Items.Add(Utilities.BuildGdefClassDefinition(gdefTable.GlyphClassDef));
                    if (gdefTable.MarkAttachClassDef is not null)
                        gdefRoot.Items.Add(Utilities.BuildGdefClassDefinition(gdefTable.MarkAttachClassDef));
                    if (gdefTable.AttachListTable is not null)
                    {
                        gdefRoot.FormChild(nameof(gdefTable.AttachListTable.AttachPointOffsets),
                            string.Join(", ", gdefTable.AttachListTable.AttachPointOffsets));
                        gdefRoot.Items.Add(Utilities.BuildCommonCoverageItem(gdefTable.AttachListTable.Coverage));
                    }

                    if (gdefTable.ItemVarStore is not null)
                        gdefRoot.Items.Add(Utilities.BuildCommonVariationIndex(gdefTable.ItemVarStore));
                    if (gdefTable.LigCaretListTable is not null)
                    {
                        TreeViewItem ligCaretListRoot = gdefRoot.FormChild("Ligature Caret List Table");
                        TreeViewItem offsets =
                            ligCaretListRoot.FormChild(nameof(gdefTable.LigCaretListTable.LigGlyphOffsets));
                        gdefTable.LigCaretListTable.LigGlyphOffsets.ForEach(l =>
                        {
                            offsets.FormChild(nameof(l.CaretValueOffsets), string.Join(", ", l.CaretValueOffsets));
                        });
                        ligCaretListRoot.Items.Add(Utilities.BuildCommonCoverageItem(gdefTable.LigCaretListTable.Coverage));
                    }

                    break;

                case GvarTable gvarTable:
                    var gvarRoot = new TreeViewItem { Header = "gvar" };
                    ResultView.Items.Add(gvarRoot);
                    TreeViewItem tuplesHeader = gvarRoot.FormChild("Tuples");
                    gvarTable.Tuples.ForEach(tuple =>
                    {
                        TreeViewItem tupleHeader = tuplesHeader.FormChild("Tuple");
                        tupleHeader.FormChild(nameof(tuple.Coordinates), string.Join(", ", tuple.Coordinates));
                    });
                    TreeViewItem glyphVariations = gvarRoot.FormChild("Glyph Variations");
                    gvarTable.GlyphVariations.ForEach(header =>
                    {
                        TreeViewItem gvHeader = glyphVariations.FormChild("Glyph Variation");
                        gvHeader.FormChild(nameof(header.HasSharedPointNumbers), header.HasSharedPointNumbers);
                        TreeViewItem tupleVariations = gvHeader.FormChild("Tuple Variations");
                        header.TupleVariationHeaders.ForEach(vh =>
                        {
                            TreeViewItem variationHeader = tupleVariations.FormChild("Variation Header");
                            if (vh.PeakTuple is not null)
                            {
                                variationHeader.FormChild(nameof(vh.PeakTuple), string.Join(", ", vh.PeakTuple.Coordinates));
                            }

                            if (vh.IntermediateStartTuple is not null)
                            {
                                variationHeader.FormChild(nameof(vh.IntermediateStartTuple),
                                    string.Join(", ", vh.IntermediateStartTuple.Coordinates));
                            }

                            if (vh.IntermediateEndTuple is not null)
                            {
                                variationHeader.FormChild(nameof(vh.IntermediateEndTuple), string.Join(", ", vh.IntermediateEndTuple.Coordinates));
                            }
                            variationHeader.FormChild(nameof(vh.TupleIndex), vh.TupleIndex);
                        });
                    });
                    break;

                case AvarTable avarTable:
                    var avarRoot = new TreeViewItem { Header = "avar" };
                    ResultView.Items.Add(avarRoot);
                    break;

                case BaseTable baseTable:
                    var baseRoot = new TreeViewItem { Header = "base" };
                    ResultView.Items.Add(baseRoot);
                    if (baseTable.HorizontalAxisTable is not null)
                    {
                        TreeViewItem? horzAxisView = Utilities.BuildAxisTable(baseTable.HorizontalAxisTable);
                        if (horzAxisView is not null) baseRoot.Items.Add(horzAxisView);
                    }

                    if (baseTable.VerticalAxisTable is not null)
                    {
                        TreeViewItem? vertAxisView = Utilities.BuildAxisTable(baseTable.VerticalAxisTable);
                        if (vertAxisView is not null) baseRoot.Items.Add(vertAxisView);
                    }

                    if (baseTable.ItemVariationStore is not null)
                    {
                        TreeViewItem itemVariationStore = baseRoot.FormChild(nameof(baseTable.ItemVariationStore));
                        baseTable.ItemVariationStore.ItemVariationData.ForEach(ivd =>
                        {
                            TreeViewItem data = itemVariationStore.FormChild(nameof(baseTable.ItemVariationStore.ItemVariationData));
                            data.FormChild(nameof(ivd.RegionIndexes), string.Join(", ", ivd.RegionIndexes));
                        });
                    }
                    break;

                case CbdtTable cbdtTable:
                    var cbdtRoot = new TreeViewItem { Header = "cbdt" };
                    ResultView.Items.Add(cbdtRoot);
                    break;

                case CblcTable cblcTable:
                    var cblcRoot = new TreeViewItem { Header = "cblc" };
                    ResultView.Items.Add(cblcRoot);
                    break;

                case EbdtTable ebdtTable:
                    var ebdtRoot = new TreeViewItem { Header = "ebdt" };
                    ResultView.Items.Add(ebdtRoot);
                    break;

                case EbscTable ebscTable:
                    var ebscRoot = new TreeViewItem { Header = "ebsc" };
                    ResultView.Items.Add(ebscRoot);
                    break;

                case EblcTable eblcTable:
                    var eblcRoot = new TreeViewItem { Header = "eblc" };
                    ResultView.Items.Add(eblcRoot);
                    break;

                case Type1Table type1Table:
                    var type1Root = new TreeViewItem { Header = "CFF" };
                    ResultView.Items.Add(type1Root);
                    break;

                case ColrTable colrTable:
                    var colrRoot = new TreeViewItem { Header = "Colr" };
                    ResultView.Items.Add(colrRoot);
                    break;

                case CpalTable cpalTable:
                    var cpalRoot = new TreeViewItem { Header = "CPAL" };
                    ResultView.Items.Add(cpalRoot);
                    break;

                case CvarTable cvarTable:
                    var cvarRoot = new TreeViewItem { Header = "Cvar" };
                    ResultView.Items.Add(cvarRoot);
                    break;

                case FftmTable fftmTable:
                    var fftmRoot = new TreeViewItem { Header = "FFTM" };
                    ResultView.Items.Add(fftmRoot);
                    break;

                case FvarTable fvarTable:
                    var fvarRoot = new TreeViewItem { Header = "Fvar" };
                    ResultView.Items.Add(fvarRoot);
                    break;

                case GsubTable gsubTable:
                    var gsubRoot = new TreeViewItem { Header = "gsub" };
                    ResultView.Items.Add(gsubRoot);
                    TreeViewItem lookupList = gsubRoot.FormChild("Subtables");
                    gsubTable.GsubLookupList.LookupTables.ForEach(lt =>
                    {
                        lt.SubTables.ForEach(st =>
                        {
                            switch (st)
                            {
                                case SingleSubstitutionFormat1 ssf1:
                                    TreeViewItem ssf1Header = lookupList.FormChild("Single Substitution Format 1");
                                    ssf1Header.FormChild(nameof(ssf1.DeltaGlyphId), ssf1.DeltaGlyphId);
                                    ssf1Header.Items.Add(Utilities.BuildCommonCoverageItem(ssf1.Coverage));
                                    break;
                                case SingleSubstitutionFormat2 ssf2:
                                    TreeViewItem ssf2Header = lookupList.FormChild("Single Substitution Format 2");
                                    ssf2Header.Items.Add(Utilities.BuildCommonCoverageItem(ssf2.Coverage));
                                    ssf2Header.FormChild(nameof(ssf2.SubstituteGlyphIds), string.Join(", ", ssf2.SubstituteGlyphIds));
                                    break;
                                case MultipleSubstitutionFormat1 msf1:
                                    TreeViewItem msf1Header = lookupList.FormChild("Multiple Substitution Format 1");
                                    msf1Header.Items.Add(Utilities.BuildCommonCoverageItem(msf1.Coverage));
                                    TreeViewItem seqHeader = msf1Header.FormChild("Sequences");
                                    msf1.Sequences.ForEach(s =>
                                    {
                                        seqHeader.FormChild(nameof(s.SubstituteGlyphIds), string.Join(", ", s.SubstituteGlyphIds));
                                    });
                                    break;
                                case AlternateSubstitutionFormat1 asf1:
                                    TreeViewItem asf1Header = lookupList.FormChild("Alternate Substitution Format 1");
                                    asf1Header.Items.Add(Utilities.BuildCommonCoverageItem(asf1.Coverage));
                                    TreeViewItem asHeader = asf1Header.FormChild("Alternate Sets");
                                    asf1.AlternateSets.ForEach(altSet =>
                                    {
                                        asHeader.FormChild(nameof(altSet.AlternateGlyphIds), string.Join(", ", altSet.AlternateGlyphIds));
                                    });
                                    break;
                                case LigatureSubstitutionFormat1 lsf1:
                                    TreeViewItem lsf1Header = lookupList.FormChild("Ligature Substitution Format 1");
                                    lsf1Header.Items.Add(Utilities.BuildCommonCoverageItem(lsf1.Coverage));
                                    TreeViewItem lsHeader = lsf1Header.FormChild("Ligature Sets");
                                    lsf1.LigatureSets.ForEach(ls =>
                                    {
                                        TreeViewItem ltHeader = lsHeader.FormChild("Ligature Tables");
                                        ls.LigatureTables.ForEach(ligt =>
                                        {
                                            TreeViewItem lHeader = ltHeader.FormChild("Ligature Table");
                                            lHeader.FormChild(nameof(ligt.LigatureGlyph), ligt.LigatureGlyph);
                                            lHeader.FormChild(nameof(ligt.ComponentGlyphIds), string.Join(", ", ligt.ComponentGlyphIds));
                                        });
                                    });
                                    break;
                                case SequenceContextFormat1 scf1:
                                    TreeViewItem scf1Header = lookupList.FormChild("Sequence Context Format 1");
                                    scf1Header.Items.Add(Utilities.BuildCommonCoverageItem(scf1.Coverage));
                                    TreeViewItem srsHeader = scf1Header.FormChild("Sequence Rule Sets");
                                    scf1.SequenceRuleSets.ForEach(srs =>
                                    {
                                        TreeViewItem srHeader = srsHeader.FormChild("Sequence Rules");
                                        srs.SequenceRules.ForEach(sr =>
                                        {
                                            srHeader.FormChild(nameof(sr.GlyphIds), string.Join(", ", sr.GlyphIds));
                                        });
                                    });
                                    break;
                                case SequenceContextFormat2 scf2:
                                    TreeViewItem scf2Header = lookupList.FormChild("Sequence Context Format 2");
                                    scf2Header.Items.Add(Utilities.BuildCommonCoverageItem(scf2.Coverage));
                                    scf2Header.Items.Add(Utilities.BuildCommonClassDefinition(scf2.ClassDef));
                                    TreeViewItem csrsHeader = scf2Header.FormChild("Class Sequence Rule Sets");
                                    scf2.ClassSequenceRuleSets.ForEach(csrs =>
                                    {
                                        TreeViewItem csrHeader = csrsHeader.FormChild("Class Sequence Rules");
                                        csrs.ClassSeqRules.ForEach(csr =>
                                        {
                                            TreeViewItem slHeader = csrHeader.FormChild("Sequence Lookups");
                                            csr.SequenceLookups.ForEach(sl =>
                                            {
                                                slHeader.FormChild($"Sequence index: {sl.SequenceIndex} Lookup list index: {sl.LookupListIndex}");
                                            });
                                        });
                                    });
                                    break;
                                case SequenceContextFormat3 scf3:
                                    TreeViewItem scf3Header = lookupList.FormChild("Sequence Context Format 3");
                                    TreeViewItem cfHeader = scf3Header.FormChild("Coverage formats");
                                    scf3.CoverageFormats.ForEach(cf =>
                                    {
                                        cfHeader.Items.Add(Utilities.BuildCommonCoverageItem(cf));
                                    });
                                    TreeViewItem slHeader = scf3Header.FormChild("Sequence lookups");
                                    scf3.SequenceLookups.ForEach(sl =>
                                    {
                                        slHeader.FormChild($"Sequence index: {sl.SequenceIndex} Lookup list index: {sl.LookupListIndex}");
                                    });
                                    break;
                                case ChainedSequenceContextFormat1 cscf1:
                                    TreeViewItem cscf1Header = lookupList.FormChild("Chained Sequence Context Format 1");
                                    cscf1Header.Items.Add(Utilities.BuildCommonCoverageItem(cscf1.CoverageFormat));
                                    TreeViewItem csrs1Header = cscf1Header.FormChild("Chained Sequence Rule Sets");
                                    cscf1.ChainedSequenceRuleSets.ForEach(csrs =>
                                    {
                                        TreeViewItem csr1Header = csrs1Header.FormChild("Chained Sequence Rules");
                                        csrs.ChainedSequenceRules.ForEach(csr =>
                                        {
                                            TreeViewItem csr2Header = csr1Header.FormChild("Chained Sequence Rule");
                                            csr2Header.FormChild(nameof(csr.BacktrackSequence), string.Join(", ", csr.BacktrackSequence));
                                            csr2Header.FormChild(nameof(csr.InputSequence), string.Join(", ", csr.InputSequence));
                                            csr2Header.FormChild(nameof(csr.LookaheadSequence), string.Join(", ", csr.LookaheadSequence));
                                            TreeViewItem sl1Header = csr2Header.FormChild("Chained Sequence Lookups");
                                            csr.SequenceLookups.ForEach(sl =>
                                            {
                                                sl1Header.FormChild(
                                                    $"Sequence index: {sl.SequenceIndex} Lookup list index: {sl.LookupListIndex}");
                                            });
                                        });
                                    });
                                    break;
                                case ChainedSequenceContextFormat2 cscf2:
                                    TreeViewItem cscf2Header = lookupList.FormChild("Chained Sequence Context Format 2");
                                    cscf2Header.Items.Add(Utilities.BuildCommonCoverageItem(cscf2.Coverage));
                                    cscf2Header.Items.Add(
                                        Utilities.BuildCommonClassDefinition(cscf2.BacktrackClassDef));
                                    cscf2Header.Items.Add(Utilities.BuildCommonClassDefinition(cscf2.InputClassDef));
                                    cscf2Header.Items.Add(Utilities.BuildCommonClassDefinition(cscf2.LookaheadClassDef));
                                    TreeViewItem ccsrsHeader = cscf2Header.FormChild("Chained Class Sequence Rule Sets");
                                    cscf2.ChainedClassSequenceRuleSets.ForEach(ccsrs =>
                                    {
                                        TreeViewItem ccsrHeader = ccsrsHeader.FormChild("Chained Class Sequence Rules");
                                        ccsrs.ChainedClassSequenceRules.ForEach(ccsr =>
                                        {
                                            ccsrHeader.FormChild(nameof(ccsr.BacktrackSequences), string.Join(", ", ccsr.BacktrackSequences));
                                            ccsrHeader.FormChild(nameof(ccsr.InputSequences), string.Join(", ", ccsr.InputSequences));
                                            ccsrHeader.FormChild(nameof(ccsr.LookaheadSequences), string.Join(", ", ccsr.LookaheadSequences));
                                            TreeViewItem sl1Header = ccsrHeader.FormChild("Sequence Lookups");
                                            ccsr.SequenceLookups.ForEach(sl =>
                                            {
                                                sl1Header.FormChild($"Sequence index: {sl.SequenceIndex} Lookup list index: {sl.LookupListIndex}");
                                            });
                                        });
                                    });
                                    break;
                                case ChainedSequenceContextFormat3 cscf3:
                                    TreeViewItem cscf3Header = lookupList.FormChild("Chained Sequence Context Format 3");
                                    TreeViewItem bcHeader = cscf3Header.FormChild("Backtrack Coverages");
                                    cscf3.BacktrackCoverages.ForEach(bc =>
                                    {
                                        bcHeader.Items.Add(Utilities.BuildCommonCoverageItem(bc));
                                    });
                                    TreeViewItem icHeader = cscf3Header.FormChild("Input Coverages");
                                    cscf3.InputCoverages.ForEach(ic =>
                                    {
                                        icHeader.Items.Add(Utilities.BuildCommonCoverageItem(ic));
                                    });
                                    TreeViewItem lcHeader = cscf3Header.FormChild("Lookahead Coverages");
                                    cscf3.LookaheadCoverages.ForEach(lc =>
                                    {
                                        lcHeader.Items.Add(Utilities.BuildCommonCoverageItem(lc));
                                    });
                                    TreeViewItem sl2Header = cscf3Header.FormChild("Sequence Lookups");
                                    cscf3.SequenceLookups.ForEach(sl =>
                                    {
                                        sl2Header.FormChild($"Sequence index: {sl.SequenceIndex} Lookup list index: {sl.LookupListIndex}");
                                    });
                                    break;
                                case SubstitutionExtensionFormat1 sef1:
                                    TreeViewItem sef1Header = lookupList.FormChild("Substitution Extension Format 1");
                                    TreeViewItem subTableHeader = sef1Header.FormChild("Substitution Table");
                                    switch (sef1.SubstitutionTable)
                                    {
                                    }
                                    break;
                                case ReverseChainSingleSubstitutionFormat1 rcssf1:
                                    TreeViewItem rcssf1Header = lookupList.FormChild("Reverse Chain Single Substitution Format 1");
                                    rcssf1Header.Items.Add(Utilities.BuildCommonCoverageItem(rcssf1.Coverage));
                                    rcssf1Header.FormChild(nameof(rcssf1.SubstituteGlyphIds), string.Join(", ", rcssf1.SubstituteGlyphIds));
                                    TreeViewItem bcCoveragesHeader = rcssf1Header.FormChild("Backtrack Coverages");
                                    rcssf1.BacktrackCoverages.ForEach(bc =>
                                    {
                                        bcCoveragesHeader.Items.Add(Utilities.BuildCommonCoverageItem(bc));
                                    });
                                    TreeViewItem lcCoveragesHeader = rcssf1Header.FormChild("Lookahead Coverages");
                                    rcssf1.LookaheadCoverages.ForEach(lc =>
                                    {
                                        lcCoveragesHeader.Items.Add(Utilities.BuildCommonCoverageItem(lc));
                                    });
                                    break;
                            }
                        });
                    });
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
                    headRoot.FormChild("Bounds",
                        $"{headTable.XMin}, {headTable.YMin} {headTable.XMax}, {headTable.YMax}");
                    break;

                case HmtxTable hmtxTable:
                    var hmtxRoot = new TreeViewItem { Header = "hmtx" };
                    ResultView.Items.Add(hmtxRoot);
                    break;

                case HheaTable hheaTable:
                    var hheaRoot = new TreeViewItem { Header = "hhea" };
                    ResultView.Items.Add(hheaRoot);
                    break;

                case HvarTable hvarTable:
                    var hvarRoot = new TreeViewItem { Header = "hvar" };
                    ResultView.Items.Add(hvarRoot);
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
                                TreeViewItem gposExtensionDisable =
                                    prioritiesHeader.FormChild("Gpos Extension Disable", p.GposExtensionDisable);
                                gposExtensionDisable.FormChild("Lookup Indices",
                                    string.Join(", ", p.GposExtensionDisable.GsubLookupIndices));
                            }

                            if (p.GposExtensionEnable is not null)
                            {
                                TreeViewItem gposExtensionEnable =
                                    prioritiesHeader.FormChild("Gpos Extension Enable", p.GposExtensionEnable);
                                gposExtensionEnable.FormChild("Lookup Indices",
                                    string.Join(", ", p.GposExtensionEnable.GsubLookupIndices));
                            }

                            if (p.ExtensionJstfMax is not null)
                                prioritiesHeader.FormChild(nameof(p.ExtensionJstfMax),
                                    string.Join(", ", p.ExtensionJstfMax));

                            if (p.GposShrinkageDisable is not null)
                            {
                                TreeViewItem gposShrinkageDisable =
                                    prioritiesHeader.FormChild("Gpos Shrinkage Disable", p.GposShrinkageDisable);
                                gposShrinkageDisable.FormChild("Lookup Indices",
                                    string.Join(", ", p.GposShrinkageDisable.GsubLookupIndices));
                            }

                            if (p.GposShrinkageEnable is null) return;
                            TreeViewItem gposShrinkageEnable =
                                prioritiesHeader.FormChild("Gpos Shrinkage Enable", p.GposShrinkageEnable);
                            gposShrinkageEnable.FormChild("Lookup Indices",
                                string.Join(", ", p.GposShrinkageEnable.GsubLookupIndices));
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
                    var mathRoot = new TreeViewItem { Header = "math" };
                    ResultView.Items.Add(mathRoot);
                    TreeViewItem constants = mathRoot.FormChild(nameof(mathTable.Constants));
                    TreeViewItem mathLeading = constants.FormChild(nameof(mathTable.Constants.MathLeading));
                    mathLeading.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.MathLeading));
                    TreeViewItem axisHeight = constants.FormChild(nameof(mathTable.Constants.AxisHeight));
                    axisHeight.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.AxisHeight));
                    TreeViewItem accentBaseHeight = constants.FormChild(nameof(mathTable.Constants.AccentBaseHeight));
                    accentBaseHeight.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.AccentBaseHeight));
                    TreeViewItem fractionRuleThickness = constants.FormChild(nameof(mathTable.Constants.FractionRuleThickness));
                    fractionRuleThickness.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.FractionRuleThickness));
                    TreeViewItem overbarExtraAscender = constants.FormChild(nameof(mathTable.Constants.OverbarExtraAscender));
                    overbarExtraAscender.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.OverbarExtraAscender));
                    TreeViewItem overbarRuleThickness = constants.FormChild(nameof(mathTable.Constants.OverbarRuleThickness));
                    overbarRuleThickness.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.OverbarRuleThickness));
                    TreeViewItem overbarVerticalGap =
                        constants.FormChild(nameof(mathTable.Constants.OverbarVerticalGap));
                    overbarVerticalGap.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.OverbarVerticalGap));
                    TreeViewItem radicalExtraAscender =
                        constants.FormChild(nameof(mathTable.Constants.RadicalExtraAscender));
                    radicalExtraAscender.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.RadicalExtraAscender));
                    TreeViewItem radicalRuleThickness =
                        constants.FormChild(nameof(mathTable.Constants.RadicalRuleThickness));
                    radicalRuleThickness.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.RadicalRuleThickness));
                    TreeViewItem radicalVerticalGap =
                        constants.FormChild(nameof(mathTable.Constants.RadicalVerticalGap));
                    radicalVerticalGap.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.RadicalVerticalGap));
                    TreeViewItem flattenedAccentBaseHeight = constants.FormChild(nameof(mathTable.Constants.FlattenedAccentBaseHeight));
                    flattenedAccentBaseHeight.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.FlattenedAccentBaseHeight));
                    TreeViewItem subscriptShiftDown =
                        constants.FormChild(nameof(mathTable.Constants.SubscriptShiftDown));
                    subscriptShiftDown.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.SubscriptShiftDown));
                    TreeViewItem subscriptTopMax = constants.FormChild(nameof(mathTable.Constants.SubscriptTopMax));
                    subscriptTopMax.FormChild(nameof(mathTable.Constants.SubscriptTopMax));
                    TreeViewItem subscriptBaselineDropMin = constants.FormChild(nameof(mathTable.Constants.SubscriptBaselineDropMin));
                    subscriptBaselineDropMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.SubscriptBaselineDropMin));
                    TreeViewItem superscriptShiftUp = constants.FormChild(nameof(mathTable.Constants.SuperscriptShiftUp));
                    superscriptShiftUp.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.SuperscriptShiftUp));
                    TreeViewItem superscriptShiftUpCramped = constants.FormChild(nameof(mathTable.Constants.SuperscriptShiftUpCramped));
                    superscriptShiftUpCramped.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.SuperscriptShiftUpCramped));
                    TreeViewItem superscriptBottomMin = constants.FormChild(nameof(mathTable.Constants.SuperscriptBottomMin));
                    superscriptBottomMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.SuperscriptBottomMin));
                    TreeViewItem superscriptBaselineDropMax = constants.FormChild(nameof(mathTable.Constants.SuperscriptBaselineDropMax));
                    superscriptBaselineDropMax.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.SuperscriptBaselineDropMax));
                    TreeViewItem subSuperscriptGapMin =
                        constants.FormChild(nameof(mathTable.Constants.SubSuperscriptGapMin));
                    subSuperscriptGapMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.SubSuperscriptGapMin));
                    TreeViewItem superscriptBottomMaxWithSubscript = constants.FormChild(nameof(mathTable.Constants.SuperscriptBottomMaxWithSubscript));
                    superscriptBottomMaxWithSubscript.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.SuperscriptBottomMaxWithSubscript));
                    TreeViewItem spaceAfterScript = constants.FormChild(nameof(mathTable.Constants.SpaceAfterScript));
                    spaceAfterScript.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.SpaceAfterScript));
                    TreeViewItem upperLimitGapMin = constants.FormChild(nameof(mathTable.Constants.UpperLimitGapMin));
                    upperLimitGapMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.UpperLimitGapMin));
                    TreeViewItem upperLimitBaselineRiseMin = constants.FormChild(nameof(mathTable.Constants.UpperLimitBaselineRiseMin));
                    upperLimitBaselineRiseMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.UpperLimitBaselineRiseMin));
                    TreeViewItem lowerLimitGapMin = constants.FormChild(nameof(mathTable.Constants.LowerLimitGapMin));
                    lowerLimitGapMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.LowerLimitGapMin));
                    TreeViewItem lowerLimitBaselineDropMin = constants.FormChild(nameof(mathTable.Constants.LowerLimitBaselineDropMin));
                    lowerLimitBaselineDropMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.LowerLimitBaselineDropMin));
                    TreeViewItem stackTopShiftUp = constants.FormChild(nameof(mathTable.Constants.StackTopShiftUp));
                    stackTopShiftUp.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.StackTopShiftUp));
                    TreeViewItem stackTopDisplayStyleShiftUp = constants.FormChild(nameof(mathTable.Constants.StackTopDisplayStyleShiftUp));
                    stackTopDisplayStyleShiftUp.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.StackTopDisplayStyleShiftUp));
                    TreeViewItem stackBottomShiftDown = constants.FormChild(nameof(mathTable.Constants.StackBottomShiftDown));
                    stackBottomShiftDown.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.StackBottomShiftDown));
                    TreeViewItem stackBottomDisplayStyleShiftDown = constants.FormChild(nameof(mathTable.Constants.StackBottomDisplayStyleShiftDown));
                    stackBottomDisplayStyleShiftDown.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.StackBottomDisplayStyleShiftDown));
                    TreeViewItem stackGapMin = constants.FormChild(nameof(mathTable.Constants.StackGapMin));
                    stackGapMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.StackGapMin));
                    TreeViewItem stackDisplayStyleGapMin = constants.FormChild(nameof(mathTable.Constants.StackDisplayStyleGapMin));
                    stackDisplayStyleGapMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.StackDisplayStyleGapMin));
                    TreeViewItem stretchStackTopShiftUp = constants.FormChild(nameof(mathTable.Constants.StretchStackTopShiftUp));
                    stretchStackTopShiftUp.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.StretchStackTopShiftUp));
                    TreeViewItem stretchStackBottomShiftDown = constants.FormChild(nameof(mathTable.Constants.StretchStackBottomShiftDown));
                    stretchStackBottomShiftDown.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.StretchStackBottomShiftDown));
                    TreeViewItem stretchStackGapAboveMin = constants.FormChild(nameof(mathTable.Constants.StretchStackGapAboveMin));
                    stretchStackGapAboveMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.StretchStackGapAboveMin));
                    TreeViewItem stretchStackGapBelowMin = constants.FormChild(nameof(mathTable.Constants.StretchStackGapBelowMin));
                    stretchStackGapBelowMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.StretchStackGapBelowMin));
                    TreeViewItem fractionNumeratorShiftUp = constants.FormChild(nameof(mathTable.Constants.FractionNumeratorShiftUp));
                    fractionNumeratorShiftUp.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.FractionNumeratorShiftUp));
                    TreeViewItem fractionNumeratorDisplayStyleShiftUp = constants.FormChild(nameof(mathTable.Constants.FractionNumeratorDisplayStyleShiftUp));
                    fractionNumeratorDisplayStyleShiftUp.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.FractionNumeratorDisplayStyleShiftUp));
                    TreeViewItem fractionDenominatorShiftDown = constants.FormChild(nameof(mathTable.Constants.FractionDenominatorShiftDown));
                    fractionDenominatorShiftDown.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.FractionDenominatorShiftDown));
                    TreeViewItem fractionDenominatorDisplayStyleShiftDown = constants.FormChild(nameof(mathTable.Constants.FractionDenominatorDisplayStyleShiftDown));
                    fractionDenominatorDisplayStyleShiftDown.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.FractionDenominatorDisplayStyleShiftDown));
                    TreeViewItem fractionNumeratorGapMin = constants.FormChild(nameof(mathTable.Constants.FractionNumeratorGapMin));
                    fractionNumeratorGapMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.FractionNumeratorGapMin));
                    TreeViewItem fractionNumDisplayStyleGapMin =
                        constants.FormChild(nameof(mathTable.Constants.FractionNumDisplayStyleGapMin));
                    fractionNumDisplayStyleGapMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.FractionNumDisplayStyleGapMin));
                    TreeViewItem fractionDenominatorGapMin = constants.FormChild(nameof(mathTable.Constants.FractionDenominatorGapMin));
                    fractionDenominatorGapMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.FractionDenominatorGapMin));
                    TreeViewItem fractionDenomDisplayStyleGapMin = constants.FormChild(nameof(mathTable.Constants.FractionDenomDisplayStyleGapMin));
                    fractionDenomDisplayStyleGapMin.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.FractionDenomDisplayStyleGapMin));
                    TreeViewItem skewedFractionHorizontalGap = constants.FormChild(nameof(mathTable.Constants.SkewedFractionHorizontalGap));
                    skewedFractionHorizontalGap.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.SkewedFractionHorizontalGap));
                    TreeViewItem skewedFractionVerticalGap = constants.FormChild(nameof(mathTable.Constants.SkewedFractionVerticalGap));
                    skewedFractionVerticalGap.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.SkewedFractionVerticalGap));
                    TreeViewItem underbarVerticalGap = constants.FormChild(nameof(mathTable.Constants.UnderbarVerticalGap));
                    underbarVerticalGap.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.UnderbarVerticalGap));
                    TreeViewItem underbarRuleThickness = constants.FormChild(nameof(mathTable.Constants.UnderbarRuleThickness));
                    underbarRuleThickness.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.UnderbarRuleThickness));
                    TreeViewItem underbarExtraDescender = constants.FormChild(nameof(mathTable.Constants.UnderbarExtraDescender));
                    underbarExtraDescender.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.UnderbarExtraDescender));
                    TreeViewItem radicalDisplayStyleVerticalGap =
                        constants.FormChild(nameof(mathTable.Constants.RadicalDisplayStyleVerticalGap));
                    radicalDisplayStyleVerticalGap.Items.Add(
                        Utilities.BuildMathValueRecord(mathTable.Constants.RadicalDisplayStyleVerticalGap));
                    TreeViewItem radicalKernBeforeDegree =
                        constants.FormChild(nameof(mathTable.Constants.RadicalKernBeforeDegree));
                    radicalKernBeforeDegree.Items.Add(
                        Utilities.BuildMathValueRecord(mathTable.Constants.RadicalKernBeforeDegree));
                    TreeViewItem radicalKernAfterDegree =
                        constants.FormChild(nameof(mathTable.Constants.RadicalKernAfterDegree));
                    radicalKernAfterDegree.Items.Add(Utilities.BuildMathValueRecord(mathTable.Constants.RadicalKernAfterDegree));
                    constants.FormChild(nameof(mathTable.Constants.ScriptPercentScaleDown), mathTable.Constants.ScriptPercentScaleDown);
                    constants.FormChild(nameof(mathTable.Constants.ScriptPercentScaleUp), mathTable.Constants.ScriptPercentScaleUp);
                    constants.FormChild(nameof(mathTable.Constants.DelimitedSubFormulaMinHeight), mathTable.Constants.DelimitedSubFormulaMinHeight);
                    constants.FormChild(nameof(mathTable.Constants.DisplayOperatorMinHeight), mathTable.Constants.DisplayOperatorMinHeight);
                    constants.FormChild(nameof(mathTable.Constants.RadicalDegreeBottomRaisePercent),
                        mathTable.Constants.RadicalDegreeBottomRaisePercent);
                    TreeViewItem mathGlyphInfoTable = mathRoot.FormChild("Glyph Info");
                    mathGlyphInfoTable.Items.Add(Utilities.BuildCommonCoverageItem(mathTable.GlyphInfo.ExtendedShapeCoverage));
                    TreeViewItem mathKernInfoTable = mathGlyphInfoTable.FormChild("Kern Info");
                    mathKernInfoTable.Items.Add(Utilities.BuildCommonCoverageItem(mathTable.GlyphInfo.KernInfo.MathKernCoverage));
                    TreeViewItem mathKernInfoRecords = mathKernInfoTable.FormChild("Kern Info Records");
                    mathTable.GlyphInfo.KernInfo.MathKernInfoRecords.ForEach(mki =>
                    {
                        mathKernInfoRecords.Items.Add(Utilities.BuildMathKernInfo(mki));
                    });
                    TreeViewItem mathItalicsCorrectionInfo = mathGlyphInfoTable.FormChild("Italics Correction Info");
                    mathItalicsCorrectionInfo.Items.Add(Utilities.BuildCommonCoverageItem(mathTable.GlyphInfo.ItalicsCorrectionInfo.Coverage));
                    TreeViewItem italicsCorrections = mathItalicsCorrectionInfo.FormChild("Italics Corrections");
                    mathTable.GlyphInfo.ItalicsCorrectionInfo.ItalicsCorrections.ForEach(ic =>
                    {
                        italicsCorrections.Items.Add(Utilities.BuildMathValueRecord(ic));
                    });
                    TreeViewItem topAccentAttachment = mathGlyphInfoTable.FormChild("Top Accent Attachment Coverage");
                    topAccentAttachment.Items.Add(Utilities.BuildCommonCoverageItem(mathTable.GlyphInfo.TopAccentAttachment.TopAccentCoverage));
                    TreeViewItem topAccentAttachments = mathGlyphInfoTable.FormChild("Top Accent Attachments");
                    mathTable.GlyphInfo.TopAccentAttachment.TopAccentAttachments.ForEach(a =>
                    {
                        topAccentAttachments.Items.Add(Utilities.BuildMathValueRecord(a));
                    });
                    TreeViewItem variantsHeader = mathRoot.FormChild("Variants");
                    variantsHeader.FormChild(nameof(mathTable.Variants.MinConnectorOverlap),
                        mathTable.Variants.MinConnectorOverlap);
                    if (mathTable.Variants.HorizGlyphCoverage is not null)
                    {
                        variantsHeader.Items.Add(Utilities.BuildCommonCoverageItem(mathTable.Variants.HorizGlyphCoverage));
                    }

                    TreeViewItem hgcHeader = variantsHeader.FormChild("Horizontal Glyph Construction");
                    mathTable.Variants.HorizGlyphConstruction.ForEach(hgc =>
                    {
                        if (hgc.GlyphAssembly is not null)
                        {
                            hgcHeader.Items.Add(Utilities.BuildMathValueRecord(hgc.GlyphAssembly.ItalicCorrection));
                        }
                        TreeViewItem gvrHeader = hgcHeader.FormChild("Glyph Variants");
                        hgc.GlyphVariantRecords.ForEach(gvr =>
                        {
                            gvrHeader.FormChild(
                                $"Variant glyph: {gvr.VariantGlyph} Advance measurement: {gvr.AdvanceMeasurement}");
                        });
                    });

                    if (mathTable.Variants.VertGlyphCoverage is not null)
                    {
                        variantsHeader.Items.Add(Utilities.BuildCommonCoverageItem(mathTable.Variants.VertGlyphCoverage));
                    }
                    TreeViewItem vgcHeader = variantsHeader.FormChild("Vertical Glyph Construction");
                    mathTable.Variants.VertGlyphConstruction.ForEach(vgc =>
                    {
                        if (vgc.GlyphAssembly is not null)
                        {
                            vgcHeader.Items.Add(Utilities.BuildMathValueRecord(vgc.GlyphAssembly.ItalicCorrection));
                        }
                        TreeViewItem gvrHeader = vgcHeader.FormChild("Glyph Variants");
                        vgc.GlyphVariantRecords.ForEach(gvr =>
                        {
                            gvrHeader.FormChild($"Variant glyph: {gvr.VariantGlyph} Advance measurement: {gvr.AdvanceMeasurement}");
                        });
                    });
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
                        classDefinitions.Items.Add(Utilities.BuildCommonClassDefinition(cd));
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
                    var mvarRoot = new TreeViewItem { Header = "mvar" };
                    ResultView.Items.Add(mvarRoot);
                    mvarTable.ValueRecords.ForEach(vr =>
                    {
                        TreeViewItem recordHeader = mvarRoot.FormChild("ValueRecord");
                        recordHeader.FormChild(nameof(vr.ValueTag), Encoding.ASCII.GetString(vr.ValueTag));
                        recordHeader.FormChild(nameof(vr.DeltaSetInnerIndex), vr.DeltaSetInnerIndex);
                        recordHeader.FormChild(nameof(vr.DeltaSetOuterIndex), vr.DeltaSetOuterIndex);
                    });
                    break;

                case DsigTable dsigTable:
                    var dsigRoot = new TreeViewItem { Header = "dsig" };
                    ResultView.Items.Add(dsigRoot);
                    dsigRoot.FormChild(nameof(dsigTable.PermissionFlags), dsigTable.PermissionFlags);
                    dsigTable.SigRecords.ForEach(r =>
                    {
                        TreeViewItem sigHeader = dsigRoot.FormChild("Signature Record");
                        sigHeader.FormChild(nameof(r.SignatureBlock), string.Join(" ", r.SignatureBlock.Signature));
                    });
                    break;

                case HdmxTable hdmxTable:
                    var hdmxRoot = new TreeViewItem { Header = "hdmx" };
                    ResultView.Items.Add(hdmxRoot);
                    hdmxTable.Records.ForEach(r =>
                    {
                        TreeViewItem recordRoot = hdmxRoot.FormChild("Record");
                        recordRoot.FormChild(nameof(r.PixelSize), r.PixelSize);
                        recordRoot.FormChild(nameof(r.MaxWidth), r.MaxWidth);
                        recordRoot.FormChild(nameof(r.Widths), string.Join(", ", r.Widths));
                    });
                    break;

                case LtshTable ltshTable:
                    var ltshRoot = new TreeViewItem { Header = "ltsh" };
                    ResultView.Items.Add(ltshRoot);
                    ltshRoot.FormChild(nameof(ltshTable.YPels), string.Join(", ", ltshTable.YPels));
                    break;
            }
        });
    }
}