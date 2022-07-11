using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArgonVisualX2;

public class CustomTreeViewItem : TreeViewItem
{
    public string HeaderName { get; }

    public virtual ImageSource Icon { get; }

    public CustomTreeViewItem()
    {
        HeaderName = "Tyler";

        Height = 25;

        StackPanel stackPanel = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(0, 0, 0, 2),
            VerticalAlignment = VerticalAlignment.Center
        };

        stackPanel.Children.Add(new Image()
        {
            Source = Icon,
            Width = 20,
            Height = 20,
            Margin = new Thickness(3, 3, 6, 3)
        });

        stackPanel.Children.Add(new TextBlock()
        {
            Text = HeaderName,
            FontSize = 16,
            VerticalAlignment = VerticalAlignment.Center
        });

        Header = stackPanel;
    }
}