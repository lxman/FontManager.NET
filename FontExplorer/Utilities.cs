using System.Windows.Controls;
using NewFontParser.Tables.Base;
using NewFontParser.Tables.Bitmap.Common;
using NewFontParser.Tables.Colr;
using NewFontParser.Tables.Colr.PaintTables;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.ClassDefinition;
using NewFontParser.Tables.Common.CoverageFormat;
using NewFontParser.Tables.Common.ItemVariationStore;
using NewFontParser.Tables.Common.TupleVariationStore;
using NewFontParser.Tables.Gpos.LookupSubtables.AnchorTable;
using NewFontParser.Tables.Gpos.LookupSubtables.Common;
using NewFontParser.Tables.Gpos.LookupSubtables.MarkMarkPos;
using NewFontParser.Tables.Math;
using NewFontParser.Tables.TtTables.Glyf;
using DeltaSetIndexMap = NewFontParser.Tables.Common.ItemVariationStore.DeltaSetIndexMap;
using Tuple = NewFontParser.Tables.Common.TupleVariationStore.Tuple;

namespace FontExplorer;

public static class Utilities
{
    public static TreeViewItem BuildCommonCoverageItem(ICoverageFormat coverage)
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

    public static TreeViewItem BuildCommonClassDefinition(IClassDefinition classDefinition)
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

    public static TreeViewItem BuildGdefClassDefinition(IClassDefinition classDefinition)
    {
        var classDefinitionItem = new TreeViewItem { Header = "Class Definition" };
        switch (classDefinition)
        {
            case ClassDefinitionFormat1 def1:
                classDefinitionItem.Header = "Class Definition Format 1";
                classDefinitionItem.FormChild(nameof(def1.StartGlyph), def1.StartGlyph);
                classDefinitionItem.FormChild(nameof(def1.ClassValues), string.Join(", ", def1.ClassValues));
                break;
            case ClassDefinitionFormat2 def2:
                classDefinitionItem.Header = "Class Definition Format 2";
                def2.ClassRanges.ForEach(cr =>
                {
                    var classRange = new TreeViewItem { Header = "Class Range" };
                    classRange.FormChild(nameof(cr.StartGlyphID), cr.StartGlyphID);
                    classRange.FormChild(nameof(cr.EndGlyphID), cr.EndGlyphID);
                    classRange.FormChild(nameof(cr.Class), cr.Class);
                    classDefinitionItem.Items.Add(classRange);
                });
                break;
        }

        return classDefinitionItem;
    }

    public static TreeViewItem BuildCommonValueRecord(ValueRecord valueRecord, ValueFormat format)
    {
        var valueRecordItem = new TreeViewItem { Header = "Value Record" };
        if (format.HasFlag(ValueFormat.XAdvance))
            valueRecordItem.FormChild(nameof(valueRecord.XAdvance), valueRecord.XAdvance!.Value);
        if (format.HasFlag(ValueFormat.YAdvance))
            valueRecordItem.FormChild(nameof(valueRecord.YAdvance), valueRecord.YAdvance!.Value);
        if (format.HasFlag(ValueFormat.XPlacement))
            valueRecordItem.FormChild(nameof(valueRecord.XPlacement), valueRecord.XPlacement!.Value);
        if (format.HasFlag(ValueFormat.YPlacement))
            valueRecordItem.FormChild(nameof(valueRecord.YPlacement), valueRecord.YPlacement!.Value);
        if (format.HasFlag(ValueFormat.XPlacementDevice))
            valueRecordItem.FormChild(nameof(valueRecord.XPlaDeviceOffset), valueRecord.XPlaDeviceOffset!.Value);
        if (format.HasFlag(ValueFormat.YPlacementDevice))
            valueRecordItem.FormChild(nameof(valueRecord.YPlaDeviceLength), valueRecord.YPlaDeviceLength!.Value);
        if (format.HasFlag(ValueFormat.XAdvanceDevice))
            valueRecordItem.FormChild(nameof(valueRecord.XAdvDeviceOffset), valueRecord.XAdvDeviceOffset!.Value);
        if (format.HasFlag(ValueFormat.YAdvanceDevice))
            valueRecordItem.FormChild(nameof(valueRecord.YAdvDeviceLength), valueRecord.YAdvDeviceLength!.Value);
        return valueRecordItem;
    }

