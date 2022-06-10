using System.Windows.Controls;
using System.Windows;

namespace Argon.Widgets;

public class ArgTextBlock : TextBlock
{
    public ArgTextBlock()
    {
        Foreground = GlobalStyle.Text;
        FontSize = GlobalStyle.FontSizeNormal;
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Center;
    }

    public ArgTextBlock(string text) : this()
    {
        Text = text;
    }
}