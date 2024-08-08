using System.Collections.Generic;
using NewFontParser.Models;
using NewFontParser.Tables;
using NewFontParser.Tables.Cmap;
using NewFontParser.Tables.Head;
using NewFontParser.Tables.Hhea;
using NewFontParser.Tables.Hmtx;
using NewFontParser.Tables.Name;

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

        internal void Process()
        {
            ProcessCmap();
            ProcessHead();
            ProcessHhea();
            ProcessMaxp();
            ProcessHmtx();
            ProcessName();
        }

        private void ProcessCmap()
        {
            byte[] cmapData = TableRecords.Find(x => x.Tag == "cmap").Data;
            Tables.Add(new CmapTable(cmapData));
        }

        private void ProcessHead()
        {
            byte[] headData = TableRecords.Find(x => x.Tag == "head").Data;
            Tables.Add(new HeadTable(headData));
        }

        private void ProcessHhea()
        {
            byte[] hheaData = TableRecords.Find(x => x.Tag == "hhea").Data;
            _hheaTable = new HheaTable(hheaData);
            Tables.Add(_hheaTable);
        }

        private void ProcessMaxp()
        {
            byte[] maxpData = TableRecords.Find(x => x.Tag == "maxp").Data;
            _maxPTable = new MaxPTable(maxpData);
            Tables.Add(_maxPTable);
        }

        private void ProcessHmtx()
        {
            byte[] hmtxData = TableRecords.Find(x => x.Tag == "hmtx").Data;
            Tables.Add(new HmtxTable(hmtxData, _hheaTable!.NumberOfHMetrics, _maxPTable!.NumGlyphs));
        }

        private void ProcessName()
        {
            byte[] nameData = TableRecords.Find(x => x.Tag == "name").Data;
            var version = (ushort)(nameData[0] << 8 | nameData[1]);
            switch (version)
            {
                case 0:
                    Tables.Add(new NameTable0(nameData));
                    break;
                case 1:
                    Tables.Add(new NameTable1(nameData));
                    break;
            }
        }
    }
}