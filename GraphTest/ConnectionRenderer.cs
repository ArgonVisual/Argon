using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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
        IEnumerable<Parameter> parameters = _graph.GetAllParameters();
        foreach (Parameter parameter in parameters)
        {
            if (parameter.ConnectedOut != null)
            {
                DrawConnection(dc, parameter, parameter.ConnectedOut);
            }
        }

        if (_graph.DraggedParameter != null)
        {
            Parameter parameter = _graph.DraggedParameter;
            Parameter? hoveredParameter = _graph.HoveredParameter;

            Point mousePosition = Mouse.GetPosition(_graph);
            Point parameterPosition = GetParameterConnnectionPosition(parameter, mousePosition.Y, out bool bottom);

            Point startPosition = new Point(parameterPosition.X, parameterPosition.Y);
            Point endPosition = mousePosition;

            if (hoveredParameter != null && Parameter.CanConnect(hoveredParameter, parameter))
            {
                Point hoveredParameterPosition = GetParameterConnnectionPosition(hoveredParameter, !bottom);
                endPosition = new Point(hoveredParameterPosition.X, hoveredParameterPosition.Y);
            }
            else
            {
                dc.DrawEllipse(_redBrush, null, endPosition, 8, 8);
            }

            dc.DrawLine(_redPen, startPosition, endPosition);
        }
    }

    private void DrawConnection(DrawingContext dc, Parameter from, Parameter to) 
    {
        Point fromPos = GetParameterConnnectionPosition(from, true);
        Point toPos = GetParameterConnnectionPosition(to, false);

        dc.DrawLine(_redPen, fromPos, toPos);
    }

    private void DrawEllipse(DrawingContext dc, Point position)
    {
        dc.DrawEllipse(_redBrush, null, position, 10, 10);
    }

    private Point GetParameterConnnectionPosition(Parameter parameter, bool bottom)
    {
        Point position = parameter.TransformToAncestor(_graph).Transform(new Point(0, 0));
        Point centerTop = new Point(position.X + (parameter.ActualWidth / 2), position.Y);

        const double offset = 3;

        if (bottom)
        {
            return new Point(centerTop.X, centerTop.Y + parameter.ActualHeight + offset);
        }
        else
        {
            return new Point(centerTop.X, centerTop.Y - offset);
        }
    }

    private Point GetParameterConnnectionPosition(Parameter parameter, double yPos, out bool bottom)
    {
        Point position = parameter.TransformToAncestor(_graph).Transform(new Point(0, 0));
        Point centerTop = new Point(position.X + (parameter.ActualWidth / 2), position.Y);

        const double offset = 3;

        if (position.Y + (parameter.ActualHeight / 2) < yPos)
        {
            bottom = true;
            return new Point(centerTop.X, centerTop.Y + parameter.ActualHeight + offset);
        }
        else
        {
            bottom = false;
            return new Point(centerTop.X, centerTop.Y - offset);
        }
    }

    private static Brush _redBrush = Brushes.Red;
    private static Pen _redPen = new Pen(_redBrush, 5);
}