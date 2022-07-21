using System.Windows;
using System.Windows.Controls;

namespace ScriptingDemo;

public class NodePanel : Panel
{
    public Graph Graph { get; }

    public NodePanel(Graph graph)
    {
        Graph = graph;
    }

    // Override the default Measure method of Panel
    protected override Size MeasureOverride(Size availableSize)
    {
        Size panelDesiredSize = new Size();

        // In our example, we just have one child.
        // Report that our panel requires just the size of its only child.
        foreach (UIElement child in InternalChildren)
        {
            child.Measure(availableSize);
            panelDesiredSize = child.DesiredSize;
        }

        return panelDesiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Point currentPosition = new Point(500, 100);
        Graph.RootNode.ArrangeNode(currentPosition);

        return finalSize; // Returns the final Arranged size
    }
}