using System.Windows.Controls;
using System.Windows;

namespace Argon.Widgets;

public class ArgonTextBlock : TextBlock
{
    public ArgonTextBlock()
    {
        Foreground = GlobalStyle.Text;
        FontSize = GlobalStyle.FontSizeNormal;
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Center;
    }

    public ArgonTextBlock(string text) : this()
    {
        Text = text;
    }
}