using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ArgonVisual;

public class TextButton : Button
{
    public string Text 
    {
        get => _textBlock.Text;
        set => _textBlock.Text = value;
    }

    private TextBlock _textBlock;

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