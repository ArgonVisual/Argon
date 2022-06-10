using System.Windows;
using System.Windows.Controls;

namespace Argon.Widgets;

public class ArgonTreeView : TreeView
{
    public ArgonTreeView() 
    {
        Background = GlobalStyle.Transparent;
        BorderBrush = null;
        BorderThickness = new Thickness(0);
    }
}

public class ArgonTreeViewItem : TreeViewItem
{
    public ArgonTreeViewItem(string text) 
    {
        Header = new ArgonTextBlock(text);
    }
}