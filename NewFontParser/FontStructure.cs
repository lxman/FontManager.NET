using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NewFontParser.Models;
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
using NewFontParser.Tables.Proprietary.Aat.Bdat;
using NewFontParser.Tables.Proprietary.Aat.Bloc;
using NewFontParser.Tables.Proprietary.Aat.Kerx;
using NewFontParser.Tables.Proprietary.Aat.Morx;
using NewFontParser.Tables.Proprietary.Aat.Prop;
using NewFontParser.Tables.Proprietary.Aat.Zapf;
using NewFontParser.Tables.Proprietary.Bdf;
using NewFontParser.Tables.Proprietary.Pclt;
using NewFontParser.Tables.Proprietary.Pfed;
using NewFontParser.Tables.Proprietary.Tex;
using NewFontParser.Tables.Proprietary.Webf;
using NewFontParser.Tables.Stat;
using NewFontParser.Tables.Svg;
using NewFontParser.Tables.Todo.Arabic.Tsid;
using NewFontParser.Tables.Todo.Arabic.Tsif;
using NewFontParser.Tables.Todo.Arabic.Tsip;
using NewFontParser.Tables.Todo.Arabic.Tsis;
using NewFontParser.Tables.Todo.Arabic.Tsiv;
using NewFontParser.Tables.Todo.Graphite.Glat;
using NewFontParser.Tables.Todo.Graphite.Gloc;
using NewFontParser.Tables.Todo.Graphite.Silf;
using NewFontParser.Tables.Todo.Graphite.Sill;
using NewFontParser.Tables.Todo.Graphite.Silt;
using NewFontParser.Tables.Todo.Ttfa;
using NewFontParser.Tables.TtTables;
using NewFontParser.Tables.TtTables.Glyf;
using NewFontParser.Tables.Vdmx;
using NewFontParser.Tables.Vorg;
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

        public GlyphTable? GetGlyphTable()
        {
            return Tables.Find(x => x is GlyphTable) as GlyphTable;
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
            ProcessTable<SvgTable>();
            ProcessTable<BaseTable>();
            ProcessTable<MorxTable>();
            ProcessTable<Tables.Proprietary.Aat.Feat.FeatTable>();
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
            ProcessTable<EblcTable>();
            ProcessTable<EbdtTable>();
            ProcessTable<CbdtTable>();
            ProcessTable<CblcTable>();
            ProcessTable<EbscTable>();
            ProcessTable<TexTable>();
            ProcessTable<PcltTable>();
            ProcessTable<BdfTable>();
            ProcessTable<VorgTable>();
            ProcessTable<WebfTable>();
            ProcessTable<KernTable>();
            ProcessTable<MetaTable>();
            ProcessTable<JstfTable>();
            ProcessTable<MergTable>();
            ProcessTable<GlatTable>();
            ProcessTable<GlocTable>();
            ProcessTable<SilfTable>();
            ProcessTable<SillTable>();
            ProcessTable<BlocTable>();
            ProcessTable<BdatTable>();
            ProcessTable<KerxTable>();
            ProcessTable<ZapfTable>();
            ProcessTable<TsidTable>();
            ProcessTable<TsifTable>();
            ProcessTable<TsipTable>();
            ProcessTable<TsisTable>();
            ProcessTable<TsivTable>();
            ProcessTable<Tables.Todo.Graphite.Feat.FeatTable>();
            // If the Cpal table is missing don't try to process the Colr table
            if (ProcessTable<CpalTable>()) ProcessTable<ColrTable>();
            ProcessTable<SiltTable>();
            (Tables.Find(x => x is VmtxTable) as VmtxTable)?.Process(GetTable<VheaTable>().NumberOfLongVerMetrics);
            (Tables.Find(x => x is HdmxTable) as HdmxTable)?.Process(GetTable<MaxPTable>().NumGlyphs);
            (Tables.Find(x => x is LocaTable) as LocaTable)?.Process(GetTable<MaxPTable>().NumGlyphs, GetTable<HeadTable>().IndexToLocFormat == IndexToLocFormat.Offset16);
            (Tables.Find(x => x is GlyphTable) as GlyphTable)?.Process(GetTable<MaxPTable>().NumGlyphs, GetTable<LocaTable>());
            (Tables.Find(x => x is HmtxTable) as HmtxTable)?.Process(GetTable<HheaTable>().NumberOfHMetrics, GetTable<MaxPTable>().NumGlyphs);
            (Tables.Find(x => x is LtshTable) as LtshTable)?.Process(GetTable<MaxPTable>().NumGlyphs);
            (Tables.Find(x => x is EbdtTable) as EbdtTable)?.Process(GetTable<EblcTable>());
            if (!(Tables.Find(x => x is CmapTable) is CmapTable cmapTable)) return;
            if (!(Tables.Find(x => x is GlyphTable) is GlyphTable glyphTable)) return;
            Console.Write("Glyph Ids: ");
            for (ushort i = 0; i < 0x7F; i++)
            {
                ushort glyphId = cmapTable.GetGlyphId(i);
                Console.Write($"{glyphId}, ");
                GlyphData? data = glyphTable.GetGlyphData(glyphId);
            }
            Console.WriteLine();

            // For testing the interpreter
            //ushort demoId = cmapTable.GetGlyphId(0x41);
            //GlyphData? demoData = glyphTable.GetGlyphData(demoId);
            //if (!(demoData?.GlyphSpec is SimpleGlyph glyphData)) return;
            //byte[] instructions = glyphData.Instructions;
            //var cvtTable = GetTable<CvtTable>();
            //GraphicsState? graphicsState = null;
            //Dictionary<int, byte[]>? functions = null;
            //var maxpTable = GetTable<MaxPTable>();
            //if (Tables.Find(x => x is FpgmTable) is FpgmTable fpgmTable)
            //{
            //    var interpreter = new Interpreter(
            //        glyphTable,
            //        cvtTable,
            //        fpgmTable.Instructions,
            //        maxpTable);
            //    interpreter.Execute();
            //    graphicsState = interpreter.GraphicsState;
            //    functions = interpreter.Functions;
            //}
            //if (!(graphicsState is null))
            //{
            //    var interpreter = new Interpreter(
            //        glyphTable,
            //        demoData,
            //        cvtTable,
            //        maxpTable,
            //        graphicsState,
            //        functions,
            //        instructions);
            //    interpreter.Execute();
            //}

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

        public DocumentIndex? GetSvgDocumentIndex()
        {
            return GetTable<SvgTable>().Documents;
        }

        private bool ProcessTable<T>() where T : IInfoTable
        {
            string tag;
            try
            {
                tag = typeof(T).GetProperty("Tag", BindingFlags.Static | BindingFlags.Public)?.GetValue(null).ToString() ?? string.Empty;
            }
            catch (Exception e)
            {
                Log.Debug($"Tag name not found for table {typeof(T).FullName}");
                return false;
            }

            if (!TableRecords.Exists(x => x.Tag == tag))
            {
                //Log.Debug($"{tag} table entry not found");
                return false;
            }

            try
            {
                _tables.FirstOrDefault(t => t.Name == tag)!.Attempted = true;
                byte[] data = TableRecords.Find(x => x.Tag == tag).Data;
                Tables.Add((T)Activator.CreateInstance(typeof(T), data));
                _tables.Remove(_tables.FirstOrDefault(x => x.Name == tag));
            }
            catch (Exception e)
            {
                Log.Error(e, $"{typeof(T).Name} blew up");
                return false;
            }
            return true;
        }

        private T GetTable<T>() where T : IInfoTable
        {
            IInfoTable table = Tables.Find(x => x is T);
            return (T)table;
        }
    }
}