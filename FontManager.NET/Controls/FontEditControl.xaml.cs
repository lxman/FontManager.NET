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

        public FontEditControl()
        {
            InitializeComponent();
        }

        private void FontEditViewPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface? surface = e.Surface;
            SKCanvas? canvas = surface.Canvas;

            canvas.Clear(SKColors.White);

            var textPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black,
                TextAlign = SKTextAlign.Center,
                TextSize = 24
            };

            var curvePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Red,
                StrokeWidth = 2
            };

            // Draw a bezier curve with the SKPath _path
            canvas.DrawPath(_path, curvePaint);

            canvas.DrawText("Hello, SkiaSharp!", Convert.ToSingle(e.Info.Width) / 2, Convert.ToSingle(e.Info.Height) / 2, textPaint);
        }

        private void FontEditViewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _points[0] = new SKPoint(Convert.ToSingle(e.GetPosition(FontEditView).X), Convert.ToSingle(e.GetPosition(FontEditView).Y));
        }
    }
}