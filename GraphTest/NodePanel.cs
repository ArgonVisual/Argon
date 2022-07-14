using System.Windows;
using System.Windows.Controls;

namespace GraphTest;

public class NodePanel : Panel
{
    public Point ScreenOffset { get; set; }

    public NodePanel() 
    {
        
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
        foreach (Node child in InternalChildren)
        {
            child.Arrange(new Rect(new Point(child.Position.X + ScreenOffset.X, child.Position.Y + ScreenOffset.Y), child.DesiredSize));
        }
        return finalSize; // Returns the final Arranged size
    }
}