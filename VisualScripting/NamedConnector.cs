using System.Windows;
using System.Windows.Controls;

namespace RigidScripting;

public class NamedConnector : Border
{
    public NamedConnector(string title) 
    {
        CornerRadius = VisualStyle.CornerRadius;
        Background = VisualStyle.NodeBackground;

        Child = new TextBlock()
        {
            Text = title,
            Margin = new Thickness(15, 0, 15, 0),
            HorizontalAlignment = HorizontalAlignment.Center
        };
    }
}