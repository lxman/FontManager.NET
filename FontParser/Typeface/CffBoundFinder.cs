using System;

namespace FontParser.Typeface
{
    public class CffBoundFinder : IGlyphTranslator
    {
        private float _minX, _maxX, _minY, _maxY;
        private float _curX, _curY;
        private float _latestMove_X, _latestMove_Y;

        /// <summary>
        /// curve flatten steps  => this a copy from Typography.Contours's GlyphPartFlattener
        /// </summary>
        private const int Nsteps = 3;

        private bool _contourOpen;
        private bool _first_eval = true;

        public void Reset()
        {
            _curX = _curY = _latestMove_X = _latestMove_Y = 0;
            _minX = _minY = float.MaxValue;//**
            _maxX = _maxY = float.MinValue;//**
            _first_eval = true;
            _contourOpen = false;
        }

        public void BeginRead(int contourCount)
        {
        }

        public void EndRead()
        {
        }

        public void CloseContour()
        {
            _contourOpen = false;
            _curX = _latestMove_X;
            _curY = _latestMove_Y;
        }

        public void Curve3(float x1, float y1, float x2, float y2)
        {
            //this a copy from Typography.Contours -> GlyphPartFlattener

            float eachstep = (float)1 / Nsteps;
            float t = eachstep;//start

            for (var n = 1; n < Nsteps; ++n)
            {
                float c = 1.0f - t;

                UpdateMinMax(
                    (c * c * _curX) + (2 * t * c * x1) + (t * t * x2),  //x
                    (c * c * _curY) + (2 * t * c * y1) + (t * t * y2)); //y

                t += eachstep;
            }

            //
            UpdateMinMax(
                _curX = x2,
                _curY = y2);

            _contourOpen = true;
        }

        public void Curve4(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            //this a copy from Typography.Contours -> GlyphPartFlattener

            float eachstep = (float)1 / Nsteps;
            float t = eachstep;//start

            for (var n = 1; n < Nsteps; ++n)
            {
                float c = 1.0f - t;

                UpdateMinMax(
                    (_curX * c * c * c) + (x1 * 3 * t * c * c) + (x2 * 3 * t * t * c) + x3 * t * t * t,  //x
                    (_curY * c * c * c) + (y1 * 3 * t * c * c) + (y2 * 3 * t * t * c) + y3 * t * t * t); //y

                t += eachstep;
            }
            //
            UpdateMinMax(
                _curX = x3,
                _curY = y3);

            _contourOpen = true;
        }

        public void LineTo(float x1, float y1)
        {
            UpdateMinMax(
                _curX = x1,
                _curY = y1);

            _contourOpen = true;
        }

        public void MoveTo(float x0, float y0)
        {
            if (_contourOpen)
            {
                CloseContour();
            }

            UpdateMinMax(
                _curX = x0,
                _curY = y0);
        }

        private void UpdateMinMax(float x0, float y0)
        {
            if (_first_eval)
            {
                //4 times

                if (x0 < _minX)
                {
                    _minX = x0;
                }
                //
                if (x0 > _maxX)
                {
                    _maxX = x0;
                }
                //
                if (y0 < _minY)
                {
                    _minY = y0;
                }
                //
                if (y0 > _maxY)
                {
                    _maxY = y0;
                }

                _first_eval = false;
            }
            else
            {
                //2 times

                if (x0 < _minX)
                {
                    _minX = x0;
                }
                else if (x0 > _maxX)
                {
                    _maxX = x0;
                }

                if (y0 < _minY)
                {
                    _minY = y0;
                }
                else if (y0 > _maxY)
                {
                    _maxY = y0;
                }
            }
        }

        public Bounds GetResultBounds()
        {
            return new Bounds(
                (short)Math.Floor(_minX),
                (short)Math.Floor(_minY),
                (short)Math.Ceiling(_maxX),
                (short)Math.Ceiling(_maxY));
        }
    }
}