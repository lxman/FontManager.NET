using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using FontParser.Tables.TtTables.Glyf;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace FontManager.NET.Controls
{
    /// <summary>
    /// Interaction logic for DisplayGlyphControl.xaml
    /// </summary>
    public partial class DisplayGlyphControl : UserControl
    {
        private readonly List<SKPath> _paths = [];
        private static readonly SKPaint GlyphPaint = new()
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 2
        };
        private SKMatrix _scaleMatrix = SKMatrix.CreateScaleTranslation(1, -1, 0, -1);
        private SKMatrix _translateMatrix = SKMatrix.CreateTranslation(0, 0);
        private GlyphData? _glyphData;

        public DisplayGlyphControl()
        {
            InitializeComponent();
        }

        public void AssignGlyph(GlyphData glyphData)
        {
            _glyphData = glyphData;
            CreatePaths();
        }

        private void DisplayGlyphPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface? surface = e.Surface;
            SKCanvas? canvas = surface.Canvas;

            canvas.Clear(SKColors.White);
            _paths.ForEach(p => canvas.DrawPath(p, GlyphPaint));
        }

        private void DisplayGlyphOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _paths.Clear();
            CreatePaths();
        }

        private void CreatePaths()
        {
            if (_glyphData is null) return;
            int yOffset = _glyphData.Header.YMax - _glyphData.Header.YMin;
            Rectangle bounds = new(0, -yOffset, _glyphData.Header.XMax - _glyphData.Header.XMin, _glyphData.Header.YMax - _glyphData.Header.YMin);

            SKSize size = DisplayGlyph.CanvasSize;
            int glyphHeight = _glyphData.Header.YMax - _glyphData.Header.YMin;
            float scaleFactor = size.Height / glyphHeight;
            _scaleMatrix.ScaleX = scaleFactor;
            _scaleMatrix.ScaleY = scaleFactor;
            _scaleMatrix.TransX = 0;
            _scaleMatrix.TransY = 0;
            _translateMatrix.TransX = -_glyphData.Header.XMin;
            _translateMatrix.TransY = -_glyphData.Header.YMin;

            switch (_glyphData.GlyphSpec)
            {
                case CompositeGlyph compositeGlyph:
                    break;
                case SimpleGlyph simpleGlyph:
                    List<SimpleGlyphCoordinate> coordinates = simpleGlyph.Coordinates;
                    List<PointF> points = coordinates.Select(c => c.Point with { Y = bounds.Height - c.Point.Y }).ToList();
                    List<ushort> endPoints = simpleGlyph.EndPtsOfContours;
                    var pointIndex = 0;
                    endPoints.ForEach(ep =>
                    {
                        var path = new SKPath();
                        path.MoveTo(points[pointIndex++].ToSKPoint());
                        while (pointIndex <= ep)
                        {
                            path.LineTo(points[pointIndex++].ToSKPoint());
                        }
                        path.Close();
                        path.Transform(_translateMatrix);
                        path.Transform(_scaleMatrix);
                        _paths.Add(path);
                    });
                    DisplayGlyph.InvalidateVisual();
                    break;
            }
        }
    }
}
