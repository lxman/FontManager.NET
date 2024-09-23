using System.Windows.Controls;
using NewFontParser.Tables.Base;
using NewFontParser.Tables.Common;
using NewFontParser.Tables.Common.ClassDefinition;
using NewFontParser.Tables.Common.CoverageFormat;
using NewFontParser.Tables.Gpos.LookupSubtables.AnchorTable;
using NewFontParser.Tables.Gpos.LookupSubtables.Common;
using NewFontParser.Tables.Gpos.LookupSubtables.MarkMarkPos;
using NewFontParser.Tables.Math;

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
}