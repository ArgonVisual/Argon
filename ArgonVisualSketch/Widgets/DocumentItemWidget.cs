using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ArgonVisual.Widgets;

public class DocumentItemWidget : Border
{
    private bool _isSelected;
    private static Brush _classbackground = BrushHelper.MakeSolidBrush(34, 185, 147);
    private static Brush _classSelectedbackground = BrushHelper.MakeSolidBrush(177, 255, 225);
    private static Brush _structBackground = BrushHelper.MakeSolidBrush(134, 193, 41);
    private static Brush _structSelectedBackground = BrushHelper.MakeSolidBrush(208, 250, 137);

    private TextBlock _nameText;

    public bool IsStruct;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (value)
            {
                Background = IsStruct ? _structSelectedBackground : _classSelectedbackground;
                _nameText.Foreground = Brushes.Black;
            }
            else
            {
                Background = IsStruct ? _structBackground : _classbackground;
                _nameText.Foreground = Brushes.White;
            }

            _isSelected = value;
        }
    }

    public DocumentItemWidget(string title, bool isStruct = false)
    {
        IsStruct = isStruct;

        Background = isStruct ? _structBackground : _classbackground;

        MinWidth = 120;
        MinHeight = 40;

        BorderBrush = null;

        BorderThickness = new Thickness(2);
        Margin = new Thickness(5);
        CornerRadius = new CornerRadius(10);

        Child = _nameText = new TextBlock()
        {
            Text = title,
            FontSize = 20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(15, 5, 15, 5),
            FontFamily = ArgonStyle.Fonts.Bold
        };
    }
}