using System;
using System.Text;
using FontParser.Tables;
using FontParser.Tables.AdvancedLayout.FontMath;
using FontParser.Tables.CFF.CFF;

namespace FontParser
{
    public class Glyph
    {
        /// <summary>
        /// glyph info has only essential layout detail (this is our extension)
        /// </summary>
        private readonly bool _onlyLayoutEssMode;

        private bool _hasOrgAdvWidth;       //FOUND in all mode

        internal Glyph(
            GlyphPointF[] glyphPoints,
            ushort[] contourEndPoints,
            Bounds bounds,
            byte[] glyphInstructions,
            ushort index)
        {
            //create from TTF

#if DEBUG
            dbugId = s_debugTotalId++;
            if (dbugId == 444)
            {
            }
#endif
            GlyphPoints = glyphPoints;
            EndPoints = contourEndPoints;
            Bounds = bounds;
            GlyphInstructions = glyphInstructions;
            GlyphIndex = index;
        }

        public ushort GlyphIndex { get; }                       //FOUND in all mode
        public Bounds Bounds { get; internal set; }             //FOUND in all mode
        public ushort OriginalAdvanceWidth { get; private set; } //FOUND in all mode
        internal ushort BitmapGlyphAdvanceWidth { get; set; }    //FOUND in all mode

        //TrueTypeFont
        public ushort[] EndPoints { get; private set; }         //NULL in  _onlyLayoutEssMode

        public GlyphPointF[] GlyphPoints { get; private set; }  //NULL in  _onlyLayoutEssMode
        internal byte[] GlyphInstructions { get; set; }         //NULL in _onlyLayoutEssMode
        public bool HasGlyphInstructions => GlyphInstructions != null; //FALSE  n _onlyLayoutEssMode

        //
        public GlyphClassKind GlyphClass { get; internal set; } //FOUND in all mode

        internal ushort MarkClassDef { get; set; }              //FOUND in all mode

        public short MinX => Bounds.XMin;
        public short MaxX => Bounds.XMax;
        public short MinY => Bounds.YMin;
        public short MaxY => Bounds.YMax;

        public static bool HasOriginalAdvancedWidth(Glyph glyph) => glyph._hasOrgAdvWidth;

        public static void SetOriginalAdvancedWidth(Glyph glyph, ushort advW)
        {
            glyph.OriginalAdvanceWidth = advW;
            glyph._hasOrgAdvWidth = true;
        }

        /// <summary>
        /// TrueType outline, offset glyph points
        /// </summary>
        /// <param name="glyph"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        internal static void TtfOffsetXY(Glyph glyph, short dx, short dy)
        {
            //change data on current glyph
            GlyphPointF[] glyphPoints = glyph.GlyphPoints;
            for (int i = glyphPoints.Length - 1; i >= 0; --i)
            {
                glyphPoints[i] = glyphPoints[i].Offset(dx, dy);
            }
            //-------------------------
            Bounds orgBounds = glyph.Bounds;
            glyph.Bounds = new Bounds(
               (short)(orgBounds.XMin + dx),
               (short)(orgBounds.YMin + dy),
               (short)(orgBounds.XMax + dx),
               (short)(orgBounds.YMax + dy));
        }

        /// <summary>
        ///TrueType outline, transform normal
        /// </summary>
        /// <param name="glyph"></param>
        /// <param name="m00"></param>
        /// <param name="m01"></param>
        /// <param name="m10"></param>
        /// <param name="m11"></param>
        internal static void TtfTransformWith2x2Matrix(Glyph glyph, float m00, float m01, float m10, float m11)
        {
            //http://stackoverflow.com/questions/13188156/whats-the-different-between-vector2-transform-and-vector2-transformnormal-i
            //http://www.technologicalutopia.com/sourcecode/xnageometry/vector2.cs.htm

            //change data on current glyph
            float new_xmin = 0;
            float new_ymin = 0;
            float new_xmax = 0;
            float new_ymax = 0;

            GlyphPointF[] glyphPoints = glyph.GlyphPoints;
            for (int i = 0; i < glyphPoints.Length; ++i)
            {
                GlyphPointF p = glyphPoints[i];
                float x = p.P.X;
                float y = p.P.Y;

                float newX, newY;
                //please note that this is transform normal***
                glyphPoints[i] = new GlyphPointF(
                   newX = (float)Math.Round((x * m00) + (y * m10)),
                   newY = (float)Math.Round((x * m01) + (y * m11)),
                   p.onCurve);

                //short newX = xs[i] = (short)Math.Round((x * m00) + (y * m10));
                //short newY = ys[i] = (short)Math.Round((x * m01) + (y * m11));
                //------
                if (newX < new_xmin)
                {
                    new_xmin = newX;
                }
                if (newX > new_xmax)
                {
                    new_xmax = newX;
                }
                //------
                if (newY < new_ymin)
                {
                    new_ymin = newY;
                }
                if (newY > new_ymax)
                {
                    new_ymax = newY;
                }
            }
            //TODO: review here
            glyph.Bounds = new Bounds(
               (short)new_xmin, (short)new_ymin,
               (short)new_xmax, (short)new_ymax);
        }

        /// <summary>
        /// TrueType outline glyph clone
        /// </summary>
        /// <param name="original"></param>
        /// <param name="newGlyphIndex"></param>
        /// <returns></returns>
        internal static Glyph TtfOutlineGlyphClone(Glyph original, ushort newGlyphIndex)
        {
            //for true type instruction glyph***
            return new Glyph(
                Utils.CloneArray(original.GlyphPoints),
                Utils.CloneArray(original.EndPoints),
                original.Bounds,
                original.GlyphInstructions != null ? Utils.CloneArray(original.GlyphInstructions) : null,
                newGlyphIndex);
        }

