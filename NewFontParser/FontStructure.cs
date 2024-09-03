using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NewFontParser.Models;
using NewFontParser.Tables;
using NewFontParser.Tables.Aat.Feat;
using NewFontParser.Tables.Aat.Prop;
using NewFontParser.Tables.Avar;
using NewFontParser.Tables.Base;
using NewFontParser.Tables.Bitmap.Cbdt;
using NewFontParser.Tables.Bitmap.Cblc;
using NewFontParser.Tables.Bitmap.Ebdt;
using NewFontParser.Tables.Bitmap.Eblc;
using NewFontParser.Tables.Bitmap.Ebsc;
using NewFontParser.Tables.Cff.Type1;
using NewFontParser.Tables.Cmap;
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
using NewFontParser.Tables.Math;
using NewFontParser.Tables.Morx;
using NewFontParser.Tables.Mvar;
using NewFontParser.Tables.Name;
using NewFontParser.Tables.Optional;
using NewFontParser.Tables.Optional.Dsig;
using NewFontParser.Tables.Optional.Hdmx;
using NewFontParser.Tables.Pfed;
using NewFontParser.Tables.Proprietary.Tex;
using NewFontParser.Tables.Stat;
using NewFontParser.Tables.Svg;
using NewFontParser.Tables.Ttfa;
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

        private readonly List<TableStatusRecord> _tables = new List<TableStatusRecord>();

        private readonly string _currentFile;

        public FontStructure(string path)
        {
            _currentFile = path.Split("\\", StringSplitOptions.RemoveEmptyEntries)[^1];
        }

        public void CollectTableNames()
        {
            _tables.AddRange(TableRecords.Select(r => new TableStatusRecord() { Name = r.Tag }));
        }

        internal void Process()
        {
            ProcessTable<CmapTable>();
            ProcessTable<HeadTable>();
            ProcessTable<HheaTable>();
            ProcessTable<MaxPTable>();
            ProcessTable<HmtxTable>();
            ProcessTable<NameTable>();
            ProcessTable<Os2Table>();
            ProcessTable<PostTable>();
            ProcessTable<CvtTable>();
            ProcessTable<FpgmTable>();
            ProcessTable<LocaTable>();
            ProcessTable<GlyphTable>();
            ProcessTable<PrepTable>();
            ProcessTable<GaspTable>();
            ProcessTable<DsigTable>();
            ProcessTable<HdmxTable>();
            ProcessTable<LtshTable>();
            ProcessTable<VheaTable>();
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
            ProcessTable<FeatTable>();
            ProcessTable<HvarTable>();
            ProcessTable<MvarTable>();
            ProcessTable<StatTable>();
            ProcessTable<FvarTable>();
            ProcessTable<GvarTable>();
            ProcessTable<AvarTable>();
            ProcessTable<CvarTable>();
            ProcessTable<PfedTable>();
            ProcessTable<TtfaTable>();
            ProcessTable<PropTable>();
            ProcessTable<EbdtTable>();
            ProcessTable<CbdtTable>();
            ProcessTable<CblcTable>();
            ProcessTable<EblcTable>();
            ProcessTable<EbscTable>();
            ProcessTable<TexTable>();
            (Tables.Find(x => x is VmtxTable) as VmtxTable)?.Process(GetTable<VheaTable>().NumberOfLongVerMetrics);
            (Tables.Find(x => x is HdmxTable) as HdmxTable)?.Process(GetTable<MaxPTable>().NumGlyphs);
            (Tables.Find(x => x is LocaTable) as LocaTable)?.Process(GetTable<MaxPTable>().NumGlyphs, GetTable<HeadTable>().IndexToLocFormat == IndexToLocFormat.Offset16);
            (Tables.Find(x => x is GlyphTable) as GlyphTable)?.Process(GetTable<MaxPTable>().NumGlyphs, GetTable<LocaTable>());
            (Tables.Find(x => x is HmtxTable) as HmtxTable)?.Process(GetTable<HheaTable>().NumberOfHMetrics, GetTable<MaxPTable>().NumGlyphs);
            (Tables.Find(x => x is LtshTable) as LtshTable)?.Process(GetTable<MaxPTable>().NumGlyphs);
            if (!_tables.Any()) return;
            if (_tables.Any(t => !t.Attempted))
            {
                Console.WriteLine("Remaining tables to parse:");
                _tables.Where(t => !t.Attempted).ToList().ForEach(t => Console.WriteLine($"\t{t.Name}"));
                Console.WriteLine();
            }
            if (!_tables.Any(t => t.Attempted)) return;
            Console.WriteLine("Parsing failed for:");
            _tables.Where(t => t.Attempted).ToList().ForEach(t => Console.WriteLine($"\t{t.Name}"));
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

            try
            {
                Log.Debug($"Processing {tag}");
                _tables.FirstOrDefault(t => t.Name == tag)!.Attempted = true;
                byte[] data = TableRecords.Find(x => x.Tag == tag).Data;
                Tables.Add((T)Activator.CreateInstance(typeof(T), data));
                _tables.Remove(_tables.FirstOrDefault(x => x.Name == tag));
                Log.Debug($"{tag} success");
            }
            catch (Exception e)
            {
                Log.Error(e, $"{typeof(T).Name} blew up");
            }
        }

        private T GetTable<T>() where T : IInfoTable
        {
            IInfoTable table = Tables.Find(x => x is T);
            return (T)table;
        }
    }
}