using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ArgonVisual.Widgets.Nodes;

/// <summary>
/// Base class for a node that can be shown on a <see cref="GraphPanel"/>.
/// </summary>
public class GraphNodePreview : Border
{
    private static Brush _normalBackground = BrushHelper.MakeSolidBrush(50, 50, 55);
    private static Brush _hoverBackground = BrushHelper.MakeSolidBrush(80, 80, 85);

    private FunctionNameTextBlock _nameText;

    public GraphNodePreview() 
    {
        Background = _normalBackground;
        CornerRadius = new CornerRadius(10);

        Child = _nameText = new FunctionNameTextBlock()
        {
            Text = "Create new {UEngine}",
            Margin = new Thickness(5),
            FontSize = 25
        };
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        Background = _hoverBackground;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        Background = _normalBackground;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {

    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {

    }
}