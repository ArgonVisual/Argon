using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Argon.Widgets;

/// <summary>
/// Argon Button
/// </summary>
public class ArgButton : Border
{
    /// <summary>
    /// Gets called when the button is clicked
    /// </summary>
    public Action? Click { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="ArgButton"/>.
    /// </summary>
    public ArgButton() 
    {
        BorderBrush = null;
        BorderThickness = new Thickness();
        Background = GlobalStyle.ButtonNormal;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        Click?.Invoke();
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        Background = GlobalStyle.ButtonHover;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        Background = GlobalStyle.ButtonNormal;
    }
}