using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ArgonUserInterfacePrototype.Graph;

public class ExecutionPin : GraphPin
{
    public ExecutionPin(GraphNode outerNode, PinDirection direction) : base(outerNode, direction, Brushes.White)
    {
        ConnectorElement = CreateConnectorElement();

        Child = ConnectorElement;
    }

    private static Pen _pen = new Pen(Brushes.White, 16)
    {
        EndLineCap = PenLineCap.Round,
        StartLineCap = PenLineCap.Round,
    };

    protected override FrameworkElement CreateConnectorElement()
    {
        return new Ellipse()
        {
            Width = PinSize,
            Height = PinSize,
            Fill = Brushes.White,
            Margin = new Thickness(4),
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };
    }

    public override Pen Pen => _pen;

}