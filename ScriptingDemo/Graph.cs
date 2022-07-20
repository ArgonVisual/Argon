using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScriptingDemo;

public class Graph : Border
{
    private class ConnectionRenderer : FrameworkElement
    {
        public ConnectionRenderer() 
        {
            
        }

        protected override void OnRender(DrawingContext dc)
        {

        }
    }

    public Node RootNode { get; }

    public NodePanel NodePanel { get; }
    private ConnectionRenderer _connectionRenderer;

    public Graph() 
    {
        Background = VisualStyle.GraphBackground;

        Grid grid = new Grid();

        grid.Children.Add(_connectionRenderer = new ConnectionRenderer());
        grid.Children.Add(NodePanel = new NodePanel());

        RootNode rootNode = new RootNode(this) { Position = new Point(200, 200) };
        BranchNode branchNode = new BranchNode() { Position = new Point(150, 300) };

        // Nodes must always be added to their parent before their children can be added.
        // If this rule is not followed, then the code will not work.

        rootNode.Nodes.Add(branchNode);
        branchNode.TrueNodes.Add(new BranchNode() { Position = new Point(200, 400) });


        NodePanel.Children.Add(rootNode);
        RootNode = rootNode;
        Child = grid;
    }
}