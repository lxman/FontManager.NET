using System.Drawing;
using System.Windows;
using FontManager.NET.Controls;

namespace FontManager.NET
{
    /// <summary>
    /// Interaction logic for FontGlyphsDisplayWindow.xaml
    /// </summary>
    public partial class FontGlyphsDisplayWindow : Window
    {
        public FontGlyphsDisplayWindow(PointF[] outline)
        {
            InitializeComponent();
            FontDisplayControl fontDisplayControl = new(outline);
            MainGrid.Children.Add(fontDisplayControl);
        }
    }
}