using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArgonVisual.Widgets;

public class TextMenuItem : MenuItem
{
    public TextMenuItem(string name, Action clickAction)
    {
        Header = new TextBlock()
        {
            Text = name,
            Foreground = Brushes.Black,
            FontSize = 15
        };

        Click += (sender, args) => clickAction();
    }
}