using System.Windows.Media;

namespace ArgonUserInterfacePrototype.PinTypes;

public class PT_Integer : PinType
{
    public static PT_Integer Instance { get; } = new PT_Integer();

    protected PT_Integer() : base("Integer", Brushes.Green)
    {

    }
}