    public static TreeViewItem BuildCommonVariationIndex(VariationIndexTable variationIndexTable)
    {
        var variationIndexItem = new TreeViewItem { Header = "Variation Index Table" };
        variationIndexItem.FormChild(nameof(variationIndexTable.DeltaFormat),
            string.Join(", ", variationIndexTable.DeltaFormat));
        variationIndexItem.FormChild(nameof(variationIndexTable.DeltaSetInnerIndex),
            string.Join(", ", variationIndexTable.DeltaSetInnerIndex));
        variationIndexItem.FormChild(nameof(variationIndexTable.DeltaSetOuterIndex),
            string.Join(", ", variationIndexTable.DeltaSetOuterIndex));
        return variationIndexItem;
    }

    public static TreeViewItem? BuildAxisTable(AxisTable axisTable)
    {
        if (axisTable.BaseTagListTable is null && axisTable.BaseScriptListTable is null) return null;
        var axisItem = new TreeViewItem { Header = "Axis Table" };
        if (axisTable.BaseTagListTable is not null)
        {
            TreeViewItem baseTagListHeader = axisItem.FormChild(nameof(axisTable.BaseTagListTable));
            axisTable.BaseTagListTable.BaselineTags.ForEach(t =>
            {
                baseTagListHeader.Items.Add(t);
            });
        }
        if (axisTable.BaseScriptListTable is not null)
        {
            TreeViewItem bscHeader = axisItem.FormChild("Base Script List");
            axisTable.BaseScriptListTable.BaseScriptRecords.ForEach(bsr =>
            {
                bscHeader.FormChild(nameof(bsr.BaseScriptTag), bsr.BaseScriptTag);
                TreeViewItem bst = bscHeader.FormChild(nameof(bsr.BaseScriptTable));
                TreeViewItem bvt = bst.FormChild(nameof(bsr.BaseScriptTable.BaseValuesTable));
                TreeViewItem bcf = bvt.FormChild(nameof(bsr.BaseScriptTable.BaseValuesTable.BaseCoordFormats));
                bsr.BaseScriptTable.BaseValuesTable.BaseCoordFormats.ForEach(cf =>
                {
                    bcf.FormChild(nameof(cf.Coordinate), cf.Coordinate);
                });
            });
        }

        if (axisTable.BaseTagListTable is null) return null;
        TreeViewItem btlHeader = axisItem.FormChild("Base Tag List");
        axisTable.BaseTagListTable.BaselineTags.ForEach(bt => { btlHeader.FormChild(bt); });
        return null;
    }

    public static TreeViewItem BuildMathValueRecord(MathValueRecord mathValueRecord)
    {
        var recordItem = new TreeViewItem { Header = "Math Value Record" };
        recordItem.FormChild(nameof(mathValueRecord.Value), mathValueRecord.Value);
        if (mathValueRecord.DeviceTable is null) return recordItem;
        TreeViewItem deviceTable = recordItem.FormChild(nameof(mathValueRecord.DeviceTable));
        deviceTable.FormChild(nameof(mathValueRecord.DeviceTable.StartSize), mathValueRecord.DeviceTable.StartSize);
        deviceTable.FormChild(nameof(mathValueRecord.DeviceTable.EndSize), mathValueRecord.DeviceTable.EndSize);
        deviceTable.FormChild(nameof(mathValueRecord.DeviceTable.DeltaFormat), mathValueRecord.DeviceTable.DeltaFormat);
        deviceTable.FormChild(nameof(mathValueRecord.DeviceTable.DeltaValues), string.Join(", ", mathValueRecord.DeviceTable.DeltaValues));
        return recordItem;
    }

    public static TreeViewItem BuildDeviceTable(DeviceTable deviceTable)
    {
        var toReturn = new TreeViewItem { Header = "Device Table" };
        toReturn.FormChild(nameof(deviceTable.StartSize), deviceTable.StartSize);
        toReturn.FormChild(nameof(deviceTable.EndSize), deviceTable.EndSize);
        toReturn.FormChild(nameof(deviceTable.DeltaFormat), deviceTable.DeltaFormat);
        toReturn.FormChild(nameof(deviceTable.DeltaValues), string.Join(", ", deviceTable.DeltaValues));
        return toReturn;
    }

