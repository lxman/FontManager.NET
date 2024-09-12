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

        public DisplayGlyphControl()
        {
            InitializeComponent();
        }

        public void AssignGlyph(GlyphData glyphData)
        {
            SKSize size = DisplayGlyph.CanvasSize;
            int glyphWidth = glyphData.Header.XMax - glyphData.Header.XMin;
            int glyphHeight = glyphData.Header.YMax - glyphData.Header.YMin;
            SKPoint origin = new(glyphData.Header.XMin, glyphData.Header.YMin);
            float scaleFactor = size.Height / glyphHeight;
            _scaleMatrix.ScaleX = scaleFactor;
            _scaleMatrix.ScaleY = scaleFactor;
            _scaleMatrix.TransX = -origin.X;
            _scaleMatrix.TransY = Convert.ToSingle(-origin.Y * .35);

            switch (glyphData.GlyphSpec)
            {
                case CompositeGlyph compositeGlyph:
                    break;
                case SimpleGlyph simpleGlyph:
                    List<SimpleGlyphCoordinate> coordinates = simpleGlyph.Coordinates;
                    ushort[] endPoints = simpleGlyph.EndPtsOfContours;
                    _path.Reset();
                    _path.MoveTo(coordinates[0].Point.ToSKPoint());
                    for (var i = 1; i < coordinates.Count; i++)
                    {
                        _path.LineTo(coordinates[i].Point.ToSKPoint());
                    }
                    _path.Transform(_scaleMatrix);
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
