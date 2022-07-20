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
}