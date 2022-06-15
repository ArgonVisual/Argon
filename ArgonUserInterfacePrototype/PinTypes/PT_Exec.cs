using System.Windows.Media;

namespace ArgonUserInterfacePrototype.PinTypes;

public class PT_Exec : PinType
{
    public static PT_Exec Instance { get; } = new PT_Exec();

    protected PT_Exec() : base("Exec", Brushes.White)
    {

    }
}
