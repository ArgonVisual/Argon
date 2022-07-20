using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScriptingDemo;

public class NamedConnector : Border
{
    public string Title 
    {
        get => _titleText.Text;
        set => _titleText.Text = value;
    }

    private TextBlock _titleText;

    public NamedConnector(string title, Brush? background = null) 
    {
        MinWidth = 80;
        CornerRadius = VisualStyle.CornerRadius;
        Background = background ?? VisualStyle.NodeBackground;

        HorizontalAlignment = HorizontalAlignment.Center;

        Child = _titleText = new TextBlock()
        {
            Text = title,
            FontWeight = FontWeights.Medium,
            Margin = new Thickness(10, 1, 10, -2),
            HorizontalAlignment = HorizontalAlignment.Center,
            FontSize = 20
        };
    }
}