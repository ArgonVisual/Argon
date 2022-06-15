using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ArgonUserInterfacePrototype.PinTypes;

namespace ArgonUserInterfacePrototype;

public class GraphPanel : Grid
{
    private class ConnectionRenderer : FrameworkElement
    {
        private GraphPanel _outerGraph;

        public ConnectionRenderer(GraphPanel graphPanel)
        {
            IsHitTestVisible = false;
            _outerGraph = graphPanel;

            // Path path = new Path() 
            // {
            //     Stroke = Brushes.Black,
            //     StrokeThickness = 10
            // };
            // 
            // PathGeometry pathGeometry = new PathGeometry();
            // 
            // PathFigureCollection figureCollection = new PathFigureCollection();
            // 
            // PathSegmentCollection pathSegments = new PathSegmentCollection();
            // 
            // PolyBezierSegment polyBezierSegment = new PolyBezierSegment();
            // 
            // PointCollection pointCollection = new PointCollection();
            // 
            // pointCollection.Add(new Point(90, 200));
            // pointCollection.Add(new Point(140, 200));
            // pointCollection.Add(new Point(160, 200));
            // 
            // polyBezierSegment.Points = pointCollection;
            // 
            // pathSegments.Add(polyBezierSegment);
            // 
            // figureCollection.Add(new PathFigure() 
            // {
            //     StartPoint = new Point(100, 100),
            //     Segments = pathSegments
            // });
            // 
            // pathGeometry.Figures = figureCollection;
            // 
            // path.Data = pathGeometry;
            // 
            // Children.Add(path);
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (_outerGraph.DraggedPin is not null)
            {
                Point mousePosition = Mouse.GetPosition(this);

                DrawLine(dc, _outerGraph.DraggedPin.Pen, _outerGraph.DraggedPin.GetPositionInGraphSpace(), mousePosition);
            }

            IEnumerable<GraphNode> nodes = _outerGraph.GetAllNodes();
            foreach (GraphNode node in nodes)
            {
                IEnumerable<GraphPin> outputPins = node.GetOutputPins();
                foreach (GraphPin outputPin in outputPins)
                {
                    if (outputPin.ConnectedPin is not null)
                    {
                        DrawLine(dc, outputPin.Pen, outputPin.GetPositionInGraphSpace(), outputPin.ConnectedPin.GetPositionInGraphSpace());
                    }
                }
            }
        }

        private void DrawLine(DrawingContext dc, Pen pen, Point start, Point end) 
        {
            dc.DrawLine(_outerPen, start, end);
            dc.DrawLine(pen, start, end);
        }

        private static Pen _outerPen = new Pen(Brushes.Black, 20)
        {
            EndLineCap = PenLineCap.Round,
            StartLineCap = PenLineCap.Round,
        };
    }

    public GraphPin? DraggedPin;
    public GraphPin? HoveredPin;

    public GraphNode? DraggedNode;
    public Point MouseOffsetInDraggedNode;

    private Canvas _nodesCanvas;

    private const int _gridSize = 50;
    private const int _halfGridSize = _gridSize / 2;

    private static bool _isPanning;

    public Point GraphOffset { get; private set; }
    private Point _graphOffsetBeforeDrag;
    private Point _mouseStartDragPosition;

    private ConnectionRenderer _connectionRenderer;

    public GraphPanel()
    {
        Background = BrushHelper.MakeSolidBrush(80, 80, 80);

        ClipToBounds = true;

        Children.Add(_nodesCanvas = new Canvas());
        Children.Add(_connectionRenderer = new ConnectionRenderer(this));
        Children.Add(new Border() 
        {
            BorderBrush = Brushes.DarkGray,
            BorderThickness = new Thickness(4),
            Background = null
        });

        GraphNode node1 = new GraphNode(this);
        SetNodeScreenPosition(node1, 190, 205);
        AddNode(node1);

        GraphNode node2 = new GraphNode(this);
        SetNodeScreenPosition(node2, 800, 220);
        AddNode(node2);
    }

    public void AddNode(GraphNode node) 
    {
        _nodesCanvas.Children.Add(node);
    }

