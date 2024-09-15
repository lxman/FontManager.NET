using System.Windows.Controls;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using Svg.Skia;

namespace SVGTester
{
    /// <summary>
    /// Interaction logic for SVGRenderControl.xaml
    /// </summary>
    public partial class SvgRenderControl : UserControl
    {
        private readonly SKPicture _skPicture;
        private readonly SKRect _viewBounds;
        private readonly SKBitmap _bitmap;

        public SvgRenderControl(string svgDocument)
        {
            InitializeComponent();
            var svg = new SKSvg();
            SKImage image = SKImage.FromEncodedData(svgDocument);
            SKPicture? skPicture = svg.FromSvg(svgDocument);
            _viewBounds = svg.Picture?.CullRect ?? throw new ArgumentException("No cull rectangle found.");
            _skPicture = skPicture ?? throw new InvalidOperationException("Failed to load SVG document.");
            _bitmap = skPicture.ToBitmap(new SKColor(255, 255, 255), 1, 1, SKColorType.RgbaF32, SKAlphaType.Opaque, SKColorSpace.CreateSrgb());
        }

        private void ViewPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;

            //canvas.Translate(Convert.ToSingle(Width) / 2, Convert.ToSingle(Height) / 2);
            //canvas.Scale(0.9f *
            //             Math.Min(Convert.ToSingle(Width) / _viewBounds.Width,
            //                 Convert.ToSingle(Height) / _viewBounds.Height));
            //canvas.Translate(-_viewBounds.MidX, -_viewBounds.MidY);
            //canvas.DrawPicture(_skPicture);
            if (_bitmap is not null) canvas.DrawBitmap(_bitmap, new SKPoint(0, 10));
        }
    }
}