    public static TreeViewItem BuildAnchorTable(IAnchorTable anchorTable)
    {
        switch (anchorTable)
        {
            case AnchorTableFormat1 table1:
                return new TreeViewItem { Header = $"X: {table1.X}, Y: {table1.Y}" };
            case AnchorTableFormat2 table2:
                return new TreeViewItem {Header = $"X: {table2.X}, Y: {table2.Y}, AnchorPoint: {table2.AnchorPoint}"};
            case AnchorTableFormat3 table3:
                var toReturn = new TreeViewItem();
                toReturn.FormChild(nameof(table3.X), table3.X);
                toReturn.FormChild(nameof(table3.Y), table3.Y);
                if (table3.DeviceX is not null)
                {
                    TreeViewItem deviceTableX = BuildDeviceTable(table3.DeviceX);
                    deviceTableX.Header = "Device X";
                    toReturn.Items.Add(deviceTableX);
                }
                if (table3.DeviceY is null) return toReturn;
                TreeViewItem deviceTableY = BuildDeviceTable(table3.DeviceY);
                deviceTableY.Header = "Device Y";
                toReturn.Items.Add(deviceTableY);
                return toReturn;
            default:
                throw new ArgumentException("Bad anchor table");
        }
    }

    public static TreeViewItem BuildMarkArray(MarkArray markArray)
    {
        var toReturn = new TreeViewItem { Header = "Mark Array" };
        markArray.MarkRecords.ToList().ForEach(mr =>
        {
            TreeViewItem mrBase = toReturn.FormChild("Mark Record");
            mrBase.FormChild(nameof(mr.MarkClass), mr.MarkClass);
            mrBase.Items.Add(BuildAnchorTable(mr.AnchorTable));
        });
        return toReturn;
    }

    public static TreeViewItem BuildMark2Array(Mark2Array markArray)
    {
        var toReturn = new TreeViewItem { Header = "Mark 2 Array" };
        markArray.Mark2Records.ToList().ForEach(mr =>
        {
            TreeViewItem mrBase = toReturn.FormChild("Mark Record");
            TreeViewItem anchorTablesHeader = mrBase.FormChild(nameof(mr.AnchorTables));
            mr.AnchorTables.ForEach(at =>
            {
                anchorTablesHeader.Items.Add(BuildAnchorTable(at));
            });
        });
        return toReturn;
    }

    public static TreeViewItem BuildMathKernInfo(MathKernInfoRecord mathKernInfoRecord)
    {
        var toReturn = new TreeViewItem { Header = "Math Kern Info Record" };
        if (mathKernInfoRecord.TopRightMathKern is not null)
        {
            TreeViewItem topRightMathKern = toReturn.FormChild("Top Right Math Kern");
            topRightMathKern.Items.Add(BuildMathKernTable(mathKernInfoRecord.TopRightMathKern));
        }

        if (mathKernInfoRecord.TopLeftMathKern is not null)
        {
            TreeViewItem topLeftMathKern = toReturn.FormChild("Top Left Math Kern");
            topLeftMathKern.Items.Add(BuildMathKernTable(mathKernInfoRecord.TopLeftMathKern));
        }

        if (mathKernInfoRecord.BottomLeftMathKern is not null)
        {
            TreeViewItem bottomLeftMathKern = toReturn.FormChild("Bottom Left Math Kern");
            bottomLeftMathKern.Items.Add(BuildMathKernTable(mathKernInfoRecord.BottomLeftMathKern));
        }

        if (mathKernInfoRecord.BottomRightMathKern is null) return toReturn;
        TreeViewItem bottomRightMathKern = toReturn.FormChild("Bottom Right Math Kern");
        bottomRightMathKern.Items.Add(BuildMathKernTable(mathKernInfoRecord.BottomRightMathKern));

        return toReturn;
    }

    public static TreeViewItem BuildMathKernTable(MathKernTable mathKernTable)
    {
        var toReturn = new TreeViewItem { Header = "Math Kern Table" };
        TreeViewItem correctionHeights = toReturn.FormChild("Correction Heights");
        mathKernTable.CorrectionHeights.ForEach(ch =>
        {
            correctionHeights.FormChild(nameof(ch.Value), ch.Value);
            if (ch.DeviceTable is not null) correctionHeights.Items.Add(BuildDeviceTable(ch.DeviceTable));
        });
        TreeViewItem kernValues = toReturn.FormChild("Kern Values");
        mathKernTable.KernValues.ForEach(kv =>
        {
            kernValues.Items.Add(BuildMathValueRecord(kv));
        });
        return toReturn;
    }

