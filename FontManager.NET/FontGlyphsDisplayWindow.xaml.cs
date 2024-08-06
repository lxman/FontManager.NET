using System.Windows;
using FontManager.NET.Controls;
using FontParser;

namespace FontManager.NET
{
    /// <summary>
    /// Interaction logic for FontGlyphsDisplayWindow.xaml
    /// </summary>
    public partial class FontGlyphsDisplayWindow : Window
    {
        public FontGlyphsDisplayWindow(GlyphPointF[] outline)
        {
            InitializeComponent();
            FontDisplayControl fontDisplayControl = new(outline);
            MainGrid.Children.Add(fontDisplayControl);
        }
    }
}