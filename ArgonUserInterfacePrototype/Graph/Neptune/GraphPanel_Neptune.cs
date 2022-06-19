namespace ArgonUserInterfacePrototype.Graph.Neptune;

public class GraphPanel_Neptune : GraphPanel
{
    public GraphPanel_Neptune() 
    {
        BranchNode_Neptune branchNode = new BranchNode_Neptune(this);
        AddNode(branchNode);
        SetNodeScreenPosition(branchNode, 200, 200);
    }
}

public class BranchNode_Neptune : GraphNode
{
    public BranchNode_Neptune(GraphPanel graphPanel) : base(graphPanel)
    {

    }
}