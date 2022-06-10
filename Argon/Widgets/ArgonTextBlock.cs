using System.Windows.Controls;

namespace Argon.Widgets;

public class ArgonTextBlock : TextBlock
{
    public ArgonTextBlock()
    {
        Foreground = GlobalStyle.Text;
        FontSize = 20;
    }

    public ArgonTextBlock(string text) : this()
    {
        Text = text;
    }
}