using System.Windows;
using System.Windows.Controls;

namespace ArgonVisual;

public class NameTextBox : StackPanel
{
    public string BoxName
    {
        get => _nameText.Text;
        set => _nameText.Text = value;
    }

    public string Text
    {
        get => _textBox.Text;
        set => _textBox.Text = value;
    }

    private TextBox _textBox;

    private TextBlock _nameText;

    public NameTextBox(string name)
    {
        Margin = new Thickness(10);

        _textBox = new TextBox()
        {
            FontSize = 20
        };

        _nameText = new TextBlock()
        {
            Text = name,
            FontSize = 15
        };

        Children.Add(_nameText);
        Children.Add(_textBox);
    }
}