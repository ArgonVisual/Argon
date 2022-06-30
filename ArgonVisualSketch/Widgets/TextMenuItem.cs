using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArgonVisual.Widgets;

/// <summary>
/// Represents a menu item that shows text and does something when it is clicked.
/// </summary>
public class TextMenuItem : MenuItem
{
    /// <summary>
    /// Initializes a new instance of <see cref="TextMenuItem"/>.
    /// </summary>
    /// <param name="name">The title to show.</param>
    /// <param name="clickAction">The action to call when this is clicked.</param>
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