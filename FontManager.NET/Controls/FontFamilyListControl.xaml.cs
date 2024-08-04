using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace FontManager.NET.Controls
{
    /// <summary>
    /// Interaction logic for FontFamilyListControl.xaml
    /// </summary>
    public partial class FontFamilyListControl : UserControl
    {
        #region Observable Collection Family List

        public ObservableCollection<ListBoxItem> ObservableCollectionFamilyList { get; } = [];

        #endregion Observable Collection Family List

        public FontFamilyListControl()
        {
            InitializeComponent();
        }
    }
}