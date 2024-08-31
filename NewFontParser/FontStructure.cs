using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NewFontParser.Models;
using NewFontParser.Tables;
using NewFontParser.Tables.Base;
using NewFontParser.Tables.Cff.Type1;
using NewFontParser.Tables.Cmap;
using NewFontParser.Tables.Colr;
using NewFontParser.Tables.Cpal;
using NewFontParser.Tables.Fftm;
using NewFontParser.Tables.Gdef;
using NewFontParser.Tables.Gpos;
using NewFontParser.Tables.Gsub;
using NewFontParser.Tables.Head;
using NewFontParser.Tables.Hhea;
using NewFontParser.Tables.Hmtx;
using NewFontParser.Tables.Math;
using NewFontParser.Tables.Morx;
using NewFontParser.Tables.Name;
using NewFontParser.Tables.Optional;
using NewFontParser.Tables.Optional.Dsig;
using NewFontParser.Tables.Optional.Hdmx;
using NewFontParser.Tables.Svg;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;
using NewFontParser.Tables.Vdmx;
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

        private readonly List<string> _tables = new List<string>();

        private HheaTable? _hheaTable;

        private MaxPTable? _maxPTable;

        private LocaTable? _locaTable;

        private HeadTable? _headTable;

        private VheaTable? _vheaTable;

        private readonly string _path;

        public FontStructure(string path)
        {
            _path = path;
        }

        public void CollectTableNames()
        {
            _tables.AddRange(TableRecords.Select(r => r.Tag));
        }

        internal void Process()
        {
            ProcessTable<CmapTable>();
            ProcessTable<HeadTable>();
            _headTable = Tables.Find(x => x is HeadTable) as HeadTable;
            ProcessTable<HheaTable>();
            _hheaTable = Tables.Find(x => x is HheaTable) as HheaTable;
            ProcessTable<MaxPTable>();
            _maxPTable = Tables.Find(x => x is MaxPTable) as MaxPTable;
            ProcessTable<HmtxTable>();
            ProcessTable<NameTable>();
            ProcessTable<Os2Table>();
            ProcessTable<PostTable>();
            ProcessTable<CvtTable>();
            ProcessTable<FpgmTable>();
            ProcessTable<LocaTable>();
            _locaTable = Tables.Find(x => x is LocaTable) as LocaTable;
            ProcessTable<GlyphTable>();
            ProcessTable<PrepTable>();
            ProcessTable<GaspTable>();
            ProcessTable<DsigTable>();
            ProcessTable<HdmxTable>();
            ProcessTable<LtshTable>();
            ProcessTable<VheaTable>();
            _vheaTable = Tables.Find(x => x is VheaTable) as VheaTable;
            ProcessTable<VmtxTable>();
            ProcessTable<GdefTable>();
            ProcessTable<VdmxTable>();
            ProcessTable<GposTable>();
            ProcessTable<GsubTable>();
            ProcessTable<Type1Table>();
            ProcessTable<MathTable>();
            ProcessTable<FftmTable>();
            ProcessTable<ColrTable>();
            ProcessTable<CpalTable>();
            ProcessTable<SvgTable>();
            ProcessTable<BaseTable>();
            ProcessTable<MorxTable>();
            (Tables.Find(x => x is VmtxTable) as VmtxTable)?.Process(_vheaTable!.NumberOfLongVerMetrics);
            (Tables.Find(x => x is HdmxTable) as HdmxTable)?.Process(_maxPTable!.NumGlyphs);
            (Tables.Find(x => x is LocaTable) as LocaTable)?.Process(_maxPTable!.NumGlyphs, _headTable!.IndexToLocFormat == IndexToLocFormat.Offset16);
            (Tables.Find(x => x is GlyphTable) as GlyphTable)?.Process(_maxPTable!.NumGlyphs, _locaTable!);
            (Tables.Find(x => x is HmtxTable) as HmtxTable)?.Process(_hheaTable!.NumberOfHMetrics, _maxPTable!.NumGlyphs);
            (Tables.Find(x => x is LtshTable) as LtshTable)?.Process(_maxPTable!.NumGlyphs);
            if (!_tables.Any()) return;
            Console.WriteLine($"Remaining {_path.Split('\\').Last()} tables to parse:");
            _tables.ForEach(t => Console.WriteLine($"\t{t}"));
            Console.WriteLine();
        }

        private void ProcessTable<T>() where T : IInfoTable
        {
            string tag;
            try
            {
                tag = typeof(T).GetProperty("Tag", BindingFlags.Static | BindingFlags.Public)?.GetValue(null).ToString() ?? string.Empty;
            }
            catch (Exception e)
            {
                Log.Debug($"Tag name not found for table {typeof(T).FullName}");
                return;
            }

            if (!TableRecords.Exists(x => x.Tag == tag))
            {
                Log.Debug($"{tag} table entry not found");
                return;
            }

            Log.Debug($"Processing {tag}");
            byte[] data = TableRecords.Find(x => x.Tag == tag).Data;
            Tables.Add((T)Activator.CreateInstance(typeof(T), data));
            _tables.Remove(tag);
            Log.Debug($"{tag} success");
        }
    }
}