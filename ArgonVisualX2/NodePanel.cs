using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArgonVisualX2;


public class NodePanel : Panel
{
    private static NodePanel? _global;
    public static NodePanel Global => _global ?? throw new NullReferenceException("NodePanel has not been instanced.");

    public static readonly DependencyProperty NodesOffsetProperty = DependencyProperty.Register("NodesOffset", typeof(Point), typeof(NodePanel), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsArrange));

    public Point NodesOffset
    {
        get => (Point)GetValue(NodesOffsetProperty);
        set => SetValue(NodesOffsetProperty, value);
    }

    static NodePanel()
    {
        ClipToBoundsProperty.OverrideMetadata(typeof(NodePanel), new PropertyMetadata(true));
    }

    // Default public constructor
    public NodePanel()
        : base()
    {
        _global = this;
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
        foreach (ContentPresenter child in InternalChildren)
        {
            NodeData nodeData = (NodeData)child.Content;

            if (nodeData.VisualNameText is null)
            {
                Border border = (Border)VisualTreeHelper.GetChild(child, 0);
                object editableText = VisualTreeHelper.GetChild(border, 0);
                // nodeData.VisualNameText = editableText;
            }

            nodeData.PopulateInlines();

            child.Arrange(new Rect(new Point(nodeData.Position.X + NodesOffset.X, nodeData.Position.Y + NodesOffset.Y), child.DesiredSize));
        }
        return finalSize; // Returns the final Arranged size
    }
}
