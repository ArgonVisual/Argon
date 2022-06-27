using System.Windows;
using System.Windows.Controls;

namespace ArgonVisual.Views;

public class PropertiesView : ViewBase
{
    public PropertiesView() 
    {
        
    }

    protected override FrameworkElement GetBodyContent()
    {
        return new TextBlock()
        {
            Text = "Tab Body!"
        };
    }

    protected override string Getitle()
    {
        return "Properties";
    }
}