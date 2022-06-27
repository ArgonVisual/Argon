using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArgonVisual.TreeItems;

public abstract class ArgonTreeItem : TreeViewItem
{
    public string Title 
    {
        get => _titleText.Text;
        set => _titleText.Text = value;
    }

    private TextBlock _titleText;

    public ArgonTreeItem(string title) 
    {
        StackPanel stackPanel = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(1, 1, 10, 1)
        };
        
        stackPanel.Children.Add(new Image()
        {
            Source = GetIcon(),
            Width = 23,
            Height = 23,
            Margin = new Thickness(3, 3, 6, 3)
        });

        stackPanel.Children.Add(_titleText = new TextBlock()
        {
            Text = title,
            FontSize = 18,
            VerticalAlignment = VerticalAlignment.Center
        });

        Header = stackPanel;
    }

    protected abstract ImageSource GetIcon();
}