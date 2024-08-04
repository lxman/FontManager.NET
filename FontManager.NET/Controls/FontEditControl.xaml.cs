using System.Windows.Controls;
using System.Windows.Input;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace FontManager.NET.Controls
{
    /// <summary>
    /// Interaction logic for FontEditControl.xaml
    /// </summary>
    public partial class FontEditControl : UserControl
    {
        private readonly SKPath _path = new();
        private readonly SKPoint[] _points = new SKPoint[3];
        private readonly SKPoint[] _controlPoints = new SKPoint[2];
        private int _stateMachine = 0;
        private static readonly SKPaint CurvePaint = new()
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Red,
            StrokeWidth = 2
        };

        public FontEditControl()
        {
            InitializeComponent();
        }

        private void FontEditViewPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface? surface = e.Surface;
            SKCanvas? canvas = surface.Canvas;

            canvas.Clear(SKColors.White);

            // Draw a bezier curve with the SKPath _path
            canvas.DrawPath(_path, CurvePaint);
            if (_path.PointCount > 2)
            {
                canvas.DrawPoint(_controlPoints[0], CurvePaint);
            }
            if (_path.PointCount > 3)
            {
                canvas.DrawPoint(_controlPoints[1], CurvePaint);
            }
        }

        private void FontEditViewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                switch (_stateMachine)
                {
                    case 0:
                        _points[0] = new SKPoint(Convert.ToSingle(e.GetPosition(FontEditView).X), Convert.ToSingle(e.GetPosition(FontEditView).Y));
                        _stateMachine++;
                        break;
                    case 1:
                        _points[1] = new SKPoint(Convert.ToSingle(e.GetPosition(FontEditView).X), Convert.ToSingle(e.GetPosition(FontEditView).Y));
                        _controlPoints[0] = _points[1];
                        _stateMachine++;
                        break;
                    case 2:
                        _points[2] = new SKPoint(Convert.ToSingle(e.GetPosition(FontEditView).X), Convert.ToSingle(e.GetPosition(FontEditView).Y));
                        _controlPoints[1] = _points[2];
                        _stateMachine++;
                        break;
                }
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                _path.Reset();
                _stateMachine = 0;
                FontEditView.InvalidateVisual();
            }
        }

        private void FontEditViewMouseMove(object sender, MouseEventArgs e)
        {
            switch (_stateMachine)
            {
                case 1:
                    _points[1] = new SKPoint(Convert.ToSingle(e.GetPosition(FontEditView).X), Convert.ToSingle(e.GetPosition(FontEditView).Y));
                    _path.Reset();
                    _path.MoveTo(_points[0]);
                    _path.LineTo(_points[1]);
                    FontEditView.InvalidateVisual();
                    break;
                case 2:
                    _points[2] = new SKPoint(Convert.ToSingle(e.GetPosition(FontEditView).X), Convert.ToSingle(e.GetPosition(FontEditView).Y));
                    _path.Reset();
                    _path.MoveTo(_points[0]);
                    _path.QuadTo(_points[1], _points[2]);
                    FontEditView.InvalidateVisual();
                    break;
                case 3:
                    _points[2] = new SKPoint(Convert.ToSingle(e.GetPosition(FontEditView).X), Convert.ToSingle(e.GetPosition(FontEditView).Y));
                    _path.Reset();
                    _path.MoveTo(_points[0]);
                    _path.CubicTo(_points[1], _points[2], _points[2]);
                    FontEditView.InvalidateVisual();
                    break;
            }
        }
    }
}