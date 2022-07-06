using System;
using System.Windows;
using System.Windows.Controls;
using ArgonVisual.Widgets.Nodes;

namespace ArgonVisual.Widgets;

/// <summary>
/// The widget that appears when right clicking the the graph panel that allows
/// the user to add new nodes to the graph.
/// </summary>
public class NodeBrowser : StackPanel
{
    private TextButton _addNodeButton;

    /// <summary>
    /// The graph panel to add new node to once it has been created.
    /// </summary>
    public GraphPanel GraphPanel { get; }

    public Action<GraphNodePreview> SpawnNode;

    /// <summary>
    /// Initializes a new instance of <see cref="NodeBrowser"/>.
    /// </summary>
    /// <param name="graphPanel">The graph panel to add new node to once it has been created.</param>
    public NodeBrowser(GraphPanel graphPanel, Action<GraphNodePreview> spawnNode) 
    {
        GraphPanel = graphPanel;
        SpawnNode = spawnNode;

        Children.Add(_addNodeButton = new TextButton("Add New Node"));
        _addNodeButton.Click += AddNewNode;
    }

    private void AddNewNode(object sender, RoutedEventArgs e)
    {
        SpawnNode(new GraphNodePreview());
        GraphPanel.HideNodeBrowser();
    }
}