using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static RigidScripting.WidgetHelper;

namespace RigidScripting;

public class BranchNode : Node
{
    public NodePanel FalseNodePanel => _falseContainer.NodePanel;
    public NodePanel TrueNodePanel => _trueContainer.NodePanel;

    private BorderedNodeContainer _falseContainer;
    private BorderedNodeContainer _trueContainer;

    private Border _titleBorder;

    private class BorderedNodeContainer : Border
    {
        public NodePanel NodePanel { get; }

        public BorderedNodeContainer(Brush background) 
        {
            BorderBrush = background;
            BorderThickness = VisualStyle.Thickness;
            Background = null;
            CornerRadius = VisualStyle.CornerRadius;
            MinWidth = 50;
            MinHeight = 50;
            VerticalAlignment = VerticalAlignment.Top;

            Child = NodePanel = new NodePanel();
        }
    }

    public BranchNode() 
    {
        StackPanel panel = new StackPanel();

        _titleBorder = new Border() 
        {
            CornerRadius = new CornerRadius(VisualStyle.CornerRadiusAmount, VisualStyle.CornerRadiusAmount, 0, 0),
            Background = VisualStyle.NodeBackground,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(VisualStyle.CornerRadiusAmount, 0, VisualStyle.CornerRadiusAmount, 0),
            Child = new TextBlock() 
            {
                Text = "Is 40 > 20?",
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5, 2, 5, 2)
            }
        };

        panel.Children.Add(_titleBorder);

        Grid trueAndFalseGrid = new Grid() { HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(VisualStyle.ThicknessAmount + 2) };

        trueAndFalseGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

        trueAndFalseGrid.AddColumnAuto(_falseContainer = new BorderedNodeContainer(VisualStyle.FalseBrush) { Margin = new Thickness(0, 0, 4, 0) });
        trueAndFalseGrid.AddColumnAuto(_trueContainer = new BorderedNodeContainer(VisualStyle.TrueBrush) { Margin = new Thickness(4, 0, 0, 0) });

        panel.Children.Add(new Border() 
        {
            BorderBrush = VisualStyle.NodeBackground,
            BorderThickness = VisualStyle.ThicknessSmall,
            CornerRadius = VisualStyle.CornerRadius,
            Child = trueAndFalseGrid
        });

        Content = panel;
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        _titleBorder.Background = VisualStyle.NodeHoveredBackground;
        e.Handled = true;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        _titleBorder.Background = VisualStyle.NodeBackground;
        e.Handled = true;
    }
}