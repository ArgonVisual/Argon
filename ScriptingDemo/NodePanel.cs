using System.Windows;
using System.Windows.Controls;

namespace ScriptingDemo;

public class NodePanel : Panel
{
    public Point ScreenOffset { get; set; }
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
        Node currentNode = Graph.RootNode;
        Point currentPosition = new Point(600, 100);
        Graph.RootNode.Arrange(new Rect(new Point(currentPosition.X + ScreenOffset.X, currentPosition.Y + ScreenOffset.Y), Graph.RootNode.DesiredSize));

        currentNode.EnumerateAllChildren((node) => 
        {
            currentPosition.Y += _incrementSize;
            node.Arrange(new Rect(new Point(currentPosition.X + ScreenOffset.X, currentPosition.Y + ScreenOffset.Y), node.DesiredSize));
        });

        return finalSize; // Returns the final Arranged size
    }

    private const double _incrementSize = 100;
}