using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FontManager.NET.Controls;
using FontManager.NET.Models;
using FontParser;
using FontParser.Tables.TtTables.Glyf;
using Ookii.Dialogs.Wpf;
using Xceed.Wpf.Toolkit;

namespace FontManager.NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> SelectableFonts { get; set; } = [];

        public string ChosenFont { get; set; } = string.Empty;

        private const string FontDirectory = @"C:\Users\jorda\source\Woff2Fonts";

        #region Spinner TextBox DP

        public string TextBoxContent
        {
            get => (string)GetValue(TextBoxContentProperty);
            set
            {
                SetValue(TextBoxContentProperty, value);
                PointSizeChanged();
            }
        }

        public static readonly DependencyProperty TextBoxContentProperty =
            DependencyProperty
                .Register(
                    nameof(TextBoxContent),
                    typeof(string),
                    typeof(TextBox),
                    new PropertyMetadata("14"));

        #endregion Spinner TextBox DP

        public MainWindow()
        {
            InitializeComponent();
            TextBoxContent = "14";
            FontList.ItemsSource = Fonts.SystemFontFamilies.SelectMany(f => f.FamilyNames.Values!).Order();
        }

        private void ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (FontList.SelectedItem is not string fontName) return;
            DisplayArea.Children.Clear();
            List<FontFamily> families = Fonts.SystemFontFamilies.Where(f => f.FamilyNames.Values!.Contains(fontName)).ToList();
            families.ForEach(f =>
            {
                f.FamilyTypefaces.ToList().ForEach(t =>
                {
                    FontFamilyControl control = new()
                    {
                        ButtonContent = t.AdjustedFaceNames.Values.First(),
                        TextBlockContent = "The quick brown fox jumped over the lazy cow",
                        TextBlockFont = new DisplayFontDefinition(new Typeface(f, t.Style, t.Weight, t.Stretch), Convert.ToDouble(TextBoxContent))
                    };
                    DisplayArea.Children.Add(control);
                });
            });
        }

        private void PointSizeChanged()
        {
            var value = Convert.ToDouble(TextBoxContent);
            value = value <= 0 ? 0.1 : value;
            PointSize.ValidSpinDirection = value <= 0.1 ? ValidSpinDirections.Increase : ValidSpinDirections.Increase | ValidSpinDirections.Decrease;
            DisplayArea.Children.Cast<FontFamilyControl>().ToList().ForEach(c => c.TextBlockFont = new DisplayFontDefinition(c.TextBlockFont.Typeface, value));
        }

        private void SizeChange(object? sender, SpinEventArgs e)
        {
            e.Handled = true;
            var spinner = (ButtonSpinner)sender!;
            var pointSize = (TextBox)spinner.Content;
            var value = Convert.ToDouble(pointSize.Text);
            value = value <= 0 ? 0.1 : value;
            spinner.ValidSpinDirection = value <= 0.1 ? ValidSpinDirections.Increase : ValidSpinDirections.Increase | ValidSpinDirections.Decrease;
            if (e.Direction == SpinDirection.Increase)
            {
                value += 0.1;
            }
            else
            {
                value -= 0.1;
            }

            value = Math.Round(value, 1);

            pointSize.Text = value.ToString(new CultureInfo("en-us"));
        }

        private void BrowseButtonClick(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog dialog = new()
            {
                CheckFileExists = false,
                ValidateNames = false,
                CheckPathExists = true,
                FileName = "Open Folder"
            };
            if (dialog.ShowDialog() == true)
            {
                if (dialog.FileName.Split("\\").Last() == "Open Folder")
                {
                    PathDisplay.Text = dialog.FileName.Replace("Open Folder", "");
                }

                string path = PathDisplay.Text;
                List<string> fonts =
                    Directory
                        .EnumerateFiles(path)
                        .Where(f =>
                            f.EndsWith(".ttf")
                            || f.EndsWith(".otf")
                            || f.EndsWith(".ttc")
                            || f.EndsWith(".woff")
                            || f.EndsWith(".woff2")
                        ).ToList();
                fonts.ForEach(f =>
                {
                    FontFamily family = new(new Uri(f), f.Split("\\").Last()[..^4]);
                    family.FamilyTypefaces.ToList().ForEach(t =>
                    {
                        t.AdjustedFaceNames.Values.ToList().ForEach(n =>
                        {
                            ListBoxItem fontItem = new()
                            {
                                Content = $"{family.Source} {n}",
                                FontFamily = family,
                                FontWeight = t.Weight,
                                FontStyle = t.Style,
                                FontStretch = t.Stretch,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalContentAlignment = HorizontalAlignment.Left,
                                VerticalContentAlignment = VerticalAlignment.Center,
                                Tag = f
                            };
                            fontItem.MouseDoubleClick += ListBoxItemDoubleClick;
                            FamilyListControl.ObservableCollectionFamilyList.Add(fontItem);
                        });
                    });
                });
            }
        }

        private void ListBoxItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (ListBoxItem)sender;
            e.Handled = true;
            var f = item.Tag.ToString()!;
        }

        private static List<string> GetFonts(string directory) =>
            Directory
                .GetFiles(directory)
                .Where(f =>
                    f.ToLower().EndsWith(".ttf")
                    || f.ToLower().EndsWith(".otf")
                    || f.ToLower().EndsWith(".ttc")
                    || f.ToLower().EndsWith(".woff")
                    || f.ToLower().EndsWith(".woff2")
                ).ToList();

        private void DisplayGlyphTabGotFocus(object sender, RoutedEventArgs e)
        {
            List<string> fonts = GetFonts(FontDirectory);
            List<string> names = fonts.Select(f => f.Split("\\").Last()).ToList();
            if (names.Contains(ChosenFont))
            {
                return;
            }
            SelectableFonts.Clear();
            SelectableFonts.AddRange(names);
            ChosenFont = names.First();
        }

        private void ChooseFontSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            var chosenFont = $"{FontDirectory}\\{ChosenFont}";
            FontReader reader = new();
            List<FontStructure> fontStructures = reader.ReadFile(chosenFont);
            FontStructure structure = fontStructures[0];
            var glyfTable = structure.Tables.First(t => t is GlyphTable) as GlyphTable;
            GlyphDisplayGrid.Children.Clear();
            glyfTable?.Glyphs.ForEach(g =>
            {
                switch (g.GlyphSpec)
                {
                    case CompositeGlyph compositeGlyph:
                        break;
                    case SimpleGlyph simpleGlyph:
                        var dgControl = new DisplayGlyphControl
                        {
                            Height = 200,
                            Width = 150
                        };
                        dgControl.AssignGlyph(g);
                        GlyphDisplayGrid.Children.Add(dgControl);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
    }
}