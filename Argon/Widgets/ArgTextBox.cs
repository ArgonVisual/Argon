using System.Windows;
using System.Windows.Controls;

namespace Argon.Widgets;

/// <summary>
/// A textbox that uses Argon style
/// </summary>
public class ArgTextBox : TextBox
{
    public ArgTextBox() 
    {
        Background = GlobalStyle.BackgroundDark;
        BorderBrush = null;
        BorderThickness = new Thickness();
        FontSize = GlobalStyle.FontSizeNormal;

        Foreground = GlobalStyle.Text;
        CaretBrush = GlobalStyle.Text;

        Padding = new Thickness(3);
    }
}