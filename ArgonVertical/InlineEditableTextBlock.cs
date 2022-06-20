using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ArgonUserInterfacePrototype;
using ArgonVertical.Static;

namespace ArgonVertical;

public class InlineEditableTextBox : ContentControl
{
    public bool IsEditing { get; private set; }

    public bool IsEditable { get; set; }

    public string Text 
    {
        get => _textBox.Text;
        set => _textBlock.Text = _textBox.Text = value;
    }

    public Brush TextBrush 
    {
        set 
        {
            _textBox.Foreground = _textBlock.Foreground = value;
        }

        get 
        {
            return _textBox.Foreground;
        }
    }

    public void RefreshTextBlockInlines() 
    {
        _textBlock.Inlines.Clear();
        if (PopulateInlines is not null)
        {
            PopulateInlines(Text, _textBlock.Inlines);
        }
    }

    private ArgonTextBox _textBox;
    private ArgonTextBlock _textBlock;

    public Action<string, InlineCollection>? PopulateInlines;

    public InlineEditableTextBox(bool isEditable = true)
    {
        HorizontalAlignment = HorizontalAlignment.Left;
        VerticalAlignment = VerticalAlignment.Center;

        IsEditable = isEditable;

        _textBox = new ArgonTextBox();

        _textBlock = new ArgonTextBlock();

        MinWidth = 50;

        ExitEditMode();
    }

    public void EnterEditMode() 
    {
        if (IsEditable)
        {
            IsEditing = true;
            Content = _textBox;
            _textBox.IsReadOnly = false;
            _textBox.BorderThickness = new Thickness(1);
            _textBox.Focus();
        }
    }

    public void ExitEditMode() 
    {
        IsEditing = false;

        _textBlock.Inlines.Clear();
        if (PopulateInlines is not null)
        {
            _textBlock.Text = string.Empty;
            PopulateInlines.Invoke(_textBox.Text, _textBlock.Inlines);
        }
        else
        {
            _textBlock.Text = _textBox.Text;
        }
        Content = _textBlock;
        _textBox.IsReadOnly = true;
        _textBox.BorderThickness = new Thickness(0);
        _textBox.CaretIndex = 0;
    }

    protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
    {
        if (!IsEditing)
        {
            EnterEditMode();
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (IsEditable && e.Key == Key.Enter)
        {
            ExitEditMode();
        }
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        ExitEditMode();
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        if (IsEditable)
        {
            _textBlock.Background = _hoverBackground;
        }
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        if (IsEditable) 
        {
            _textBlock.Background = _normalBackground;
        }
    }

    private static Brush _normalBackground = Brushes.Transparent;
    private static Brush _hoverBackground;

    static InlineEditableTextBox() 
    {
        _hoverBackground = BrushHelper.MakeSolidBrush(0, 0, 0);
        _hoverBackground.Opacity = 0.1;
    }
}