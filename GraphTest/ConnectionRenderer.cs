using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphTest;

public class ConnectionRenderer : FrameworkElement
{
    private Graph _graph;

    public ConnectionRenderer(Graph graph) 
    {
        IsHitTestVisible = false;
        _graph = graph;
    }

    protected override void OnRender(DrawingContext dc)
    {
        if (_graph.DraggedParameter != null)
        {
            Parameter parameter = _graph.DraggedParameter;

            Point parameterPosition = parameter.TransformToAncestor(_graph).Transform(new Point(0, 0));
            Point mousePosition = Mouse.GetPosition(_graph);

            Point startPosition = new Point(parameterPosition.X + (parameter.ActualWidth / 2), parameterPosition.Y + parameter.ActualHeight);
            Point endPosition = mousePosition;

            dc.DrawLine(_redPen, startPosition, endPosition);
            dc.DrawEllipse(_redBrush, null, startPosition, 8, 8);
            dc.DrawEllipse(_redBrush, null, endPosition, 8, 8);
        }
    }

    private static Brush _redBrush = Brushes.Red;
    private static Pen _redPen = new Pen(_redBrush, 5);
}