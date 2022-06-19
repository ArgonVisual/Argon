using System.Windows;
using System.Windows.Media;

namespace ArgonUserInterfacePrototype.Graph;

public abstract class DataPin : GraphPin
{
    public PinType PinType { get; }

    public DataPin(GraphNode outerNode, string name, PinType pinType, PinDirection direction) : base(outerNode, direction, pinType.Brush)
    {
        PinType = pinType;

        ConnectorElement = CreateConnectorElement();
    }

    public sealed override Pen Pen => PinType.Pen;
}