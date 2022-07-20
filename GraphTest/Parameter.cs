using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphTest;

public class Parameter : Border
{
    public Parameter? ConnectedIn { get; private set; }
    public Parameter? ConnectedOut { get; private set; }

    public bool HasConnection => ConnectedIn != null || ConnectedOut != null;

    public string ParameterName { get; }

    public const double DotOffset = 10;

    public Node? ParentNode 
    { 
        get 
        {
            DependencyObject parent = Parent;

            while (parent != null)
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
                else if (parent is FrameworkContentElement frameworkContentElement)
                {
                    parent = frameworkContentElement.Parent;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }
    }

    public void StopDragging() 
    {
        if (IsMouseOver)
        {
            Background = _hoverBrush;
        }
        else
        {
            Background = _normalBrush;
        }
    }

    public Parameter(string name)
    {
        CornerRadius = new CornerRadius(5);
        Background = _normalBrush;
        Child = new TextBlock()
        {
            Text = ParameterName = name,
            FontSize = 25,
            Foreground = Brushes.White,
            Margin = new Thickness(5, 0, 5, 0)
        };
    }

    public static bool CanConnect(Parameter a, Parameter b) 
    {
        if (a.ParentNode == b.ParentNode)
        {
            return false;
        }

        return true;
    }

    public void AddChildNodes(List<Node> nodes) 
    {
        PopulateChildNodesInternal(nodes);
    }

    private void PopulateChildNodesInternal(List<Node> nodes)
    {
        Node? parentNode = ParentNode;
        if (parentNode != null)
        {
            nodes.Add(parentNode);
        }

        if (ConnectedOut != null && ConnectedOut.ParentNode != null)
        {
            ConnectedOut.ParentNode.AddChildNodesInternal(nodes);
        }
    }

    public void DisconnectIn() 
    {
        if (ConnectedIn != null)
        {
            ConnectedIn.ConnectedOut = null;
            ConnectedIn = null;
        }
    }

    public void DisconnectOut()
    {
        if (ConnectedOut != null)
        {
            ConnectedOut.ConnectedIn = null;
            ConnectedOut = null;
        }
    }

    public void ConnectTo(Parameter parameter) 
    {
        DisconnectOut();
        parameter.DisconnectIn();

        ConnectedOut = parameter;
        parameter.ConnectedIn = this;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {

            }
            else
            {
                Graph.Global.DraggedParameter = this;
                Background = _draggedBrush;
            }

            e.Handled = true;
        }
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (Graph.Global.HoveredParameter != null && Graph.Global.DraggedParameter != null)
        {
            // Connect DraggedParameter to HoveredParameter

            Point hoveredPosition = TransformToAncestor(Graph.Global).Transform(new Point(0, 0));
            Point draggedPosition = Graph.Global.DraggedParameter.TransformToAncestor(Graph.Global).Transform(new Point(0, 0));

            if (draggedPosition.Y <= hoveredPosition.Y)
            {
                Graph.Global.DraggedParameter.ConnectTo(this);
            }
            else
            {
                this.ConnectTo(Graph.Global.DraggedParameter);
            }

            
            Graph.Global.HoveredParameter = null;
        }
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        if (Graph.Global.DraggedParameter != null)
        {
            if (CanConnect(this, Graph.Global.DraggedParameter))
            {
                Background = _hoverBrush;
            }
        }
        else
        {
            Background = _hoverBrush;
        }

        Graph.Global.HoveredParameter = this;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        if (Graph.Global.DraggedParameter != null && this == Graph.Global.DraggedParameter)
        {
            Background = _draggedBrush;
        }
        else
        {
            Background = _normalBrush;
        }
        
        Graph.Global.HoveredParameter = null;
    }

    protected override void OnRender(DrawingContext dc)
    {
        double center = ActualWidth / 2;


        dc.DrawEllipse(_redBrush, null, new Point(center, 0 - DotOffset), _ellipseSize, _ellipseSize);
        dc.DrawEllipse(_redBrush, null, new Point(center, ActualHeight + DotOffset), _ellipseSize, _ellipseSize);
        base.OnRender(dc);
    }

    private const double _ellipseSize = 5;

    private static Brush _redBrush = Brushes.Red;

    private static Brush _normalBrush = Brushes.DarkCyan;
    private static Brush _hoverBrush = Brushes.DarkSlateBlue;
    private static Brush _draggedBrush = Brushes.DarkGoldenrod;
}