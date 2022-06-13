using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Argon.Widgets;

public class InlineEditableTextBox : ContentControl
{
    public string Text 
    {
        get => _textBox.Text;
        set => _textBlock.Text = _textBox.Text = value;
    }

    public bool IsEditing => Content == _textBox;

    public Brush TextBoxBackground 
    {
        get => _textBox.Background;
        set => _textBox.Background = value;
    }

    private ArgonTextBlock _textBlock;
    private ArgonTextBox _textBox;

    public InlineEditableTextBox() 
    {
        _textBlock = new ArgonTextBlock();
        _textBox = new ArgonTextBox();

        Content = _textBlock;
    }

    public void EnterEditMode() 
    {
        _textBox.Text = _textBlock.Text;
        Content = _textBox;
        _textBox.Focus();
        _textBox.SelectAll();
    }

    public void ExitEditMode() 
    {
        _textBlock.Text = _textBox.Text;
        Content = _textBlock;
    }

    public void BindTextChanged(TextChangedEventHandler textChanged) 
    {
        _textBox.TextChanged += textChanged;
    }

    public void UnbindTextChanged(TextChangedEventHandler textChanged)
    {
        _textBox.TextChanged -= textChanged;
    }
}