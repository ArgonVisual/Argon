using System.Windows.Media;

namespace ArgonUserInterfacePrototype;

public abstract class PinType 
{
    public string Name { get; }
    public Brush Brush { get; }
    public Pen Pen { get; }

    protected PinType(string name, Brush brush) 
    {
        Name = name;
        Brush = brush;
        Pen = new Pen(brush, 16)
        {
            EndLineCap = PenLineCap.Round,
            StartLineCap = PenLineCap.Round,
        };
    }
}
