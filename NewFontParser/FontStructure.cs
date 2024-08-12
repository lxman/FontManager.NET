using System.Collections.Generic;
using NewFontParser.Models;
using NewFontParser.Tables;
using NewFontParser.Tables.Cmap;
using NewFontParser.Tables.Head;
using NewFontParser.Tables.Hhea;
using NewFontParser.Tables.Hmtx;
using NewFontParser.Tables.Name;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;
using Serilog;

namespace NewFontParser
{
    public class FontStructure
    {
        public FileType FileType { get; set; }

        public ushort TableCount { get; set; }

        public ushort SearchRange { get; set; }

        public ushort EntrySelector { get; set; }

        public ushort RangeShift { get; set; }

        public List<TableRecord> TableRecords { get; set; } = new List<TableRecord>();

        public List<IInfoTable> Tables { get; set; } = new List<IInfoTable>();

        private HheaTable? _hheaTable;

        private MaxPTable? _maxPTable;

        private LocaTable? _locaTable;

        private HeadTable? _headTable;

        private readonly string _path;

        public FontStructure(string path)
        {
            _path = path;
        }

        internal void Process()
        {
            ProcessCmap();
            ProcessHead();
            ProcessHhea();
            ProcessMaxp();
            ProcessHmtx();
            ProcessName();
            ProcessOs2();
            ProcessPost();
            ProcessCvt();
            ProcessFpgm();
            ProcessLoca();
            ProcessGlyf();
        }

        private void ProcessCmap()
        {
            Log.Debug("Processing cmap");
            byte[] cmapData = TableRecords.Find(x => x.Tag == "cmap").Data;
            Tables.Add(new CmapTable(cmapData));
            Log.Debug("cmap success");
        }

        private void ProcessHead()
        {
            Log.Debug("Processing head");
            byte[] headData = TableRecords.Find(x => x.Tag == "head").Data;
            _headTable = new HeadTable(headData);
            Tables.Add(_headTable);
            Log.Debug("head success");
        }

        private void ProcessHhea()
        {
            Log.Debug("Processing hhea");
            byte[] hheaData = TableRecords.Find(x => x.Tag == "hhea").Data;
            _hheaTable = new HheaTable(hheaData);
            Tables.Add(_hheaTable);
            Log.Debug("hhea success");
        }

        private void ProcessMaxp()
        {
            Log.Debug("Processing maxp");
            byte[] maxpData = TableRecords.Find(x => x.Tag == "maxp").Data;
            _maxPTable = new MaxPTable(maxpData);
            Tables.Add(_maxPTable);
            Log.Debug("maxp success");
        }

        private void ProcessHmtx()
        {
            Log.Debug("Processing hmtx");
            byte[] hmtxData = TableRecords.Find(x => x.Tag == "hmtx").Data;
            Tables.Add(new HmtxTable(hmtxData, _hheaTable!.NumberOfHMetrics, _maxPTable!.NumGlyphs));
            Log.Debug("hmtx success");
        }

        private void ProcessName()
        {
            Log.Debug("Processing name");
            byte[] nameData = TableRecords.Find(x => x.Tag == "name").Data;
            var version = (ushort)(nameData[0] << 8 | nameData[1]);
            switch (version)
            {
                case 0:
                    Tables.Add(new NameTable0(nameData));
                    Log.Debug("Added name table 0");
                    break;
                case 1:
                    Tables.Add(new NameTable1(nameData));
                    Log.Debug("Added name table 1");
                    break;
            }
            Log.Debug("name success");
        }

        private void ProcessOs2()
        {
            Log.Debug("Processing OS/2");
            byte[] os2Data = TableRecords.Find(x => x.Tag == "OS/2").Data;
            Tables.Add(new Os2Table(os2Data));
            Log.Debug("OS/2 success");
        }

        private void ProcessPost()
        {
            Log.Debug("Processing post");
            byte[] postData = TableRecords.Find(x => x.Tag == "post").Data;
            Tables.Add(new PostTable(postData));
            Log.Debug("post success");
        }

        private void ProcessCvt()
        {
            if (!TableRecords.Exists(x => x.Tag == "cvt "))
            {
                return;
            }
            Log.Debug("Processing cvt");
            byte[] cvtData = TableRecords.Find(x => x.Tag == "cvt ").Data;
            Tables.Add(new CvtTable(cvtData));
            Log.Debug("cvt success");
        }

        private void ProcessFpgm()
        {
            if (!TableRecords.Exists(x => x.Tag == "fpgm"))
            {
                return;
            }
            Log.Debug("Processing fpgm");
            byte[] fpgmData = TableRecords.Find(x => x.Tag == "fpgm").Data;
            Tables.Add(new FpgmTable(fpgmData));
            Log.Debug("fpgm success");
        }

        private void ProcessLoca()
        {
            if (!TableRecords.Exists(x => x.Tag == "loca"))
            {
                return;
            }
            Log.Debug("Processing loca");
            byte[] locaData = TableRecords.Find(x => x.Tag == "loca").Data;
            _locaTable = new LocaTable(locaData, _maxPTable!.NumGlyphs, _headTable!.IndexToLocFormat == IndexToLocFormat.Offset16);
            Tables.Add(_locaTable);
            Log.Debug("loca success");
        }

        private void ProcessGlyf()
        {
            if (!TableRecords.Exists(x => x.Tag == "glyf"))
            {
                return;
            }
            Log.Debug("Processing glyf");
            byte[] glyfData = TableRecords.Find(x => x.Tag == "glyf").Data;
            Tables.Add(new Table(glyfData, _maxPTable!.NumGlyphs, _locaTable!));
            Log.Debug("glyf success");
        }
    }
}