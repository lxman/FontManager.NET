using System.Windows;
using System.Windows.Controls;

namespace FontManager.NET.Controls
{
    /// <summary>
    /// Interaction logic for FontFamilyListControl.xaml
    /// </summary>
    public partial class FontFamilyListControl : UserControl
    {
        #region Family List DP

        public List<ListBoxItem> FamilyList
        {
            get => (List<ListBoxItem>)GetValue(FamilyListProperty);
            set
            {
                SetValue(FamilyListProperty, value);
                OnPropertyChanged(new DependencyPropertyChangedEventArgs(FamilyListProperty, null, null));
            }
        }

        public static readonly DependencyProperty FamilyListProperty =
            DependencyProperty
                .Register(
                    nameof(FamilyList),
                    typeof(List<ListBoxItem>),
                    typeof(ListBox),
                    new PropertyMetadata(new List<ListBoxItem>()));

        #endregion

        public FontFamilyListControl()
        {
            InitializeComponent();
        }
    }
}