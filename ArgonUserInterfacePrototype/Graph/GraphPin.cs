using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Controls;
using System;
using System.Windows.Media;

namespace ArgonUserInterfacePrototype.Graph;

public abstract class GraphPin : Border
{
    public enum PinDirection
    {
        Input,
        Output
    }

    public PinDirection Direction { get; }
    public GraphPin? ConnectedPin { get; private set; }
    public GraphNode OuterNode { get; }

    public const double PinSize = 20;
    public const double HalfPinSize = PinSize / 2;

    protected GraphPin(GraphNode outerNode, PinDirection direction, Brush pinTypeBrush)
    {
        Background = _normalBrush;

        OuterNode = outerNode;
        Direction = direction;

        if (Direction == PinDirection.Input)
        {
            HorizontalAlignment = HorizontalAlignment.Left;
        }
        else
        {
            HorizontalAlignment = HorizontalAlignment.Right;
        }
    }

    protected abstract FrameworkElement CreateConnectorElement();

    protected FrameworkElement ConnectorElement;

    private static Brush _hoverBrush = Brushes.Gray;
    private static Brush _normalBrush = Brushes.Transparent;

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        OuterNode.OuterGraph.HoveredPin = this;
        Background = _hoverBrush;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        OuterNode.OuterGraph.HoveredPin = null;
        Background = _normalBrush;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            OuterNode.OuterGraph.DraggedPin = this;
            e.Handled = true;
        }
    }

    public void ConnectTo(GraphPin pin)
    {
        if (pin.Direction == this.Direction)
        {
            throw new ArgumentException("Pin must have opposite direction.", nameof(pin));
        }

        pin.ConnectedPin = this;
        ConnectedPin = pin;
    }

    public Point GetPositionInGraphSpace()
    {
        return ConnectorElement.TransformToAncestor(OuterNode.OuterGraph).Transform(new Point(HalfPinSize, HalfPinSize));
    }

    public abstract Pen Pen { get; }
}