    public static TreeViewItem BuildGlyphData(GlyphData data)
    {
        var toReturn = new TreeViewItem { Header = "Glyph Data" };
        TreeViewItem header = toReturn.FormChild("Header");
        header.FormChild(nameof(data.Header.NumberOfContours), data.Header.NumberOfContours);
        header.FormChild(
            $"XMin: {data.Header.XMin} YMin: {data.Header.YMin} XMax: {data.Header.XMax} YMax: {data.Header.YMax}");
        toReturn.FormChild(nameof(data.Index), data.Index);
        switch (data.GlyphSpec)
        {
            case SimpleGlyph simpleGlyph:
                TreeViewItem sgHeader = toReturn.FormChild("Simple Glyph");
                sgHeader.FormChild(nameof(simpleGlyph.EndPtsOfContours), string.Join(',', simpleGlyph.EndPtsOfContours));
                sgHeader.FormChild(nameof(simpleGlyph.Instructions), string.Join(',', simpleGlyph.Instructions));
                TreeViewItem cHeader = sgHeader.FormChild("Coordinates");
                simpleGlyph.Coordinates.ForEach(c =>
                {
                    cHeader.FormChild(c.ToString());
                });
                break;
            case CompositeGlyph compositeGlyph:
                TreeViewItem cgHeader = toReturn.FormChild("Composite Glyph");
                cgHeader.FormChild(nameof(compositeGlyph.GlyphIndex), compositeGlyph.GlyphIndex);
                cgHeader.FormChild(nameof(compositeGlyph.Flags), compositeGlyph.Flags);
                cgHeader.FormChild(nameof(compositeGlyph.Argument1), compositeGlyph.Argument1);
                cgHeader.FormChild(nameof(compositeGlyph.Argument2), compositeGlyph.Argument2);
                break;
        }

        return toReturn;
    }

    public static TreeViewItem BuildSmallGlyphMetrics(SmallGlyphMetricsRecord smallRecord)
    {
        var toReturn = new TreeViewItem { Header = "Small Glyph Metrics" };
        toReturn.FormChild(nameof(smallRecord.Advance), smallRecord.Advance);
        toReturn.FormChild(nameof(smallRecord.Height), smallRecord.Height);
        toReturn.FormChild(nameof(smallRecord.Width), smallRecord.Width);
        toReturn.FormChild(nameof(smallRecord.BearingX), smallRecord.BearingX);
        toReturn.FormChild(nameof(smallRecord.BearingY), smallRecord.BearingY);
        return toReturn;
    }

    public static TreeViewItem BuildBigGlyphMetrics(BigGlyphMetricsRecord bigRecord)
    {
        var toReturn = new TreeViewItem { Header = "Big Glyph Metrics" };
        toReturn.FormChild(nameof(bigRecord.Height), bigRecord.Height);
        toReturn.FormChild(nameof(bigRecord.Width), bigRecord.Width);
        toReturn.FormChild(nameof(bigRecord.HorizontalAdvance), bigRecord.HorizontalAdvance);
        toReturn.FormChild(nameof(bigRecord.VerticalAdvance), bigRecord.VerticalAdvance);
        toReturn.FormChild(nameof(bigRecord.HorizontalBearingX), bigRecord.HorizontalBearingX);
        toReturn.FormChild(nameof(bigRecord.HorizontalBearingY), bigRecord.HorizontalBearingY);
        return toReturn;
    }

    public static TreeViewItem BuildSbitLineMetrics(SbitLineMetrics metrics)
    {
        var toReturn = new TreeViewItem { Header = "Scaler Bitmap Line Metrics" };
        toReturn.FormChild(nameof(metrics.Ascender), metrics.Ascender);
        toReturn.FormChild(nameof(metrics.Descender), metrics.Descender);
        toReturn.FormChild(nameof(metrics.Pad1), metrics.Pad1);
        toReturn.FormChild(nameof(metrics.Pad2), metrics.Pad2);
        toReturn.FormChild(nameof(metrics.WidthMax), metrics.WidthMax);
        toReturn.FormChild(nameof(metrics.CaretOffset), metrics.CaretOffset);
        toReturn.FormChild(nameof(metrics.CaretSlopeNumerator), metrics.CaretSlopeNumerator);
        toReturn.FormChild(nameof(metrics.CaretSlopeDenominator), metrics.CaretSlopeDenominator);
        toReturn.FormChild(nameof(metrics.MaxBeforeBL), metrics.MaxBeforeBL);
        toReturn.FormChild(nameof(metrics.MinAdvanceSB), metrics.MinAdvanceSB);
        toReturn.FormChild(nameof(metrics.MinAfterBL), metrics.MinAfterBL);
        toReturn.FormChild(nameof(metrics.MinOriginSB), metrics.MinOriginSB);
        return toReturn;
    }

