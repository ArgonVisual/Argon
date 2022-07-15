using System.Collections.Generic;
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
        // Draw parameters dots
#if false

        IEnumerable<Parameter> parameters = _graph.GetAllParameters();
        foreach (Parameter parameter in parameters)
        {
            // IReadOnlyList<Parameter> connectedParameters = parameter.ConnectedOut;
            // foreach (Parameter connectedParameter in connectedParameters)
            // {
            //     DrawConnection(dc, GetParameterPosition(parameter), GetParameterPosition(connectedParameter));
            // }

            if (_graph.IsAncestorOf(parameter))
            {
                Point top = GetParameterConnnectionPosition(parameter, false);
                Point bottom = GetParameterConnnectionPosition(parameter, true);

                DrawEllipse(dc, top);
                DrawEllipse(dc, bottom);
            }
        }
#endif

        if (_graph.DraggedParameter != null)
        {
            Parameter parameter = _graph.DraggedParameter;
            Parameter? hoveredParameter = _graph.HoveredParameter;

            Point parameterPosition = GetParameterConnnectionPosition(parameter, true);
            Point mousePosition = Mouse.GetPosition(_graph);

            Point startPosition = new Point(parameterPosition.X + (parameter.ActualWidth / 2), parameterPosition.Y + parameter.ActualHeight);
            Point endPosition = mousePosition;

            if (hoveredParameter != null)
            {
                Point hoveredParameterPosition = GetParameterConnnectionPosition(hoveredParameter, true);
                endPosition = new Point(hoveredParameterPosition.X + (hoveredParameter.ActualWidth / 2), hoveredParameterPosition.Y);
            }

            DrawConnection(dc, startPosition, endPosition);
        }
    }

    private void DrawEllipse(DrawingContext dc, Point position)
    {
        dc.DrawEllipse(_redBrush, null, position, 10, 10);
    }

    private void DrawConnection(DrawingContext dc, Point from, Point to)
    {
        dc.DrawLine(_redPen, from, to);
        dc.DrawEllipse(_redBrush, null, from, 15, 15);
        dc.DrawEllipse(_redBrush, null, to, 15, 15);
    }

    private Point GetParameterConnnectionPosition(Parameter parameter, bool bottom)
    {
        Point position = parameter.TransformToAncestor(_graph).Transform(new Point(0, 0));
        Point centerTop = new Point(position.X + (parameter.ActualWidth / 2), position.Y);

        if (bottom)
        {
            return new Point(centerTop.X, centerTop.Y + parameter.ActualHeight);
        }
        else
        {
            return centerTop;
        }
    }

    private static Brush _redBrush = Brushes.Red;
    private static Pen _redPen = new Pen(_redBrush, 5);
}