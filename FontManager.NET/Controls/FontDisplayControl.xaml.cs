using System.Windows.Controls;
using FontManager.NET.Extensions;
using FontParser;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace FontManager.NET.Controls
{
    /// <summary>
    /// Interaction logic for FontDisplayControl.xaml
    /// </summary>
    public partial class FontDisplayControl : UserControl
    {
        private readonly GlyphPointF[] _outline;

        public FontDisplayControl(GlyphPointF[] outline)
        {
            InitializeComponent();
            _outline = outline;
        }

        private void DrawFontWindowPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface? surface = e.Surface;
            SKCanvas? canvas = surface.Canvas;
            canvas.Clear(SKColors.White);
            canvas.Scale(1, -1);
            canvas.Translate(canvas.LocalClipBounds.Width / 2, -canvas.LocalClipBounds.Height / 2);
            SKPath path = new();
            path.MoveTo(_outline[0].ToSKPoint());
            for (var i = 1; i < 10; i++)
            {
                path.LineTo(_outline[i].ToSKPoint());
            }
            path.Close();
            canvas.DrawPath(path, new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1
            });
        }
    }
}