    public static TreeViewItem BuildTupleVariationHeader(TupleVariationHeader tvh)
    {
        var toReturn = new TreeViewItem { Header = "Tuple Variation" };
        toReturn.FormChild(nameof(tvh.TupleIndex), tvh.TupleIndex);
        if (tvh.PeakTuple is not null)
        {
            toReturn.FormChild(nameof(tvh.PeakTuple), string.Join(", ", tvh.PeakTuple));
        }

        if (tvh.IntermediateStartTuple is not null)
        {
            toReturn.FormChild(nameof(tvh.IntermediateStartTuple), string.Join(", ", tvh.IntermediateStartTuple));
        }

        if (tvh.IntermediateEndTuple is not null)
        {
            toReturn.FormChild(nameof(tvh.IntermediateEndTuple), string.Join(", ", tvh.IntermediateEndTuple));
        }
        toReturn.FormChild(nameof(tvh.SerializedData), string.Join(", ", tvh.SerializedData));
        return toReturn;
    }

    public static TreeViewItem BuildCommonDeltaSetIndexMap(DeltaSetIndexMap map)
    {
        var toReturn = new TreeViewItem { Header = "Delta Set Index Map" };
        map.Maps.ForEach(m =>
        {
            TreeViewItem mdHeader = toReturn.FormChild("Map Data");
            mdHeader.FormChild(nameof(m.InnerIndex), m.InnerIndex);
            mdHeader.FormChild(nameof(m.OuterIndex), m.OuterIndex);
            mdHeader.FormChild(nameof(m.OriginalData), m.OriginalData);
        });
        return toReturn;
    }

    public static TreeViewItem BuildItemVariationStore(ItemVariationStore store)
    {
        var toReturn = new TreeViewItem { Header = "Item Variation Store" };
        toReturn.FormChild(nameof(store.VariationRegionListOffset), store.VariationRegionListOffset);
        TreeViewItem ivdHeader = toReturn.FormChild("Item Variation Data");
        store.ItemVariationData.ForEach(vd =>
        {
            ivdHeader.FormChild(nameof(vd.RegionIndexes), string.Join(", ", vd.RegionIndexes));
            TreeViewItem dSetsHeader = ivdHeader.FormChild("Delta Sets");
            vd.DeltaSets.ForEach(ds =>
            {
                dSetsHeader.FormChild(nameof(ds.DeltaData), string.Join(", ", ds.DeltaData));
            });
        });
        return toReturn;
    }

