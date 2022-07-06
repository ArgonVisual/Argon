using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ArgonVisual.Widgets;

/// <summary>
/// Represents a text box that can be edited.
/// </summary>
public class EditableText : ContentControl
{
    public new double FontSize 
    {
        get => TextBox.FontSize;
        set => TextBox.FontSize = TextBlock.FontSize = value;
    }

    public new FontFamily FontFamily 
    {
        get => TextBox.FontFamily;
        set => TextBox.FontFamily = TextBlock.FontFamily = value;
    }

    public virtual string Text 
    {
        get => TextBox.Text;
        set => TextBox.Text = TextBlock.Text = value;
    }

    /// <summary>
    /// BeforeName - NewName
    /// </summary>
    public Action<string, string>? TextCommitted;

    protected TextBox TextBox;
    protected TextBlock TextBlock;

    private Action<string, InlineCollection>? _createInlinesFromText;

    /// <summary>
    /// Initializes a new intance of <see cref="EditableText"/>.
    /// </summary>
    public EditableText() 
    {
        TextBox = new TextBox()
        {
            Foreground = Brushes.Black,
            FontSize = 15
        };

        TextBox.LostFocus += HandleLostFocus;

        TextBlock = new TextBlock()
        {
            FontSize = 15
        };

        ExitEditMode();
    }

    protected EditableText(Action<string, InlineCollection>? createInlinesFromText) : this() 
    {
        _createInlinesFromText = createInlinesFromText;
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
        Content = TextBox;
    }

    protected override void OnContentChanged(object oldContent, object newContent)
    {
        if (newContent == TextBox)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate () {
                    TextBox.Focus();         // Set Logical Focus
                    Keyboard.Focus(TextBox); // Set Keyboard Focus
                    TextBox.SelectAll();
                }));
        }
    }

    /// <summary>
    /// Stops editing this text box.
    /// </summary>
    /// <returns>The text before the text was changed.</returns>
    public string ExitEditMode() 
    {
        string beforeName = TextBlock.Text;

        Text = TextBox.Text;

        if (_createInlinesFromText is not null)
        {
            TextBlock.Inlines.Clear();
            _createInlinesFromText(Text, TextBlock.Inlines);
        }
        else
        {
            TextBlock.Text = Text;
        }
        
        Content = TextBlock;
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