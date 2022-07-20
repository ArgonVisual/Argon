using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScriptingDemo;

public class BranchNode : Node
{
    public NodeCollection FalseNodes { get; }

    public NodeCollection TrueNodes { get; }

    private NamedConnector _falseConnector;
    private NamedConnector _trueConnector;

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

        trueFalseGrid.AddColumnFill(_falseConnector = new NamedConnector("FALSE", VisualStyle.FalseBrush) { Margin = new Thickness(0, 0, 2.5, 0) });
        trueFalseGrid.AddColumnFill(_trueConnector = new NamedConnector("TRUE", VisualStyle.TrueBrush) { Margin = new Thickness(2.5, 0, 0, 0) });

        panel.Children.Add(trueFalseGrid);

        Content = panel;
    }

    public override Point GetConnectionPositionForNodeCollection(NodeCollection nodeCollection, Graph graph)
    {
        if (nodeCollection == TrueNodes)
        {
            return _trueConnector.GetCenterPositionRelativeTo(graph);
        }

        return _falseConnector.GetCenterPositionRelativeTo(graph);
    }

    public override void EnumerateDirectChildren(Action<Node> action)
    {
        foreach (Node falseNode in FalseNodes)
        {
            action(falseNode);
        }

        foreach (Node trueNode in TrueNodes)
        {
            action(trueNode);
        }
    }
}