    public static TreeViewItem? BuildIPaintTable(IPaintTable pt)
    {
        TreeViewItem? toReturn = null;
        switch (pt)
        {
            case PaintColrLayers pcl:
                toReturn = new TreeViewItem { Header = "PaintColrLayers" };
                TreeViewItem paintTablesHeader = toReturn.FormChild("Paint Tables");
                pcl.PaintTables.ForEach(subTable =>
                {
                    paintTablesHeader.Items.Add(Utilities.BuildIPaintTable(subTable));
                });
                break;
            case PaintSolid ps:
                toReturn = new TreeViewItem { Header = "PaintSolid" };
                toReturn.FormChild(nameof(ps.PaletteIndex), ps.PaletteIndex);
                toReturn.FormChild(nameof(ps.Alpha), ps.Alpha);
                break;
            case PaintVarSolid pvs:
                toReturn = new TreeViewItem { Header = "PaintVarSolid" };
                toReturn.FormChild(nameof(pvs.PaletteIndex), pvs.PaletteIndex);
                toReturn.FormChild(nameof(pvs.Alpha), pvs.Alpha);
                toReturn.FormChild(nameof(pvs.VarIndexBase), pvs.VarIndexBase);
                break;
            case PaintLinearGradient plg:
                toReturn = new TreeViewItem { Header = "PaintLinearGradient" };
                toReturn.FormChild($"X0: {plg.X0}, Y0: {plg.Y0}");
                toReturn.FormChild($"X1: {plg.X1}, Y1: {plg.Y1}");
                toReturn.FormChild($"X2: {plg.X2}, Y2: {plg.Y2}");
                toReturn.Items.Add(BuildColorLine(plg.ColorLine));
                break;
            case PaintVarLinearGradient pvlg:
                toReturn = new TreeViewItem { Header = "PaintVarLinearGradient" };
                toReturn.FormChild($"X0: {pvlg.X0}, Y0: {pvlg.Y0}");
                toReturn.FormChild($"X1: {pvlg.X1}, Y1: {pvlg.Y1}");
                toReturn.FormChild($"X2: {pvlg.X2}, Y2: {pvlg.Y2}");
                toReturn.FormChild(nameof(pvlg.VarIndexBase), pvlg.VarIndexBase);
                toReturn.Items.Add(Utilities.BuildColorLine(pvlg.ColorLine));
                break;
            case PaintRadialGradient prg:
                toReturn = new TreeViewItem { Header = "PaintRadialGradient" };
                toReturn.FormChild($"X0: {prg.X0}, Y0: {prg.Y0}");
                toReturn.FormChild(nameof(prg.Radius0), prg.Radius0);
                toReturn.FormChild($"X1: {prg.X1}, Y1: {prg.Y1}");
                toReturn.FormChild(nameof(prg.Radius1), prg.Radius1);
                toReturn.Items.Add(BuildColorLine(prg.ColorLine));
                break;
            case PaintVarRadialGradient pvrg:
                toReturn = new TreeViewItem { Header = "PaintVarRadialGradient" };
                toReturn.FormChild($"X0: {pvrg.X0}, Y0: {pvrg.Y0}");
                toReturn.FormChild(nameof(pvrg.Radius0), pvrg.Radius0);
                toReturn.FormChild($"X1: {pvrg.X1}, Y1: {pvrg.Y1}");
                toReturn.FormChild(nameof(pvrg.Radius1), pvrg.Radius1);
                toReturn.FormChild(nameof(pvrg.VarIndexBase), pvrg.VarIndexBase);
                toReturn.Items.Add(BuildVarColorLine(pvrg.ColorLine));
                break;
            case PaintSweepGradient psg:
                toReturn = new TreeViewItem { Header = "PaintSweepGradient" };
                toReturn.FormChild(nameof(psg.StartAngle), psg.StartAngle);
                toReturn.FormChild(nameof(psg.EndAngle), psg.EndAngle);
                toReturn.FormChild($"Center: {psg.CenterX}, {psg.CenterY}");
                toReturn.Items.Add(BuildColorLine(psg.ColorLine));
                break;
            case PaintVarSweepGradient pvsg:
                toReturn = new TreeViewItem { Header = "PaintVarSweepGradient" };
                toReturn.FormChild(nameof(pvsg.VarIndexBase), pvsg.VarIndexBase);
                toReturn.FormChild(nameof(pvsg.StartAngle), pvsg.StartAngle);
                toReturn.FormChild(nameof(pvsg.EndAngle), pvsg.EndAngle);
                toReturn.FormChild($"Center: {pvsg.CenterX}, {pvsg.CenterY}");
                toReturn.Items.Add(BuildVarColorLine(pvsg.ColorLine));
                break;
            case PaintGlyph pg:
                toReturn = new TreeViewItem { Header = "PaintGlyph" };
                toReturn.FormChild(nameof(pg.GlyphId), pg.GlyphId);
                toReturn.FormChild(nameof(pg.PaintOffset), pg.PaintOffset);
                toReturn.Items.Add(BuildIPaintTable(pg.PaintTable));
                break;
            case PaintColrGlyph pcg:
                toReturn = new TreeViewItem { Header = "PaintColrGlyph" };
                toReturn.FormChild(nameof(pcg.GlyphId), pcg.GlyphId);
                break;
            case PaintTransform ptrans:
                toReturn = new TreeViewItem { Header = "PaintTransform" };
                toReturn.FormChild(ptrans.Transform.ToString());
                toReturn.Items.Add(BuildIPaintTable(ptrans.SubTable));
                break;
            case PaintVarTransform pvtrans:
                toReturn = new TreeViewItem { Header = "PaintVarTransform" };
                toReturn.FormChild(pvtrans.Transform.ToString());
                toReturn.Items.Add(BuildIPaintTable(pvtrans.SubTable));
                break;
            case PaintTranslate pxlate:
                toReturn = new TreeViewItem { Header = "PaintTranslate" };
                toReturn.FormChild($"Translate: dX: {pxlate.Dx}, dY: {pxlate.Dy}");
                toReturn.Items.Add(BuildIPaintTable(pxlate.SubTable));
                break;
            case PaintVarTranslate pvt:
                toReturn = new TreeViewItem { Header = "PaintVarTranslate" };
                toReturn.FormChild(nameof(pvt.VarIndexBase), pvt.VarIndexBase);
                toReturn.FormChild($"Translate: dX: {pvt.Dx}, dY: {pvt.Dy}");
                toReturn.Items.Add(BuildIPaintTable(pvt.SubTable));
                break;
            case PaintScale pscale:
                toReturn = new TreeViewItem { Header = "PaintScale" };
                toReturn.FormChild($"Scale: {pscale.ScaleX}, {pscale.ScaleY}");
                toReturn.Items.Add(BuildIPaintTable(pscale.SubTable));
                break;
            case PaintVarScale pvscale:
                toReturn = new TreeViewItem { Header = "PaintVarScale" };
                toReturn.FormChild(nameof(pvscale.VarIndexBase), pvscale.VarIndexBase);
                toReturn.FormChild($"Scale: {pvscale.ScaleX}, {pvscale.ScaleY}");
                toReturn.Items.Add(BuildIPaintTable(pvscale.SubTable));
                break;
            case PaintScaleAroundCenter psac:
                toReturn = new TreeViewItem { Header = "PaintScaleAroundCenter" };
                toReturn.FormChild($"Center: {psac.CenterX}, {psac.CenterY}");
                toReturn.FormChild($"Scale: {psac.ScaleX}, {psac.ScaleY}");
                toReturn.Items.Add(BuildIPaintTable(psac.SubTable));
                break;
            case PaintVarScaleAroundCenter pvsac:
                toReturn = new TreeViewItem { Header = "PaintVarScaleAroundCenter" };
                toReturn.FormChild(nameof(pvsac.VarIndexBase), pvsac.VarIndexBase);
                toReturn.FormChild($"Center: {pvsac.CenterX}, {pvsac.CenterY}");
                toReturn.FormChild($"Scale: {pvsac.ScaleX}, {pvsac.ScaleY}");
                toReturn.Items.Add(BuildIPaintTable(pvsac.SubTable));
                break;
            case PaintScaleUniform psu:
                toReturn = new TreeViewItem { Header = "PaintScaleUniform" };
                toReturn.FormChild(nameof(psu.Scale), psu.Scale);
                toReturn.Items.Add(BuildIPaintTable(psu.SubTable));
                break;
            case PaintVarScaleUniform pvsu:
                toReturn = new TreeViewItem { Header = "PaintVarScaleUniform" };
                toReturn.FormChild(nameof(pvsu.VarIndexBase), pvsu.VarIndexBase);
                toReturn.FormChild(nameof(pvsu.Scale), pvsu.Scale);
                toReturn.Items.Add(BuildIPaintTable(pvsu.SubTable));
                break;
            case PaintScaleUniformAroundCenter psuac:
                toReturn = new TreeViewItem { Header = "PaintScaleUniformAroundCenter" };
                toReturn.FormChild($"Center: {psuac.CenterX}, {psuac.CenterY}");
                toReturn.FormChild(nameof(psuac.Scale), psuac.Scale);
                toReturn.Items.Add(BuildIPaintTable(psuac.SubTable));
                break;
            case PaintVarScaleUniformAroundCenter pvsuac:
                toReturn = new TreeViewItem { Header = "PaintVarScaleUniformAroundCenter" };
                toReturn.FormChild(nameof(pvsuac.VarIndexBase), pvsuac.VarIndexBase);
                toReturn.FormChild($"Center: {pvsuac.CenterX}, {pvsuac.CenterY}");
                toReturn.FormChild(nameof(pvsuac.Scale), pvsuac.Scale);
                toReturn.Items.Add(BuildIPaintTable(pvsuac.SubTable));
                break;
            case PaintRotate pr:
                toReturn = new TreeViewItem { Header = "PaintRotate" };
                toReturn.FormChild(nameof(pr.Angle), pr.Angle);
                toReturn.Items.Add(BuildIPaintTable(pr.SubTable));
                break;
            case PaintVarRotate pvr:
                toReturn = new TreeViewItem { Header = "PaintVarRotate" };
                toReturn.FormChild(nameof(pvr.VarIndexBase), pvr.VarIndexBase);
                toReturn.FormChild(nameof(pvr.Angle), pvr.Angle);
                toReturn.Items.Add(BuildIPaintTable(pvr.SubTable));
                break;
            case PaintRotateAroundCenter prac:
                toReturn = new TreeViewItem { Header = "PaintRotateAroundCenter" };
                toReturn.FormChild($"Center: {prac.CenterX}, {prac.CenterY}");
                toReturn.FormChild(nameof(prac.Angle), prac.Angle);
                toReturn.Items.Add(BuildIPaintTable(prac.SubTable));
                break;
            case PaintVarRotateAroundCenter pvrac:
                toReturn = new TreeViewItem { Header = "PaintVarRotateAroundCenter" };
                toReturn.FormChild(nameof(pvrac.VarIndexBase), pvrac.VarIndexBase);
                toReturn.FormChild($"Center: {pvrac.CenterX}, {pvrac.CenterY}");
                toReturn.FormChild(nameof(pvrac.Angle), pvrac.Angle);
                toReturn.Items.Add(BuildIPaintTable(pvrac.SubTable));
                break;
            case PaintSkew pskew:
                toReturn = new TreeViewItem { Header = "PaintSkew" };
                toReturn.FormChild(nameof(pskew.XSkewAngle), pskew.XSkewAngle);
                toReturn.FormChild(nameof(pskew.YSkewAngle), pskew.YSkewAngle);
                toReturn.Items.Add(BuildIPaintTable(pskew.SubTable));
                break;
            case PaintVarSkew pvSkew:
                toReturn = new TreeViewItem { Header = "PaintVarSkew" };
                toReturn.FormChild(nameof(pvSkew.VarIndexBase), pvSkew.VarIndexBase);
                toReturn.FormChild(nameof(pvSkew.XSkewAngle), pvSkew.XSkewAngle);
                toReturn.FormChild(nameof(pvSkew.YSkewAngle), pvSkew.YSkewAngle);
                toReturn.Items.Add(BuildIPaintTable(pvSkew.SubTable));
                break;
            case PaintSkewAroundCenter psac:
                toReturn = new TreeViewItem { Header = "PaintSkewAroundCenter" };
                toReturn.FormChild($"Center: {psac.CenterX}, {psac.CenterY}");
                toReturn.FormChild(nameof(psac.XSkewAngle), psac.XSkewAngle);
                toReturn.FormChild(nameof(psac.YSkewAngle), psac.YSkewAngle);
                toReturn.Items.Add(BuildIPaintTable(psac.SubTable));
                break;
            case PaintVarSkewAroundCenter pvsac:
                toReturn = new TreeViewItem { Header = "PaintVarSkewAroundCenter" };
                toReturn.FormChild(nameof(pvsac.VarIndexBase), pvsac.VarIndexBase);
                toReturn.FormChild($"Center: {pvsac.CenterX}, {pvsac.CenterY}");
                toReturn.FormChild(nameof(pvsac.XSkewAngle), pvsac.XSkewAngle);
                toReturn.FormChild(nameof(pvsac.YSkewAngle), pvsac.YSkewAngle);
                toReturn.Items.Add(BuildIPaintTable(pvsac.SubTable));
                break;
            case PaintComposite pc:
                toReturn = new TreeViewItem { Header = "PaintComposite" };
                toReturn.FormChild(nameof(pc.CompositeMode), pc.CompositeMode);
                TreeViewItem bdHeader = toReturn.FormChild("Backdrop");
                bdHeader.Items.Add(BuildIPaintTable(pc.Backdrop));
                TreeViewItem sHeader = toReturn.FormChild("Source Table");
                sHeader.Items.Add(BuildIPaintTable(pc.SourceTable));
                break;
        }

        return toReturn;
    }

