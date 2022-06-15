using System.Windows.Media;

namespace ArgonUserInterfacePrototype.PinTypes;

public class PT_Bool : PinType
{
    public static PT_Bool Instance { get; } = new PT_Bool();

    protected PT_Bool() : base("Bool", Brushes.Red)
    {

    }
}