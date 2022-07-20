using System.Windows;
using System.Windows.Controls;

namespace RigidScripting;

public class Graph : Border
{
    private NodePanel _nodePanel;

    public Graph()
    {
        // Initialize visual style
        Background = VisualStyle.GraphBackground;

        // Populate content
        _nodePanel = new NodePanel();

        BranchNode branchNode = new BranchNode();

        branchNode.FalseNodePanel.Children.Add(new BranchNode());

        _nodePanel.Children.Add(branchNode);

        Child = _nodePanel;
    }
}