    public static TreeViewItem BuildColorLine(ColorLine colorLine)
    {
        var toReturn = new TreeViewItem { Header = "ColorLine" };
        toReturn.FormChild(nameof(colorLine.ExtendMode), colorLine.ExtendMode);
        TreeViewItem colorStopsHeader = toReturn.FormChild("Color Stops");
        colorLine.ColorStops.ForEach(cs =>
        {
            TreeViewItem csHeader = colorStopsHeader.FormChild("Color Stop");
            csHeader.FormChild(nameof(cs.PaletteIndex), cs.PaletteIndex);
            csHeader.FormChild(nameof(cs.StopOffset), cs.StopOffset);
            csHeader.FormChild(nameof(cs.Alpha), cs.Alpha);
        });
        return toReturn;
    }

    public static TreeViewItem BuildVarColorLine(VarColorLine vcl)
    {
        var toReturn = new TreeViewItem { Header = "VarColorLine" };
        toReturn.FormChild(nameof(vcl.ExtendMode), vcl.ExtendMode);
        TreeViewItem colorStopsHeader = toReturn.FormChild("Var Color Stops");
        vcl.ColorStops.ForEach(cs =>
        {
            TreeViewItem vcsHeader = colorStopsHeader.FormChild("Var Color Stop");
            vcsHeader.FormChild(nameof(cs.VarIndexBase), cs.VarIndexBase);
            vcsHeader.FormChild(nameof(cs.PaletteIndex), cs.PaletteIndex);
            vcsHeader.FormChild(nameof(cs.StopOffset), cs.StopOffset);
            vcsHeader.FormChild(nameof(cs.Alpha), cs.Alpha);
        });
        return toReturn;
    }
}