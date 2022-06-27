using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ArgonVisual;

public class FunctionPreview : Border
{
    private bool _isSelected;

    public bool IsSelected 
    {
        get => _isSelected;
        set
        {
            Background = value ? _selectedBackground : _normalBackground;
            _isSelected = value;
        }
    }

    private static Brush _normalBackground = BrushHelper.MakeSolidBrush(40, 40, 45);
    private static Brush _selectedBackground = BrushHelper.MakeSolidBrush(60, 60, 65);

    public FunctionPreview() 
    {
        Background = _normalBackground;
        HorizontalAlignment = HorizontalAlignment.Left;

        Margin = new Thickness(15, 16, 6, 0);

        CornerRadius = new CornerRadius(12);

        StackPanel mainPanel = new StackPanel();

        StackPanel topPanel = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(-10, -10, -10, 0)
        };

        topPanel.Children.Add(new Ellipse() 
        {
            Fill = BrushHelper.MakeSolidBrush(72, 162, 62),
            Width = 15,
            Height = 15,
            Margin = new Thickness(3, -5, 3, 0)
        });

        topPanel.Children.Add(new FunctionModifier("Static", BrushHelper.MakeSolidBrush(75, 145, 200)));

        topPanel.Children.Add(new FunctionModifier("Inlined", BrushHelper.MakeSolidBrush(129, 74, 170)));

        topPanel.Children.Add(new FunctionModifier("My Category", BrushHelper.MakeSolidBrush(63, 65, 153)));

        mainPanel.Children.Add(topPanel);

        mainPanel.Children.Add(new TextBlock()
        {
            Text = "This is my function",
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            FontSize = 22,
            Margin = new Thickness(8, -2, 8, 6)
        });

        Child = mainPanel;
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        Background = _selectedBackground;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        if (!_isSelected)
        {
            Background = _normalBackground;
        }
    }
}