namespace ArgonUserInterfacePrototype.Graph.Mars;

public class GraphPanel_Mars : GraphPanel
{
    public GraphPanel_Mars() 
    {
        GraphNode node1 = new FunctionNode_Mars(this);
        SetNodeScreenPosition(node1, 190, 205);
        AddNode(node1);

        GraphNode node2 = new FunctionNode_Mars(this);
        SetNodeScreenPosition(node2, 800, 220);
        AddNode(node2);
    }
}