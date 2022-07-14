using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Text;
using System.Windows.Documents;

namespace GraphTest;

public class Node : Border
{
    public Point Position { get; set; }

    private TextBlock _nameText;
    private string _nodeText;

    public Node(Point position)
    {
        Position = position;

        // Define visual appearance for node

        Background = Brushes.Black;
        CornerRadius = new CornerRadius(10);

        _nodeText = "Get {Level} from {World}";

        Child = _nameText = new TextBlock()
        {
            Foreground = Brushes.White,
            FontSize = 25,
            Margin = new Thickness(5)
        };

        PopulateInlines();
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            Graph.Global.StartDraggingNode(this);
        }
    }

    public void PopulateInlines()
    {
        StringBuilder _nameBuilder = new StringBuilder();

        _nameText.Inlines.Clear();

        if (_nodeText.Length > 0)
        {
            bool isReadingParameter = false;

            for (int i = 0; i < _nodeText.Length; i++)
            {
                char character = _nodeText[i];
                if (character == '{' && !isReadingParameter)
                {
                    if (_nameBuilder.Length > 0)
                    {
                        _nameText.Inlines.Add(new Run(_nameBuilder.ToString())
                        {
                            BaselineAlignment = BaselineAlignment.Center
                        });
                        _nameBuilder.Clear();
                    }
                    isReadingParameter = true;
                }
                else if (isReadingParameter && character == '}')
                {
                    _nameText.Inlines.Add(new InlineUIContainer(new Parameter(_nameBuilder.ToString()))
                    {
                        BaselineAlignment = BaselineAlignment.Center
                    });
                    _nameBuilder.Clear();
                    isReadingParameter = false;
                }
                else
                {
                    _nameBuilder.Append(character);
                }
            }

            if (isReadingParameter)
            {
                _nameText.Inlines.Add(new InlineUIContainer(new Parameter(_nameBuilder.ToString()))
                {
                    BaselineAlignment = BaselineAlignment.Center
                });
            }
            else
            {
                _nameText.Inlines.Add(new Run(_nameBuilder.ToString())
                {
                    BaselineAlignment = BaselineAlignment.Center
                });
            }

            _nameBuilder.Clear();
        }
    }
}

public class Parameter : Border
{
    public Parameter(string name)
    {
        CornerRadius = new CornerRadius(5);
        Background = Brushes.DarkCyan;
        Child = new TextBlock()
        {
            Text = name,
            FontSize = 25,
            Foreground = Brushes.White,
            Margin = new Thickness(5, 0, 5, 0)
        };
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            Graph.Global.DraggedParameter = this;
            e.Handled = true;
        }
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {

    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        Background = Brushes.DarkSlateBlue;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        Background = Brushes.DarkCyan;
    }
}