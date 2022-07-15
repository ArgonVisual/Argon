using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphTest;

public class Parameter : Border
{
    public Parameter? ConnectedIn { get; private set; }

    public IReadOnlyList<Parameter> ConnectedOut => _connectedOut;

    public List<Parameter> _connectedOut;

    public Node ParentNode 
    { 
        get 
        {
            DependencyObject parent = Parent;

            while (parent is not null)
            {
                Node? node = parent as Node;
                if (node is not null)
                {
                    return node;
                }
                else if (parent is FrameworkElement frameworkElement)
                {
                    parent = frameworkElement.Parent;
                }
            }

            throw new Exception("Node does not have an parent");
        }
    }

    public Parameter(string name)
    {
        _connectedOut = new List<Parameter>();

        CornerRadius = new CornerRadius(5);
        Background = Brushes.DarkCyan;
        Child = new TextBlock()
        {
            Text = name,
            FontSize = 25,
            Foreground = Brushes.White,
            Margin = new Thickness(5, 0, 5, 0)
        };
    }

    public void ConnectTo(Parameter parameter) 
    {
        parameter._connectedOut.Add(this);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            Graph.Global.DraggedParameter = this;
            e.Handled = true;
        }
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (Graph.Global.HoveredParameter != null && Graph.Global.DraggedParameter != null)
        {
            Graph.Global.DraggedParameter.ConnectTo(this);
            Graph.Global.HoveredParameter = null;
        }
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        Background = Brushes.DarkSlateBlue;
        Graph.Global.HoveredParameter = this;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        Background = Brushes.DarkCyan;
        Graph.Global.HoveredParameter = null;
    }

    protected override void OnRender(DrawingContext dc)
    {
        double center = ActualWidth / 2;
        dc.DrawEllipse(_redBrush, null, new Point(center, 0), _ellipseSize, _ellipseSize);
        dc.DrawEllipse(_redBrush, null, new Point(center, ActualHeight), _ellipseSize, _ellipseSize);
        base.OnRender(dc);
    }

    private const double _ellipseSize = 15;

    private static Brush _redBrush = Brushes.Red;
}