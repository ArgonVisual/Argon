using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ArgonVisual.Widgets;

public class TreeViewHeader : Border
{
    public string Title 
    {
        get => _nameText.Text;
        set => _nameText.Text = value;
    }

    public ImageSource Icon 
    {
        get => _iconImage.Source;
        set => _iconImage.Source = value;
    }

    private Image _iconImage;

    private TextBlock _nameText;

    private Action _openAction;

    private static Brush _normalBackground = BrushHelper.MakeSolidBrush(50, 50, 55);
    private static Brush _hoverBackground = BrushHelper.MakeSolidBrush(60, 60, 65);

    public TreeViewHeader(Action openAction) 
    {
        _openAction = openAction;

        _nameText = new TextBlock()
        {
            FontSize = 20,
            Margin = new Thickness(5)
        };

        StackPanel stackPanel = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(5, 0, 5, 0)
        };

        stackPanel.Children.Add(_iconImage = new Image()
        {
            Width = 25,
            Height = 25
        });

        stackPanel.Children.Add(_nameText);

        Background = _normalBackground;
        Margin = new Thickness(5);
        CornerRadius = new CornerRadius(5);
        Child = stackPanel;
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        Background = _hoverBackground;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        Background = _normalBackground;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            _openAction();
        }
    }
}