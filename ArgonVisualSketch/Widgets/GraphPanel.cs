using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ArgonVisual;

public class GraphPanel : Border
{
    private Canvas _nodesCanvas;

    public const double GridSize = 50;
    public const double HalfGridSize = GridSize / 2;

    public Point GraphOffset;

    public GraphPanel() 
    {
        Grid grid = new Grid();

        CornerRadius = new CornerRadius(10);

        Background = BrushHelper.MakeSolidBrush(20, 20, 25);
        ClipToBounds = true;

        grid.Children.Add(_nodesCanvas = new Canvas());

        Child = grid;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        // InvalidateVisual();
        // Point mousePosition = Mouse.GetPosition(this);
        // GraphOffset = mousePosition;
    }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);

        const double dotSize = 3;

        double dotsXOffset = GraphOffset.X - Math.Floor(GraphOffset.X / GridSize) * GridSize - GridSize;
        int horizontalDotsNum = (int)(ActualWidth / GridSize) + 3;

        double dotsYOffset = GraphOffset.Y - Math.Floor(GraphOffset.Y / GridSize) * GridSize - GridSize;
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