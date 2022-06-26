using System.Windows;
using System.Windows.Controls;

namespace ArgonVisualSketch.Views;

public class FunctionsView : ViewBase
{
    public FunctionsView() 
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
        return "Functions";
    }
}