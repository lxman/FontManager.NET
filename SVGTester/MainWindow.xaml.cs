using System.Windows;
using System.Windows.Media;
using FontParser;
using FontParser.Tables.Svg;

namespace SVGTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadSvgClick(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".ttf",
                Filter = "TTF Files (*.ttf)|*.ttf|OTF Files (*.otf)|*.otf"
            };

            bool? result = dlg.ShowDialog();

            if (result == false) return;
            string filename = dlg.FileName;
            var fontReader = new FontReader();
            List<FontStructure> structures = fontReader.ReadFile(filename);
            RenderPanel.Children.Clear();
            structures.ForEach(structure =>
            {
                DocumentIndex? documentIndex = structure.GetSvgDocumentIndex();
                if (documentIndex is null) return;
                documentIndex.Entries.ForEach(entry =>
                {
                    var svgRenderControl = new SvgRenderControl(entry.Instructions)
                    {
                        Height = 100,
                        Width = 100,
                        BorderThickness = new Thickness(2),
                        BorderBrush = Brushes.Black
                    };
                    RenderPanel.Children.Add(svgRenderControl);
                });
            });
        }
    }
}