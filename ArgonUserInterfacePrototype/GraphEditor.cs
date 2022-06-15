using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ArgonUserInterfacePrototype;

public class GraphEditor : Grid
{
    private Canvas _nodesCanvas;

    private const int _gridSize = 50;

    private static bool _isDragging;

    public Point GraphOffset { get; private set; }
    private Point _graphOffsetBeforeDrag;

    private Point _mouseStartDragPosition;

    public GraphEditor() 
    {
        Background = Brushes.Transparent;

        Children.Add(_nodesCanvas = new Canvas());

        GraphNode node = new GraphNode();
        SetNodeScreenPosition(node, 190, 205);
        AddNode(node);
    }

    public void AddNode(GraphNode node) 
    {
        _nodesCanvas.Children.Add(node);
    }

    public void SetNodeScreenPosition(GraphNode node, Point position)
    {
        Canvas.SetLeft(node, position.X);
        Canvas.SetTop(node, position.Y);
    }

    public void SetNodeScreenPosition(GraphNode node, double x, double y)
    {
        Canvas.SetLeft(node, x);
        Canvas.SetTop(node, y);
    }

    public void SetNodeGraphPosition() 
    {
        throw new NotImplementedException();
    }

    public void SetGraphOffset(Point graphOffset) 
    {
        GraphOffset = graphOffset;
        _nodesCanvas.Margin = new Thickness(graphOffset.X, graphOffset.Y, 0, 0);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_isDragging)
        {
            Point mousePosition = Mouse.GetPosition(this);
            SetGraphOffset(new Point(mousePosition.X - _mouseStartDragPosition.X + _graphOffsetBeforeDrag.X, mousePosition.Y - _mouseStartDragPosition.Y + _graphOffsetBeforeDrag.Y));
        }
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.RightButton == MouseButtonState.Pressed)
        {
            _graphOffsetBeforeDrag = GraphOffset;
            _mouseStartDragPosition = Mouse.GetPosition(this);
            _isDragging = true;
            e.Handled = true;
        }
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (e.RightButton == MouseButtonState.Released)
        {
            _isDragging = false;
            e.Handled = true;
        }
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        if (e.RightButton == MouseButtonState.Pressed)
        {
            _isDragging = false;
            e.Handled = true;
        }
    }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);

        int horizontalLinesAmount = (int)(ActualWidth / _gridSize);
        for (int i = 0; i < horizontalLinesAmount; i++)
        {
            double offset = _gridSize * i + _gridSize;
            dc.DrawLine(_linePen, new Point(offset, 0), new Point(offset, ActualHeight));
        }

        int verticalLinesAmount = (int)(ActualHeight / _gridSize);
        for (int i = 0; i < verticalLinesAmount; i++)
        {
            double offset = _gridSize * i + _gridSize;
            dc.DrawLine(_linePen, new Point(0, offset), new Point(ActualWidth, offset));
        }
    }

    private static Brush _lineBrush = Brushes.Gray;
    private static Pen _linePen = new Pen(_lineBrush, 2);
}

public class GraphNode : Grid
{
    public GraphNode() 
    {
        Width = 250;
        Height = 180;

        this.AddRowFill(new Border() 
        {
            Background = Brushes.LightGray,
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(4)
        });
    }
}