using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ScriptingDemo;

/// <summary>
/// The base class for all types of nodes
/// </summary>
public abstract class Node : ContentControl
{
    /// <summary>
    /// The position relative to the graph as to where the node should be rendered.
    /// If a node has a parent, it's position it relative to it's parents position.
    /// Else the user can drag the node anywhere they want until a connection to a parent node is created.
    /// </summary>
    public Point Position { get; set; }

    private Node? _parentNode;
    public Node? ParentNode
    {
        get => _parentNode;
        set
        {
            _parentNode = value;
            if (ParentGraph != null)
            {
                AddAllChildNodesToGraph(ParentGraph);
            }
        }
    }

    public virtual Graph? ParentGraph => ParentNode != null ? ParentNode.ParentGraph : null;

    public abstract IEnumerable<Node> GetDirectChildNodes();

    public void AddAllChildNodesToGraph(Graph graph) 
    {
        IEnumerable<Node> childNodes = GetDirectChildNodes();
        foreach (Node node in childNodes)
        {
            graph.NodePanel.Children.Add(node);
            node.AddAllChildNodesToGraph(graph);
        }
    }
}