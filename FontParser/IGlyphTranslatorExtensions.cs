using System.Numerics;
using FontParser.Exceptions;
using FontParser.Tables.CFF;
using FontParser.Tables.CFF.CFF;

namespace FontParser
{
    internal static class IGlyphTranslatorExtensions
    {
        //for TrueType Font
        public static void Read(this IGlyphTranslator tx, GlyphPointF[] glyphPoints, ushort[] contourEndPoints, float scale = 1)
        {
            if (glyphPoints == null || contourEndPoints == null)
            {
                return;//?
            }

            var startContour = 0;
            var cpoint_index = 0;//current point index

            int todoContourCount = contourEndPoints.Length;
            //-----------------------------------
            //1. start read data from a glyph
            tx.BeginRead(todoContourCount);
            //-----------------------------------
            float latest_moveto_x = 0;
            float latest_moveto_y = 0;

            while (todoContourCount > 0)
            {
                //reset
                var curveControlPointCount = 0; // 1 curve control point => Quadratic, 2 curve control points => Cubic

                //foreach contour...
                //next contour will begin at...
                int nextCntBeginAtIndex = contourEndPoints[startContour] + 1;

                //reset  ...

                var has_c_begin = false;  //see below [A]
                var c_begin = new Vector2(); //special point if the glyph starts with 'off-curve' control point
                var c1 = new Vector2(); //control point of quadratic curve
                //-------------------------------------------------------------------
                var offCurveMode = false;
                var foundFirstOnCurvePoint = false;
                var startWithOffCurve = false;
                var cnt_point_count = 0;
                //-------------------------------------------------------------------
                //[A]
                //first point may start with 'ON CURVE" or 'OFF-CURVE'
                //1. if first point is 'ON-CURVE' => we just set moveto command to it
                //
                //2. if first point is 'OFF-CURVE' => we store it into c_begin and set has_c_begin= true
                //   the c_begin will be use when we close the contour
                //
                //
                //eg. glyph '2' in Century font starts with 'OFF-CURVE' point, and ends with 'OFF-CURVE'
                //-------------------------------------------------------------------

#if DEBUG
                var dbug_cmdcount = 0;
#endif
                for (; cpoint_index < nextCntBeginAtIndex; ++cpoint_index)
                {
#if DEBUG
                    dbug_cmdcount++;

#endif
                    //for each point in this contour

                    //point p is an on-curve point (on outline). (not curve control point)
                    //possible ways..
                    //1. if we are in curve mode, then p is end point
                    //   we must decide which curve to create (Curve3 or Curve4)
                    //   easy, ...
                    //      if  curveControlPointCount == 1 , then create Curve3
                    //      else curveControlPointCount ==2 , then create Curve4
                    //2. if we are NOT in curve mode,
                    //      if p is first point then set this to x0,y0
                    //      else then p is end point of a line.

                    GlyphPointF p = glyphPoints[cpoint_index];
                    cnt_point_count++;

                    float p_x = p.X * scale;
                    float p_y = p.Y * scale;

                    //int vtag = (int)flags[cpoint_index] & 0x1;
                    //bool has_dropout = (((vtag >> 2) & 0x1) != 0);
                    //int dropoutMode = vtag >> 3;

                    if (p.onCurve)
                    {
                        //-------------------------------------------------------------------
                        //[B]
                        //point p is an 'on-curve' point (on outline).
                        //(not curve control point)***
                        //the point touch the outline.

                        //possible ways..
                        //1. if we are in offCurveMode, then p is a curve end point.
                        //   we must decide which curve to create (Curve3 or Curve4)
                        //   easy, ...
                        //      if  curveControlPointCount == 1 , then create Curve3
                        //      else curveControlPointCount ==2 , then create Curve4 (BUT SHOULD NOT BE FOUND IN TRUE TYPEFONT'(
                        //2. if we are NOT in offCurveMode,
                        //      if p is first point then set this to =>moveto(x0,y0)
                        //      else then p is end point of a line => lineto(x1,y1)
                        //-------------------------------------------------------------------

                        if (offCurveMode)
                        {
                            //as describe above [B.1] ,...

                            switch (curveControlPointCount)
                            {
                                case 1:

                                    tx.Curve3(
                                        c1.X, c1.Y,
                                        p_x, p_y);

                                    break;

                                default:

                                    //for TrueType font
                                    //we should not be here?
                                    throw new OpenFontNotSupportedException();
                            }

                            //reset curve control point count
                            curveControlPointCount = 0;
                            //we touch the curve, set offCurveMode= false
                            offCurveMode = false;
                        }
                        else
                        {
                            // p is ON CURVE, but now we are in OFF-CURVE mode.
                            //
                            //as describe above [B.2] ,...
                            if (!foundFirstOnCurvePoint)
                            {
                                //special treatment for first point
                                foundFirstOnCurvePoint = true;
                                switch (curveControlPointCount)
                                {
                                    case 0:
                                        //describe above, see [A.1]
                                        tx.MoveTo(latest_moveto_x = p_x, latest_moveto_y = p_y);
                                        break;

                                    case 1:

                                        //describe above, see [A.2]
                                        c_begin = c1;
                                        has_c_begin = true;
                                        //since c1 is off curve
                                        //we skip the c1 for and use it when we close the curve

                                        tx.MoveTo(latest_moveto_x = p_x, latest_moveto_y = p_y);
                                        curveControlPointCount--;
                                        break;

                                    default: throw new OpenFontNotSupportedException();
                                }
                            }
                            else
                            {
                                tx.LineTo(p_x, p_y);
                            }

                            //if (has_dropout)
                            //{
                            //    //printf("[%d] on,dropoutMode=%d: %d,y:%d \n", mm, dropoutMode, vpoint.x, vpoint.y);
                            //}
                            //else
                            //{
                            //    //printf("[%d] on,x: %d,y:%d \n", mm, vpoint.x, vpoint.y);
                            //}
                        }
                    }
                    else
                    {
                        //p is OFF-CURVE point (this is curve control point)
                        //
                        if (cnt_point_count == 1)
                        {
                            //1st point
                            startWithOffCurve = true;
                        }
                        switch (curveControlPointCount)
                        {
                            case 0:
                                c1 = new Vector2(p_x, p_y);
                                if (foundFirstOnCurvePoint)
                                {
                                    //this point is curve control point***
                                    //so set curve mode = true
                                    //check number if existing curve control
                                    offCurveMode = true;
                                }

                                //describe above, see [A.2]
                                break;

                            case 1:
                                {
                                    if (!foundFirstOnCurvePoint)
                                    {
                                        Vector2 mid2 = GetMidPoint(c1, p_x, p_y);
                                        //----------
                                        //2. generate curve3 ***
                                        c_begin = c1;
                                        has_c_begin = true;

                                        tx.MoveTo(latest_moveto_x = mid2.X, latest_moveto_y = mid2.Y);

                                        offCurveMode = true;
                                        foundFirstOnCurvePoint = true;

                                        c1 = new Vector2(p_x, p_y);
                                        continue;
                                    }

                                    //we already have previous 1st control point (c1)
                                    //-------------------------------------
                                    //please note that TrueType font
                                    //compose of Quadratic Bezier Curve (Curve3) ***
                                    //-------------------------------------
                                    //in this case, this is NOT Cubic,
                                    //this is 2 CONNECTED Quadratic Bézier curves***
                                    //
                                    //we must create 'end point' of the first curve
                                    //and set it as 'begin point of the second curve.
                                    //
                                    //this is done by ...
                                    //1. calculate mid-point between c1 and the latest point (p_x,p_y)
                                    Vector2 mid = GetMidPoint(c1, p_x, p_y);
                                    //----------
                                    //2. generate curve3 ***
                                    tx.Curve3(
                                        c1.X, c1.Y,
                                        mid.X, mid.Y);
                                    //------------------------
                                    //3. so curve control point number is reduce by 1***
                                    curveControlPointCount--;
                                    //------------------------
                                    //4. and set (p_x,p_y) as 1st control point for the new curve
                                    c1 = new Vector2(p_x, p_y);
                                    offCurveMode = true;
                                    //
                                    //printf("[%d] bzc2nd,  x: %d,y:%d \n", mm, vpoint.x, vpoint.y);
                                }
                                break;

                            default:
                                throw new OpenFontNotSupportedException();
                        }
                        //count
                        curveControlPointCount++;
                    }
                }
                //--------
                //when finish,
                //ensure that the contour is closed.

                if (offCurveMode)
                {
                    switch (curveControlPointCount)
                    {
                        case 0: break;
                        case 1:
                            {
                                if (has_c_begin)
                                {
                                    Vector2 mid = GetMidPoint(c1, c_begin.X, c_begin.Y);
                                    //----------
                                    //2. generate curve3 ***
                                    tx.Curve3(
                                        c1.X, c1.Y,
                                        mid.X, mid.Y);
                                    //------------------------
                                    //3. so curve control point number is reduce by 1***
                                    curveControlPointCount--;
                                    //------------------------
                                    tx.Curve3(
                                         c_begin.X, c_begin.Y,
                                         latest_moveto_x, latest_moveto_y);
                                }
                                else
                                {
                                    tx.Curve3(
                                        c1.X, c1.Y,
                                        latest_moveto_x, latest_moveto_y);
                                }
                            }
                            break;

                        default:
                            //for TrueType font
                            //we should not be here?
                            throw new OpenFontNotSupportedException();
                    }
                }
                else
                {
                    //end with touch curve
                    //but if this start with off curve
                    //then we must close it properly
                    if (startWithOffCurve)
                    {
                        //start with off-curve and end with touch curve
                        tx.Curve3(
                           c_begin.X, c_begin.Y,
                           latest_moveto_x, latest_moveto_y);
                    }
                }

                //--------
                tx.CloseContour(); //***
                startContour++;
                //--------
                todoContourCount--;
                //--------
            }
            //finish
            tx.EndRead();
        }

        private static Vector2 GetMidPoint(Vector2 v0, float x1, float y1)
        {
            //mid point between v0 and (x1,y1)
            return new Vector2(
                ((v0.X + x1) / 2f),
                ((v0.Y + y1) / 2f));
        }

        //-----------
        //for CFF1
        public static void Read(this IGlyphTranslator tx, Cff1Font cff1Font, Cff1GlyphData glyphData, float scale = 1)
        {
            var evalEngine = new CffEvaluationEngine();
            evalEngine.Run(tx, glyphData.GlyphInstructions, scale);
        }
    }
}