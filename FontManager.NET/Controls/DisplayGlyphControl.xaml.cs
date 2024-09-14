using System.Diagnostics;
using System.Drawing;
using System.Windows.Controls;
using NewFontParser.Tables.TtTables.Glyf;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace FontManager.NET.Controls
{
    /// <summary>
    /// Interaction logic for DisplayGlyphControl.xaml
    /// </summary>
    public partial class DisplayGlyphControl : UserControl
    {
        private readonly SKPath _path = new();
        private static readonly SKPaint GlyphPaint = new()
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 2
        };
        private SKMatrix _scaleMatrix = SKMatrix.CreateScaleTranslation(1, -1, 0, -1);
        private SKMatrix _translateMatrix = SKMatrix.CreateTranslation(0, 0);

        public DisplayGlyphControl()
        {
            InitializeComponent();
        }

        public void AssignGlyph(GlyphData glyphData)
        {
            int yOffset = glyphData.Header.YMax - glyphData.Header.YMin;
            Rectangle bounds = new(0, -yOffset, glyphData.Header.XMax - glyphData.Header.XMin, glyphData.Header.YMax - glyphData.Header.YMin);

            SKSize size = DisplayGlyph.CanvasSize;
            Debug.WriteLine($"Window size: {size.Width}x{size.Height}");
            int glyphWidth = glyphData.Header.XMax - glyphData.Header.XMin;
            int glyphHeight = glyphData.Header.YMax - glyphData.Header.YMin;
            SKPoint origin = new(glyphData.Header.XMin, glyphData.Header.YMin);
            float scaleFactor = size.Height / glyphHeight;
            Debug.WriteLine($"Glyph size: {glyphWidth}x{glyphHeight}");
            Debug.WriteLine($"Glyph origin: {origin.X}, {origin.Y}");
            _scaleMatrix.ScaleX = glyphHeight / size.Height;
            _scaleMatrix.ScaleY = glyphWidth / size.Width;
            _scaleMatrix.TransX = 0;
            _scaleMatrix.TransY = 0;
            _translateMatrix.TransX = -glyphData.Header.XMin;
            _translateMatrix.TransY = bounds.Height;

            switch (glyphData.GlyphSpec)
            {
                case CompositeGlyph compositeGlyph:
                    break;
                case SimpleGlyph simpleGlyph:
                    List<SimpleGlyphCoordinate> coordinates = simpleGlyph.Coordinates;
                    List<Point> points = coordinates.Select(c => c.Point with { Y = bounds.Height - c.Point.Y }).ToList();
                    ushort[] endPoints = simpleGlyph.EndPtsOfContours;
                    _path.Reset();
                    _path.MoveTo(points[0].ToSKPoint());
                    for (var i = 1; i < points.Count; i++)
                    {
                        _path.LineTo(points[i].ToSKPoint());
                    }
                    _path.Close();
                    _path.Transform(_translateMatrix);
                    //_path.Transform(_scaleMatrix);
                    DisplayGlyph.InvalidateVisual();
                    break;
            }
        }

        private void DisplayGlyphPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface? surface = e.Surface;
            SKCanvas? canvas = surface.Canvas;

            canvas.Clear(SKColors.White);
            canvas.DrawPath(_path, GlyphPaint);
        }
    }
}
