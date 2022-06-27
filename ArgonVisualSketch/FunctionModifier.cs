using System.Windows.Controls;
using System.Windows;
using ArgonVisual;
using System.Windows.Media;

namespace ArgonVisual;

public class FunctionModifier : Border
{
    public FunctionModifier(string name, Brush fill) 
    {
        Background = fill;
        CornerRadius = new CornerRadius(6);
        HorizontalAlignment = HorizontalAlignment.Left;
        VerticalAlignment = VerticalAlignment.Center;
        Margin = new Thickness(3);
        Child = new TextBlock()
        {
            Text = name,
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Center,
            FontSize = 15,
            Margin = new Thickness(5, 2, 5, 2)
        };
    }
}