using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace FontManager.NET.Controls
{
    /// <summary>
    /// Interaction logic for SpinnerControl.xaml
    /// </summary>
    public partial class DoubleSpinnerControl : UserControl
    {
        public string TextBoxContent
        {
            get => (string)GetValue(TextBoxContentProperty);
            set
            {
                SetValue(TextBoxContentProperty, value);
                ValueChanged();
            }
        }

        public static readonly DependencyProperty TextBoxContentProperty =
            DependencyProperty
                .Register(
                    nameof(TextBoxContent),
                    typeof(string),
                    typeof(TextBox),
                    new PropertyMetadata(_defaultValue));

        private static double _defaultValue;
        private readonly double _minValue;
        private readonly double _maxValue;
        private readonly double _stepSize;

        public DoubleSpinnerControl(
            double defaultValue,
            double minValue,
            double maxValue,
            double stepSize)
        {
            _defaultValue = defaultValue;
            _minValue = minValue;
            _maxValue = maxValue;
            _stepSize = stepSize;
            InitializeComponent();
        }

        private void ValueChanged()
        {
            var value = Convert.ToDouble(TextBoxContent);
            value = value <= 0 ? 0.1 : value;
            Spinner.ValidSpinDirection = value <= 0.1 ? ValidSpinDirections.Increase : ValidSpinDirections.Increase | ValidSpinDirections.Decrease;
            //DisplayArea.Children.Cast<FontFamilyControl>().ToList().ForEach(c => c.TextBlockFont = new DisplayFontDefinition(c.TextBlockFont.Typeface, value));
        }

        private void SpinnerSpin(object? sender, SpinEventArgs e)
        {
            e.Handled = true;
            var value = Convert.ToDouble(ValueDisplay.Text);
            if (e.Direction == SpinDirection.Increase)
            {
                value += _stepSize;
                if (value > _maxValue)
                {
                    value = _maxValue;
                }
            }
            else
            {
                value -= _stepSize;
                if (value < _minValue)
                {
                    value = _minValue;
                }
            }

            TextBoxContent = value.ToString(CultureInfo.CurrentCulture);
        }
    }
}
