using System;
using System.Collections.Generic;
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

namespace NewFontParser
{
    // These are the tables that we know about.
    public static class Table
    {
        public static List<Type> Types = new List<Type>
        {
            typeof(CmapTable),
            typeof(HeadTable),
            typeof(HheaTable),
            typeof(MaxPTable),
            typeof(HmtxTable),
            typeof(NameTable),
            typeof(Os2Table),
            typeof(PostTable),
            typeof(CvtTable),
            typeof(FpgmTable),
            typeof(LocaTable),
            typeof(GlyphTable),
            typeof(PrepTable),
            typeof(GaspTable),
            typeof(DsigTable),
            typeof(HdmxTable),
            typeof(LtshTable),
            typeof(VheaTable),
            typeof(VmtxTable),
            typeof(GdefTable),
            typeof(VdmxTable),
            typeof(GposTable),
            typeof(GsubTable),
            typeof(Type1Table),
            typeof(MathTable),
            typeof(FftmTable),
            typeof(SvgTable),
            typeof(BaseTable),
            typeof(MorxTable),
            typeof(Tables.Proprietary.Aat.Feat.FeatTable),
            typeof(HvarTable),
            typeof(MvarTable),
            typeof(StatTable),
            typeof(FvarTable),
            typeof(GvarTable),
            typeof(AvarTable),
            typeof(CvarTable),
            typeof(PfedTable),
            typeof(TtfaTable),
            typeof(PropTable),
            typeof(EblcTable),
            typeof(EbdtTable),
            typeof(CbdtTable),
            typeof(CblcTable),
            typeof(EbscTable),
            typeof(TexTable),
            typeof(PcltTable),
            typeof(BdfTable),
            typeof(VorgTable),
            typeof(WebfTable),
            typeof(KernTable),
            typeof(MetaTable),
            typeof(JstfTable),
            typeof(MergTable),
            typeof(GlatTable),
            typeof(GlocTable),
            typeof(SilfTable),
            typeof(SillTable),
            typeof(BlocTable),
            typeof(BdatTable),
            typeof(KerxTable),
            typeof(ZapfTable),
            typeof(TsidTable),
            typeof(TsifTable),
            typeof(TsipTable),
            typeof(TsisTable),
            typeof(TsivTable),
            typeof(Tables.Todo.Graphite.Feat.FeatTable),
            typeof(CpalTable),
            typeof(ColrTable),
            typeof(SiltTable)
        };
    }
}