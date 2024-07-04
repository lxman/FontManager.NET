using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FontManager.NET.Models;

namespace FontManager.NET.Controls
{
    /// <summary>
    /// Interaction logic for FontFamilyControl.xaml
    /// </summary>
    public partial class FontFamilyControl : UserControl
    {
        #region ButtonContent DP

        public string ButtonContent
        {
            get => (string)GetValue(ButtonContentProperty);
            set => SetValue(ButtonContentProperty, value);
        }

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty
                .Register(
                    nameof(ButtonContent),
                    typeof(string),
                    typeof(FontFamilyControl),
                    new PropertyMetadata("Button"));

        #endregion

        #region TextBlockContent DP

        public string TextBlockContent
        {
            get => (string)GetValue(TextBlockContentProperty);
            set => SetValue(TextBlockContentProperty, value);
        }

        public static readonly DependencyProperty TextBlockContentProperty =
            DependencyProperty
                .Register(
                    nameof(TextBlockContent),
                    typeof(string),
                    typeof(FontFamilyControl),
                    new PropertyMetadata("Hello world!"));

        #endregion

        #region TextBlockFont DP

        public DisplayFontDefinition TextBlockFont
        {
            get => (DisplayFontDefinition)GetValue(TextBlockFontProperty);
            set => SetValue(TextBlockFontProperty, value);
        }

        public static readonly DependencyProperty TextBlockFontProperty =
            DependencyProperty
                .Register(
                    nameof(TextBlockFont),
                    typeof(DisplayFontDefinition),
                    typeof(FontFamilyControl),
                    new PropertyMetadata(
                        new DisplayFontDefinition(
                            new Typeface(
                                new FontFamily("Arial"),
                                FontStyles.Normal,
                                FontWeights.Normal,
                                FontStretches.Normal
                                ),
                            12.0
                            )));

        #endregion

        public FontFamilyControl()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
