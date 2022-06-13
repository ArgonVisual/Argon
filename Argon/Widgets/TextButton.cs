using System.Windows.Controls;
using System.Windows;

namespace Argon.Widgets;

/// <summary>
/// A button that displays text
/// </summary>
public class TextButton : ArgonButton
{
    /// <summary>
    /// Initializes a new instance of <see cref="TextButton"/> with the <paramref name="text"/> to show.
    /// </summary>
    /// <param name="text">The text to show in the button</param>
    public TextButton(string text)
    {
        Child = new ArgonTextBlock()
        {
            Text = text,
            FontSize = 30,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
    }
}