using System.Windows.Controls;

namespace FontExplorer
{
    public static class TreeViewItemExtensions
    {
        public static TreeViewItem FormChild(this TreeViewItem parent, string varName, object value)
        {
            var child = new TreeViewItem { Header = $"{varName}: {value}" };
            parent.Items.Add(child);
            return child;
        }

        public static TreeViewItem FormChild(this TreeViewItem parent, string text)
        {
            var child = new TreeViewItem { Header = text };
            parent.Items.Add(child);
            return child;
        }
    }
}
