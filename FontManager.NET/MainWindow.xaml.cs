using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FontManager.NET.Controls;
using FontManager.NET.Models;
using Xceed.Wpf.Toolkit;

namespace FontManager.NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        #endregion

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

        private void SpinnerChange(object? sender, SpinEventArgs e)
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
                value = Math.Round(value, 1);
            }
            else
            {
                value -= 0.1;
                value = Math.Round(value, 1);
            }

            pointSize.Text = value.ToString(new CultureInfo("en-us"));
        }

        private void BrowseButtonClick(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaOpenFileDialog dialog = new()
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
                // Code to handle the selected folder
            }
        }
    }
}