using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScriptingDemo;

/// <summary>
/// Represents the first node in a graph. 
/// This node will never have a parent node since it 
/// will be the parent of all other nodes in the graph.
/// </summary>
public class RootNode : Node
{
    public NodeCollection Nodes { get; }

    private Graph _parentGraph;

    public override Graph? ParentGraph => _parentGraph;

    /// <summary>
    /// Intializes a new instance of <see cref="RootNode"/>.
    /// </summary>
    /// <param name="graph">The <see cref="Graph"/> that owns this node.</param>
    public RootNode(Graph graph) 
    {
        _parentGraph = graph;
        Nodes = new NodeCollection(this);

        Content = new Border()
        {
            Background = VisualStyle.NodeBackground,
            CornerRadius = VisualStyle.CornerRadius,
            Child = new TextBlock()
            {
                Text = "Root Node",
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(8, 4, 8, 0),
            }
        };
    }

    public override void EnumerateDirectChildren(Action<Node> action)
    {
        foreach (Node node in Nodes)
        {
            action(node);
        }
    }

    protected override void ArrangeChildren(Point position)
    {
        Nodes.Arrange(position);
    }
}