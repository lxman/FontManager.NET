using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using FontManager.NET.Extensions;
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
        private GlyphData? _glyphData;
        private static int count;

        public DisplayGlyphControl()
        {
            InitializeComponent();
        }

        public void AssignGlyph(GlyphData glyphData)
        {
            _glyphData = glyphData;
            CreatePaths();
        }

        public void TranslateX(int dx)
        {
            _scaleMatrix.TransX = dx;
            _paths.ForEach(p => p.Transform(_scaleMatrix));
            DisplayGlyph.InvalidateVisual();
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
            _scaleMatrix.ScaleX = scaleFactor * 0.75f;
            _scaleMatrix.ScaleY = scaleFactor * 0.75f;
            _scaleMatrix.TransX = -_glyphData.Header.XMin + (bounds.Width / 50f);
            _scaleMatrix.TransY = -_glyphData.Header.YMin + (bounds.Height / 50f);

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
                        path.MoveTo(points[pointIndex++].ToSkPoint());
                        while (pointIndex <= ep)
                        {
                            path.LineTo(points[pointIndex++].ToSkPoint());
                        }
                        path.Close();
                        path.Transform(_scaleMatrix);
                        _paths.Add(path);
                    });
                    //List<SKRect> skBounds = _paths.Select(p => p.Bounds).ToList();
                    //if (skBounds.Any(b => b.Height > 0))
                    //{
                    //    SKPoint center = GetCenter();
                    //    Debug.Write($"Glyph {count++}, center = {center} After translation center = ");
                    //    var translateMatrix = SKMatrix.CreateTranslation(75 - center.X, 100 - center.Y);
                    //    _paths.ForEach(p => p.Transform(translateMatrix));
                    //    center = GetCenter();
                    //    Debug.WriteLine(center);
                    //}
                    DisplayGlyph.InvalidateVisual();
                    break;
            }
        }

        private SKPoint GetCenter()
        {
            var skPoints = new List<SKPoint>();
            _paths.Select(p => p.Points).ToList().ForEach(p => skPoints.AddRange(p));
            float minX = skPoints.Min(p => p.X);
            float minY = skPoints.Min(p => p.Y);
            float maxX = skPoints.Max(p => p.X);
            float maxY = skPoints.Max(p => p.Y);
            float centerX = maxX - minX;
            float centerY = maxY - minY;
            return new SKPoint(centerX, centerY);
        }
    }
}
