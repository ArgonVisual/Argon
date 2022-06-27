using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ArgonVisual;

public class DocumentTabItem : TabItem
{
    private static Brush _textBrush = Brushes.Black;

    public DocumentTabItem(string title) 
    {
        Header = new TextBlock()
        {
            Text = title,
            FontSize = 16,
            Foreground = _textBrush
        };

        Grid grid = new Grid()
        {
            Background = BrushHelper.MakeSolidBrush(30, 30, 34)
        };
        StackPanel stackPanel = new StackPanel() 
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(5, 3, 5, 0)
        };

        stackPanel.Children.Add(new DocumentItem("MyClass") { IsSelected = true });
        stackPanel.Children.Add(new DocumentItem("MyOtherClass"));
        stackPanel.Children.Add(new DocumentItem("MyStruct", true));

        grid.AddRowAuto(stackPanel);
        grid.AddRowFill(new GraphPanel() { Margin = new Thickness(5) });

        Content = grid;
    }
}