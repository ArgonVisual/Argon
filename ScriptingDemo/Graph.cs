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
        BranchNode branchNode = new BranchNode();

        branchNode.TrueNodes.Add(new BranchNode());
        rootNode.Nodes.Add(branchNode);

        NodePanel.Children.Add(rootNode);
        RootNode = rootNode;
        Child = grid;
    }
}