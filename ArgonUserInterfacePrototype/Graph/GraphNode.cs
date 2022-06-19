using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArgonUserInterfacePrototype.Graph;

public abstract class GraphNode : Grid
{
    public GraphPanel OuterGraph { get; }

    public GraphNode(GraphPanel graphPanel)
    {
        OuterGraph = graphPanel;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            OuterGraph.DraggedNode = this;
            OuterGraph.MouseOffsetInDraggedNode = Mouse.GetPosition(this);
            e.Handled = true;
        }
    }

    public virtual IEnumerable<GraphPin> GetOutputPins()
    {
        return Enumerable.Empty<GraphPin>();
    }

    public virtual IEnumerable<GraphPin> GetInputPins()
    {
        return Enumerable.Empty<GraphPin>();
    }
}