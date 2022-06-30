using System.Windows;
using System.Windows.Controls;

namespace ArgonVisual;

/// <summary>
/// A textbox that displays a name above it.
/// </summary>
public class NameTextBox : StackPanel
{
    /// <summary>
    /// The name shown above the text box.
    /// </summary>
    public string BoxName
    {
        get => _nameText.Text;
        set => _nameText.Text = value;
    }

    /// <summary>
    /// The text inside of the textbox.
    /// </summary>
    public string Text
    {
        get => _textBox.Text;
        set => _textBox.Text = value;
    }

    private TextBox _textBox;

    private TextBlock _nameText;

    /// <summary>
    /// Initializes a new instance of <see cref="NameTextBox"/>.
    /// </summary>
    /// <param name="name">The name to show above.</param>
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