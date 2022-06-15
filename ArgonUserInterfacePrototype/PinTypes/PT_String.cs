using System.Windows.Media;

namespace ArgonUserInterfacePrototype.PinTypes;

public class PT_String : PinType
{
    public static PT_String Instance { get; } = new PT_String();

    protected PT_String() : base("String", Brushes.Orange)
    {
        
    }
}
