namespace FontParser.Tables.CFF
{
    public class PxScaleGlyphTx : IGlyphTranslator
    {
        private readonly float _scale;
        private readonly IGlyphTranslator _tx;

        private bool _is_contour_opened;

        public PxScaleGlyphTx(float scale, IGlyphTranslator tx)
        {
            _scale = scale;
            _tx = tx;
        }

        public void BeginRead(int contourCount)
        {
            _tx.BeginRead(contourCount);
        }

        public void CloseContour()
        {
            _is_contour_opened = false;
            _tx.CloseContour();
        }

        public void Curve3(float x1, float y1, float x2, float y2)
        {
            _is_contour_opened = true;
            _tx.Curve3(x1 * _scale, y1 * _scale, x2 * _scale, y2 * _scale);
        }

        public void Curve4(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            _is_contour_opened = true;
            _tx.Curve4(x1 * _scale, y1 * _scale, x2 * _scale, y2 * _scale, x3 * _scale, y3 * _scale);
        }

        public void EndRead()
        {
            _tx.EndRead();
        }

        public void LineTo(float x1, float y1)
        {
            _is_contour_opened = true;
            _tx.LineTo(x1 * _scale, y1 * _scale);
        }

        public void MoveTo(float x0, float y0)
        {
            _tx.MoveTo(x0 * _scale, y0 * _scale);
        }

        //

        public bool IsContourOpened => _is_contour_opened;
    }
}