    public void SetNodeScreenPosition(GraphNode node, Point position)
    {
        Canvas.SetLeft(node, position.X - GraphOffset.X);
        Canvas.SetTop(node, position.Y - GraphOffset.Y);
    }

    public void SetNodeScreenPosition(GraphNode node, double x, double y)
    {
        Canvas.SetLeft(node, x - GraphOffset.X);
        Canvas.SetTop(node, y - GraphOffset.Y);
    }

    public void SetNodeGraphPosition() 
    {
        throw new NotImplementedException();
    }

    public void SetGraphOffset(Point graphOffset) 
    {
        GraphOffset = graphOffset;
        _nodesCanvas.Margin = new Thickness(graphOffset.X, graphOffset.Y, 0, 0);
        // Redraw the lines
        InvalidateVisual();
        _connectionRenderer.InvalidateVisual();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_isPanning)
        {
            Point mousePosition = Mouse.GetPosition(this);
            SetGraphOffset(new Point(mousePosition.X - _mouseStartDragPosition.X + _graphOffsetBeforeDrag.X, mousePosition.Y - _mouseStartDragPosition.Y + _graphOffsetBeforeDrag.Y));
        }
        else if (DraggedNode is not null)
        {
            Point mousePosition = Mouse.GetPosition(this);
            SetNodeScreenPosition(DraggedNode, new Point(mousePosition.X - MouseOffsetInDraggedNode.X, mousePosition.Y - MouseOffsetInDraggedNode.Y));
        }

        _connectionRenderer.InvalidateVisual();
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right && DraggedNode is null && DraggedPin is null)
        {
            _graphOffsetBeforeDrag = GraphOffset;
            _mouseStartDragPosition = Mouse.GetPosition(this);
            _isPanning = true;
            e.Handled = true;
        }
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right)
        {
            _isPanning = false;
            e.Handled = true;
        }
        else if (e.ChangedButton == MouseButton.Left)
        {
            if (DraggedPin is not null && HoveredPin is not null)
            {
                if (DraggedPin.Direction != HoveredPin.Direction)
                {
                    if (DraggedPin is ExecutionPin && HoveredPin is ExecutionPin)
                    {
                        DraggedPin.ConnectTo(HoveredPin);
                    }
                    else if (DraggedPin is DataPin draggedData && HoveredPin is DataPin hoveredData && draggedData.PinType == hoveredData.PinType)
                    {
                        DraggedPin.ConnectTo(HoveredPin);
                    }
                }
            }
            else if (DraggedNode is not null)
            {
                DraggedNode = null;
            }
        }

        DraggedPin = null;
        _connectionRenderer.InvalidateVisual();
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        if (e.RightButton == MouseButtonState.Pressed)
        {
            _isPanning = false;
            e.Handled = true;
        }
    }

    public IEnumerable<GraphNode> GetAllNodes() 
    {
        foreach (GraphNode node in _nodesCanvas.Children)
        {
            yield return node;
        }
    }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);

        double linesXOffset = GraphOffset.X - Math.Round(GraphOffset.X / _gridSize) * _gridSize + _halfGridSize;
        int horizontalLinesAmount = (int)(ActualWidth / _gridSize) + 1;
        for (int i = 0; i < horizontalLinesAmount; i++)
        {
            double offset = _gridSize * i + linesXOffset;
            dc.DrawLine(_linePen, new Point(offset, 0), new Point(offset, ActualHeight));
        }

        double linesYOffset = GraphOffset.Y - Math.Round(GraphOffset.Y / _gridSize) * _gridSize + _halfGridSize;
        int verticalLinesAmount = (int)(ActualHeight / _gridSize) + 1;
        for (int i = 0; i < verticalLinesAmount; i++)
        {
            double offset = _gridSize * i + linesYOffset;
            dc.DrawLine(_linePen, new Point(0, offset), new Point(ActualWidth, offset));
        }
    }

    private static Brush _lineBrush = Brushes.Gray;
    private static Pen _linePen = new Pen(_lineBrush, 2);
}

public class GraphNode : Grid
{
    public GraphPanel OuterGraph { get; }

    private StackPanel _inputPins;
    private StackPanel _outputPins;

