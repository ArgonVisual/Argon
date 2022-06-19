using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ArgonUserInterfacePrototype.Graph;

public abstract class GraphPanel : Grid
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
        Background = BrushHelper.MakeSolidBrush(200, 200, 200);

        ClipToBounds = true;

        Children.Add(_nodesCanvas = new Canvas());
        Children.Add(_connectionRenderer = new ConnectionRenderer(this));
        Children.Add(new Border()
        {
            BorderBrush = BrushHelper.MakeSolidBrush(120, 120, 120),
            BorderThickness = new Thickness(4),
            Background = null
        });
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

    private static Brush _lineBrush = BrushHelper.MakeSolidBrush(120, 120, 120);
    private static Pen _linePen = new Pen(_lineBrush, 2);
}