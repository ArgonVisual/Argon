using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ArgonVisual.Widgets;
using ArgonVisual.Widgets.Nodes;

namespace ArgonVisual;

public class GraphPanel : Border
{
    private Canvas _nodesCanvas;

    public const double GridSize = 50;
    public const double HalfGridSize = GridSize / 2;

    private bool _isPanning;
    private Point _mouseStartPanning;
    private Point _nodeBrowserPosition;

    private const double _mouseBeginPanning = 20;

    public Point CurrentGraphScreenOffset;
    public Point LastGraphScreenOffset;

    // The node browser that is currently being shown
    private NodeBrowser _nodeBrowser;

    private Popup _nodeBrowserPopup;

    private Grid _mainGrid;

    public GraphPanel() 
    {
        _mainGrid = new Grid();

        CornerRadius = new CornerRadius(10);

        Background = BrushHelper.MakeSolidBrush(20, 20, 25);
        ClipToBounds = true;

        _mainGrid.Children.Add(_nodesCanvas = new Canvas());

        _nodeBrowserPopup = new Popup();
        _nodeBrowserPopup.Placement = PlacementMode.MousePoint;
        _mainGrid.Children.Add(_nodeBrowserPopup);

        _nodeBrowser = new NodeBrowser(this, SpawnNodeFromBrowser);
        _nodeBrowserPopup.Child = _nodeBrowser;

        Child = _mainGrid;
    }

    private void SpawnNodeFromBrowser(GraphNodePreview graphNode)
    {
        AddNode(graphNode, new Point(_nodeBrowserPosition.X - LastGraphScreenOffset.X, _nodeBrowserPosition.Y - LastGraphScreenOffset.Y));
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_isPanning && Point.Subtract(Mouse.GetPosition(this), _mouseStartPanning).LengthSquared >= _mouseBeginPanning)
        {
            Point mousePosition = Mouse.GetPosition(this);
            CurrentGraphScreenOffset = new Point(mousePosition.X - _mouseStartPanning.X + LastGraphScreenOffset.X, mousePosition.Y - _mouseStartPanning.Y + LastGraphScreenOffset.Y);
            _nodesCanvas.Margin = new Thickness(CurrentGraphScreenOffset.X, CurrentGraphScreenOffset.Y, 0, 0);
            InvalidateVisual();
        }
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        _nodeBrowserPopup.IsOpen = false;

        if (e.ChangedButton == MouseButton.Right)
        {
            _isPanning = true;
            _mouseStartPanning = Mouse.GetPosition(this);
        }
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        _isPanning = false;

        Point mousePosition = Mouse.GetPosition(this);
        if (e.ChangedButton == MouseButton.Right && Point.Subtract(mousePosition, _mouseStartPanning).LengthSquared < _mouseBeginPanning)
        {
            _nodeBrowserPopup.IsOpen = true;
            _nodeBrowserPosition = mousePosition;
        }

        LastGraphScreenOffset = CurrentGraphScreenOffset;
    }

    public void HideNodeBrowser() 
    {
        _nodeBrowserPopup.IsOpen = false;
    }

    public void AddNode(GraphNodePreview nodeToAdd, Point position) 
    {
        Canvas.SetLeft(nodeToAdd, position.X);
        Canvas.SetTop(nodeToAdd, position.Y);
        _nodesCanvas.Children.Add(nodeToAdd);
    }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);

        const double dotSize = 3;

        double dotsXOffset = CurrentGraphScreenOffset.X - Math.Floor(CurrentGraphScreenOffset.X / GridSize) * GridSize - GridSize;
        int horizontalDotsNum = (int)(ActualWidth / GridSize) + 3;

        double dotsYOffset = CurrentGraphScreenOffset.Y - Math.Floor(CurrentGraphScreenOffset.Y / GridSize) * GridSize - GridSize;
        int verticalDotsnum = (int)(ActualHeight / GridSize) + 3;

        // const double margin = 8;

        // const double minHeight = margin;
        // double maxHeight = ActualHeight - margin;
        // 
        // const double minWidth = margin;
        // double maxWidth = ActualWidth - margin;

        for (int h = 0; h < horizontalDotsNum; h++)
        {
            for (int v = 0; v < verticalDotsnum; v++)
            {
                // dc.DrawEllipse(_ellipseBrush, null, new Point(Math.Clamp(h * GridSize + dotsXOffset, minWidth, maxWidth), Math.Clamp(v * GridSize + dotsYOffset, minHeight, maxHeight)), dotSize, dotSize);
                dc.DrawEllipse(_ellipseBrush, null, new Point(h * GridSize + dotsXOffset, v * GridSize + dotsYOffset), dotSize, dotSize);
            }
        }
    }

    private static Brush _ellipseBrush = BrushHelper.MakeSolidBrush(40, 40, 45);
}