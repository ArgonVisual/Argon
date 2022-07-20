using System.Windows;
using System.Windows.Media;

namespace ScriptingDemo;

public class ConnectionRenderer : FrameworkElement
{
    public Graph Graph { get; }

    public ConnectionRenderer(Graph graph)
    {
        Graph = graph;
    }

    protected override void OnRender(DrawingContext dc)
    {
        Graph.RootNode.EnumerateAllChildren((node) => node.DrawConnectionToParent(dc, Graph));
    }
}