        /// <summary>
        /// append data from src to dest, dest data will changed***
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        internal static void TtfAppendGlyph(Glyph dest, Glyph src)
        {
            int org_dest_len = dest.EndPoints.Length;
#if DEBUG
            int src_contour_count = src.EndPoints.Length;
#endif
            if (org_dest_len == 0)
            {
                //org is empty glyph

                dest.GlyphPoints = Utils.ConcatArray(dest.GlyphPoints, src.GlyphPoints);
                dest.EndPoints = Utils.ConcatArray(dest.EndPoints, src.EndPoints);
            }
            else
            {
                ushort org_last_point = (ushort)(dest.EndPoints[org_dest_len - 1] + 1); //since start at 0

                dest.GlyphPoints = Utils.ConcatArray(dest.GlyphPoints, src.GlyphPoints);
                dest.EndPoints = Utils.ConcatArray(dest.EndPoints, src.EndPoints);
                //offset latest append contour  end points
                int newlen = dest.EndPoints.Length;
                for (int i = org_dest_len; i < newlen; ++i)
                {
                    dest.EndPoints[i] += (ushort)org_last_point;
                }
            }

            //calculate new bounds
            Bounds destBound = dest.Bounds;
            Bounds srcBound = src.Bounds;
            short newXmin = (short)Math.Min(destBound.XMin, srcBound.XMin);
            short newYMin = (short)Math.Min(destBound.YMin, srcBound.YMin);
            short newXMax = (short)Math.Max(destBound.XMax, srcBound.XMax);
            short newYMax = (short)Math.Max(destBound.YMax, srcBound.YMax);

            dest.Bounds = new Bounds(newXmin, newYMin, newXMax, newYMax);
        }

#if DEBUG
        public readonly int dbugId;
        private static int s_debugTotalId;

        public override string ToString()
        {
            var stbuilder = new StringBuilder();
            if (IsCffGlyph)
            {
                stbuilder.Append("cff");
                stbuilder.Append(",index=" + GlyphIndex);
                stbuilder.Append(",name=" + _cff1GlyphData.Name);
            }
            else
            {
                stbuilder.Append("ttf");
                stbuilder.Append(",index=" + GlyphIndex);
                stbuilder.Append(",class=" + GlyphClass.ToString());
                if (MarkClassDef != 0)
                {
                    stbuilder.Append(",mark_class=" + MarkClassDef);
                }
            }
            return stbuilder.ToString();
        }

#endif

        //--------------------

        //cff

        internal readonly Cff1GlyphData _cff1GlyphData;             //NULL in  _onlyLayoutEssMode

        internal Glyph(Cff1GlyphData cff1Glyph, ushort glyphIndex)
        {
#if DEBUG
            dbugId = s_debugTotalId++;
            cff1Glyph.dbugGlyphIndex = glyphIndex;
#endif
            //create from CFF
            _cff1GlyphData = cff1Glyph;
            GlyphIndex = glyphIndex;
        }

        public bool IsCffGlyph => _cff1GlyphData != null;

        public Cff1GlyphData GetCff1GlyphData() => _cff1GlyphData;

        //TODO: review here again
        public GlyphInfo GlyphInfo { get; internal set; }  //FOUND in all mode (if font has this data)

        private uint _streamLen;            //FOUND in all mode (if font has this data)
        private ushort _imgFormat;          //FOUND in all mode (if font has this data)

        internal Glyph(ushort glyphIndex, uint streamOffset, uint streamLen, ushort imgFormat)
        {
            //_bmpGlyphSource = bmpGlyphSource;
            BitmapStreamOffset = streamOffset;
            _streamLen = streamLen;
            _imgFormat = imgFormat;
            GlyphIndex = glyphIndex;
        }

        internal uint BitmapStreamOffset { get; private set; }
        internal uint BitmapFormat => _imgFormat;

        private Glyph(ushort glyphIndex)
        {
            //for Clone_NO_BuildingInstructions()
            _onlyLayoutEssMode = true;
            GlyphIndex = glyphIndex;
        }

        internal static void CopyExistingGlyphInfo(Glyph src, Glyph dst)
        {
            dst.Bounds = src.Bounds;
            dst._hasOrgAdvWidth = src._hasOrgAdvWidth;
            dst.OriginalAdvanceWidth = src.OriginalAdvanceWidth;
            dst.BitmapGlyphAdvanceWidth = src.BitmapGlyphAdvanceWidth;
            dst.GlyphClass = src.GlyphClass;
            dst.MarkClassDef = src.MarkClassDef;

            //ttf: NO EndPoints, GlyphPoints, HasGlyphInstructions

            //cff:  NO _cff1GlyphData

            //math-font:
            dst.GlyphInfo = src.GlyphInfo;
            dst.BitmapStreamOffset = src.BitmapStreamOffset;
            dst._streamLen = src._streamLen;
            dst._imgFormat = src._imgFormat;
        }

        internal static Glyph Clone_NO_BuildingInstructions(Glyph src)
        {
            //a new glyph has only detail about glyph layout
            //NO information about glyph building instructions
            //1. if src if ttf
            //2. if src is cff
            //3. if src is svg
            //4. if src is bitmap

            Glyph newclone = new Glyph(src.GlyphIndex);
            CopyExistingGlyphInfo(src, newclone);
            return newclone;
        }
    }
}