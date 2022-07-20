using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ScriptingDemo;

public class BranchNode : Node
{
    public NodeCollection FalseNodes { get; }

    public NodeCollection TrueNodes { get; }

    public BranchNode() 
    {
        FalseNodes = new NodeCollection(this);
        TrueNodes = new NodeCollection(this);

        StackPanel panel = new StackPanel();

        panel.Children.Add(new Border() 
        {
            Background = VisualStyle.NodeBackground,
            CornerRadius = VisualStyle.CornerRadius,
            Child = new TextBlock() 
            {
               Text = "Is 40 > 20?",
               TextAlignment = TextAlignment.Center,
               HorizontalAlignment = HorizontalAlignment.Center,
               VerticalAlignment = VerticalAlignment.Center,
               Margin = new Thickness(8, 4, 8, 0),
            }
        });

        Grid trueFalseGrid = new Grid() { Margin = new Thickness(0, 5, 0, 0) };

        trueFalseGrid.AddColumnFill(new NamedConnector("FALSE", VisualStyle.FalseBrush) { Margin = new Thickness(0, 0, 2.5, 0) });
        trueFalseGrid.AddColumnFill(new NamedConnector("TRUE", VisualStyle.TrueBrush) { Margin = new Thickness(2.5, 0, 0, 0) });

        panel.Children.Add(trueFalseGrid);

        Content = panel;
    }

    public override IEnumerable<Node> GetDirectChildNodes()
    {
        foreach (Node falseNode in FalseNodes)
        {
            yield return falseNode;
        }

        foreach (Node trueNode in TrueNodes)
        {
            yield return trueNode;
        }
    }
}