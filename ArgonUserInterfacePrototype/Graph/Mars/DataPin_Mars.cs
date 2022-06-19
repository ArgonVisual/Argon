using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ArgonUserInterfacePrototype.Graph.Mars;

public class DataPin_Mars : DataPin
{
    public DataPin_Mars(GraphNode outerNode, string name, PinType pinType, PinDirection direction) : base(outerNode, name, pinType, direction)
    {
        TextBlock nameText = new TextBlock()
        {
            Text = name,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 15
        };

        StackPanel panel = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(4)
        };

        if (Direction == PinDirection.Input)
        {
            panel.Children.Add(ConnectorElement);
            panel.Children.Add(nameText);
        }
        else
        {
            panel.Children.Add(nameText);
            panel.Children.Add(ConnectorElement);
        }

        Child = panel;
    }

    private static Brush _hoverBrush = Brushes.Gray;
    private static Brush _normalBrush = Brushes.Transparent;

    protected override FrameworkElement CreateConnectorElement()
    {
        return new Ellipse()
        {
            Width = PinSize,
            Height = PinSize,
            Fill = PinType.Brush,
            Margin = new Thickness(4),
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };
    }
}