using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ArgonVisual.Widgets;

/// <summary>
/// Represents a text box that can be edited.
/// </summary>
public class SimpleInlineEditableTextBox : ContentControl
{
    public new double FontSize 
    {
        get => _textBox.FontSize;
        set => _textBox.FontSize = _textBlock.FontSize = value;
    }

    public new FontFamily FontFamily 
    {
        get => _textBox.FontFamily;
        set => _textBox.FontFamily = _textBlock.FontFamily = value;
    }

    public string Text 
    {
        get => _textBox.Text;
        set => _textBox.Text = _textBlock.Text = value;
    }

    /// <summary>
    /// BeforeName - NewName
    /// </summary>
    public Action<string, string>? TextCommitted;

    private TextBox _textBox;
    private TextBlock _textBlock;

    /// <summary>
    /// Initializes a new intance of <see cref="SimpleInlineEditableTextBox"/>.
    /// </summary>
    public SimpleInlineEditableTextBox() 
    {
        _textBox = new TextBox()
        {
            Foreground = Brushes.Black,
            FontSize = 15
        };

        _textBox.LostFocus += HandleLostFocus;

        _textBlock = new TextBlock()
        {
            FontSize = 15
        };

        ExitEditMode();
    }

    private void HandleLostFocus(object sender, RoutedEventArgs e)
    {
        CommitText();
    }

    /// <summary>
    /// Puts this textbox into edit mode.
    /// </summary>
    public void EnterEditMode()
    {
        _textBox.Text = _textBlock.Text;
        Content = _textBox;
    }

    protected override void OnContentChanged(object oldContent, object newContent)
    {
        if (newContent == _textBox)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate () {
                    _textBox.Focus();         // Set Logical Focus
                    Keyboard.Focus(_textBox); // Set Keyboard Focus
                    _textBox.SelectAll();
                }));
        }
    }

    /// <summary>
    /// Stops editing this text box.
    /// </summary>
    /// <returns>The text before the text was changed.</returns>
    public string ExitEditMode() 
    {
        string beforeName = _textBlock.Text;
        _textBlock.Text = _textBox.Text;
        Content = _textBlock;
        return beforeName;
    }

    /// <summary>
    /// Commits text
    /// </summary>
    public void CommitText()
    {
        string beforeName = ExitEditMode();
        TextCommitted?.Invoke(beforeName, Text);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            CommitText();
        }
    }
}