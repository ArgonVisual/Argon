using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScriptingDemo;

/// <summary>
/// The base class for all types of nodes
/// </summary>
public abstract class Node : ContentControl
{
    private Node? _parentNode;
    public Node? ParentNode
    {
        get => _parentNode;
        set
        {
            _parentNode = value;

            Graph? parentGraph = ParentGraph;

            if (parentGraph != null)
            {
                EnumerateAllChildren((node) => parentGraph.NodePanel.Children.Add(node));
            }
        }
    }

    public NodeCollection? ParentCollection { get; set; }

    public virtual Graph? ParentGraph => ParentNode != null ? ParentNode.ParentGraph : null;

    public virtual Point GetConnectionPositionForNodeCollection(NodeCollection nodeCollection, Graph graph)
    {
        return this.GetCenterPositionRelativeTo(graph);
    }

    public void DrawConnectionToParent(DrawingContext dc, Graph graph) 
    {
        if (ParentNode != null && ParentCollection != null)
        {
            dc.DrawConnection(this.GetCenterPositionRelativeTo(graph), ParentNode.GetConnectionPositionForNodeCollection(ParentCollection, graph));
        }
    }

    public abstract void EnumerateDirectChildren(Action<Node> action);

    public void EnumerateAllChildren(Action<Node> action) 
    {
        action(this);
        EnumerateDirectChildren((node) => node.EnumerateAllChildren(action));
    }

    public void ArrangeNode(Point position) 
    {
        Arrange(new Rect(position, DesiredSize));
        ArrangeChildren(new Point(position.X, position.Y + ActualHeight + 30));
    }

    protected abstract void ArrangeChildren(Point position);

    public double CalculateWidth(Graph graph)
    {
        double min = double.MaxValue;
        double max = double.MinValue;

        EnumerateAllChildren((node) => 
        {
            double leftX = node.GetLeftPositionRelativeTo(graph).X;
            double rightX = node.GetRightPositionRelativeTo(graph).X;

            if (rightX > max)
            {
                max = rightX;
            }

            if (leftX < min)
            {
                min = leftX;
            }
        });

        if (min == double.MaxValue || max == double.MinValue)
        {
            return 0;
        }

        return max - min;
    }
}