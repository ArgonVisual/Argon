using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ArgonVisual;

/// <summary>
/// A button that shows text.
/// </summary>
public class TextButton : Button
{
    /// <summary>
    /// The text that is being shown in the button.
    /// </summary>
    public string Text 
    {
        get => _textBlock.Text;
        set => _textBlock.Text = value;
    }

    private TextBlock _textBlock;

    /// <summary>
    /// Initializes a new instance of <see cref="TextButton"/> with the text to show.
    /// </summary>
    /// <param name="text">The text to show in the button.</param>
    public TextButton(string text) 
    {
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Center;

        Padding = new Thickness(20, 5, 20, 5);

        Content = _textBlock = new TextBlock() 
        {
            Foreground = Brushes.Black,
            Text = text,
            FontSize = 30
        };
    }
}