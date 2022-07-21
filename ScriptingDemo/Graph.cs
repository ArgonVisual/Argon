using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ScriptingDemo;

public class Graph : Border
{
    public Node RootNode { get; }

    public NodePanel NodePanel { get; }
    public ConnectionRenderer ConnectionRenderer { get; }

    public Graph() 
    {
        Background = VisualStyle.GraphBackground;

        Grid grid = new Grid();

        grid.Children.Add(ConnectionRenderer = new ConnectionRenderer(this));
        grid.Children.Add(NodePanel = new NodePanel(this));

        RootNode rootNode = new RootNode(this);
        BranchNode topBranchNode = new BranchNode();

        BranchNode rightBranchNode = new BranchNode();
        rightBranchNode.FalseNodes.Add(new BranchNode());
        rightBranchNode.TrueNodes.Add(new BranchNode());

        BranchNode leftBranchNode = new BranchNode();
        leftBranchNode.TrueNodes.Add(new BranchNode());

        topBranchNode.TrueNodes.Add(rightBranchNode);
        topBranchNode.FalseNodes.Add(leftBranchNode);

        rootNode.Nodes.Add(topBranchNode);

        NodePanel.Children.Add(rootNode);
        RootNode = rootNode;
        Child = grid;
    }
}