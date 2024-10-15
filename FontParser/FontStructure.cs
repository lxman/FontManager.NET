﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FontParser.Models;
using FontParser.RenderFont.Interpreter;
using FontParser.Tables;
using FontParser.Tables.Bitmap.Ebdt;
using FontParser.Tables.Bitmap.Eblc;
using FontParser.Tables.Cmap;
using FontParser.Tables.Cvar;
using FontParser.Tables.Fvar;
using FontParser.Tables.Head;
using FontParser.Tables.Hhea;
using FontParser.Tables.Hmtx;
using FontParser.Tables.Optional;
using FontParser.Tables.Optional.Hdmx;
using FontParser.Tables.Svg;
using FontParser.Tables.TtTables;
using FontParser.Tables.TtTables.Glyf;

namespace FontParser
{
    public class FontStructure
    {
        public FileType FileType { get; set; }

        public ushort TableCount { get; set; }

        public ushort SearchRange { get; set; }

        public ushort EntrySelector { get; set; }

        public ushort RangeShift { get; set; }

        public List<TableRecord> TableRecords { get; set; } = new List<TableRecord>();

        public List<IFontTable> Tables { get; } = new List<IFontTable>();

        private readonly List<TableStatusRecord> _tables = new List<TableStatusRecord>();
        private readonly string _currentFile;
        private readonly ConcurrentBag<IFontTable> _fontTables = new ConcurrentBag<IFontTable>();
        private readonly ConcurrentBag<SucceededStatusRecord> _succeeded = new ConcurrentBag<SucceededStatusRecord>();
        private const bool InterpreterTest = false;

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
            _tables.AddRange(TableRecords.Select(r => new TableStatusRecord { Name = r.Tag }));
        }

        internal void ProcessParallel(
            bool deferGlyphWoff2 = false,
            bool deferLocaWoff2 = false,
            bool deferHmtxWoff2 = false)
        {
            Parallel.ForEach(Table.Types, (table) =>
            {
                PropertyInfo[] properties = table.GetProperties(BindingFlags.Static | BindingFlags.Public);
                if (!(properties[0].GetValue(null) is string tag)) return;
                var succeededRecord = new SucceededStatusRecord { Name = tag, Attempted = true };
                switch (tag)
                {
                    case "glyf" when deferGlyphWoff2:
                    case "loca" when deferLocaWoff2:
                    case "hmtx" when deferHmtxWoff2:
                        succeededRecord.Succeeded = true;
                        _succeeded.Add(succeededRecord);
                        return;
                }

                TableRecord? tableRecord = TableRecords.FirstOrDefault(r => r.Tag == tag);
                if (tableRecord is null) return;
                if (!(Activator.CreateInstance(table, tableRecord.Data) is IFontTable fontTable)) return;
                _fontTables.Add(fontTable);
                succeededRecord.Succeeded = true;
                _succeeded.Add(succeededRecord);
            });
            Tables.AddRange(_fontTables);
            _fontTables.Clear();
            PostProcess();
        }

        private void PostProcess(
            bool deferGlyphWoff2 = false,
            bool deferLocaWoff2 = false,
            bool deferHmtxWoff2 = false)
        {
            (Tables.Find(x => x is VmtxTable) as VmtxTable)?.Process(GetTable<VheaTable>().NumberOfLongVerMetrics);
            (Tables.Find(x => x is HdmxTable) as HdmxTable)?.Process(GetTable<MaxPTable>().NumGlyphs);
            (Tables.Find(x => x is LtshTable) as LtshTable)?.Process(GetTable<MaxPTable>().NumGlyphs);
            (Tables.Find(x => x is EbdtTable) as EbdtTable)?.Process(GetTable<EblcTable>());
            (Tables.Find(x => x is CvarTable) as CvarTable)?.Process(GetTable<FvarTable>().Axes.Count);
            if (!deferLocaWoff2)
            {
                (Tables.Find(x => x is LocaTable) as LocaTable)?.Process(GetTable<MaxPTable>().NumGlyphs, GetTable<HeadTable>().IndexToLocFormat == IndexToLocFormat.Offset16);
            }

            if (!deferGlyphWoff2)
            {
                (Tables.Find(x => x is GlyphTable) as GlyphTable)?.Process(GetTable<MaxPTable>().NumGlyphs, GetTable<LocaTable>());
            }

            if (!deferHmtxWoff2)
            {
                (Tables.Find(x => x is HmtxTable) as HmtxTable)?.Process(GetTable<HheaTable>().NumberOfHMetrics, GetTable<MaxPTable>().NumGlyphs);
            }

            foreach (SucceededStatusRecord? record in _succeeded)
            {
                TableStatusRecord? tsRecord = _tables.FirstOrDefault(r => r.Name == record.Name);
                if (tsRecord is null) continue;
                if (record.Succeeded) _tables.Remove(tsRecord);
                tsRecord.Attempted = true;
            }

            // For testing the interpreter
            if (InterpreterTest)
            {
                var cmapTable = GetTable<CmapTable>();
                var glyphTable = GetTable<GlyphTable>();
                var hmtxTable = GetTable<HmtxTable>();
                ushort demoId = cmapTable.GetGlyphId(0x41);
                GlyphData? demoData = glyphTable.GetGlyphData(demoId);
                if (!(demoData?.GlyphSpec is SimpleGlyph glyphData)) return;
                List<byte> instructions = glyphData.Instructions;
                var cvtTable = GetTable<CvtTable>();
                GraphicsState? graphicsState = null;
                Dictionary<int, byte[]>? functions = null;
                var maxpTable = GetTable<MaxPTable>();
                if (Tables.Find(x => x is FpgmTable) is FpgmTable fpgmTable)
                {
                    var interpreter = new Interpreter(
                        glyphTable,
                        cvtTable,
                        fpgmTable.Instructions,
                        maxpTable);
                    interpreter.Execute();
                    graphicsState = interpreter.GraphicsState;
                    functions = interpreter.Functions;
                }
                if (!(graphicsState is null))
                {
                    var interpreter = new Interpreter(
                        glyphTable,
                        demoData,
                        cvtTable,
                        hmtxTable,
                        maxpTable,
                        graphicsState,
                        functions,
                        instructions);
                    interpreter.Execute();
                }
            }

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

        public DocumentIndex GetSvgDocumentIndex()
        {
            return GetTable<SvgTable>().Documents;
        }

        private T GetTable<T>() where T : IFontTable
        {
            IFontTable table = Tables.Find(x => x is T);
            return (T)table;
        }
    }
}