    public GraphNode(GraphPanel graphPanel)
    {
        OuterGraph = graphPanel;

        StackPanel titlePanel = new StackPanel() 
        {
            Orientation = Orientation.Horizontal
        };

        titlePanel.Children.Add(new Image() 
        {
            Source = ArgonIcons.FunctionIcon
        });

        titlePanel.Children.Add(new TextBlock()
        {
            Text = "MyFunctionName",
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 20,
            Margin = new Thickness(10, 0, 10, 0)
        });

        this.AddRowFill(new Border()
        {
            Background = BrushHelper.MakeSolidBrush(234, 225, 70),
            Height = 40,
            Child = titlePanel
        });

        Grid pinsPanel = new Grid();

        pinsPanel.AddColumnFill(_inputPins = new StackPanel() 
        {
            HorizontalAlignment = HorizontalAlignment.Left
        });
        pinsPanel.AddColumnFill(_outputPins = new StackPanel() 
        {
            HorizontalAlignment = HorizontalAlignment.Right
        });

        _inputPins.Children.Add(new DataPin(this, "Age", PT_Integer.Instance, GraphPin.PinDirection.Input));
        _inputPins.Children.Add(new DataPin(this, "Name", PT_String.Instance, GraphPin.PinDirection.Input));

        _outputPins.Children.Add(new DataPin(this, "Description", PT_String.Instance, GraphPin.PinDirection.Output));

        Grid executionPanel = new Grid();

        executionPanel.AddColumnFill(new ExecutionPin(this, GraphPin.PinDirection.Input));
        executionPanel.AddColumnFill(new ExecutionPin(this, GraphPin.PinDirection.Output));

        this.AddRowAuto(new Border()
        {
            Background = Brushes.White,
            Child = executionPanel
        });

        this.AddRowFill(new Border()
        {
            Background = Brushes.LightGray,
            BorderBrush = Brushes.Gray,
            Height = 180,
            Child = pinsPanel
        });
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            OuterGraph.DraggedNode = this;
            OuterGraph.MouseOffsetInDraggedNode = Mouse.GetPosition(this);
            e.Handled = true;
        }
    }

    public IEnumerable<GraphPin> GetOutputPins() 
    {
        foreach (GraphPin outputPin in _outputPins.Children)
        {
            yield return outputPin;
        }
    }

    public IEnumerable<GraphPin> GetInputPins()
    {
        foreach (GraphPin inputPin in _inputPins.Children)
        {
            yield return inputPin;
        }
    }
}

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

    protected Ellipse ConnectorEllipse;

    protected GraphPin(GraphNode outerNode, PinDirection direction, Brush pinTypeBrush) 
    {
        Background = _normalBrush;

        OuterNode = outerNode;
        Direction = direction;

        ConnectorEllipse = new Ellipse()
        {
            Width = PinSize,
            Height = PinSize,
            Fill = pinTypeBrush,
            Margin = new Thickness(4),
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };
    }

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
        return ConnectorEllipse.TransformToAncestor(OuterNode.OuterGraph).Transform(new Point(HalfPinSize, HalfPinSize));
    }

    public abstract Pen Pen { get; }
}

public class ExecutionPin : GraphPin
{
    public ExecutionPin(GraphNode outerNode, PinDirection direction) : base(outerNode, direction, Brushes.White)
    {
        Child = ConnectorEllipse;
    }

    private static Pen _pen = new Pen(Brushes.White, 16)
    {
        EndLineCap = PenLineCap.Round,
        StartLineCap = PenLineCap.Round,
    };

    public override Pen Pen => _pen;
}

public class DataPin : GraphPin
{
    public PinType PinType { get; }

    public DataPin(GraphNode outerNode, string name, PinType pinType, PinDirection direction) : base(outerNode, direction, pinType.Brush)
    {
        PinType = pinType;

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
            panel.Children.Add(ConnectorEllipse);
            panel.Children.Add(nameText);
            HorizontalAlignment = HorizontalAlignment.Left;
        }
        else
        {
            panel.Children.Add(nameText);
            panel.Children.Add(ConnectorEllipse);
            HorizontalAlignment = HorizontalAlignment.Right;
        }

        Child = panel;
    }

    public override Pen Pen => PinType.Pen;

    private static Brush _hoverBrush = Brushes.Gray;
    private static Brush _normalBrush = Brushes.Transparent;
}