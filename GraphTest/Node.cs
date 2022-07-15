using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Text;
using System.Collections.Generic;
using System.Windows.Documents;

namespace GraphTest;

public class Node : Border
{
    public IReadOnlyList<Parameter> Parameters => _parameters;
    private List<Parameter> _parameters;

    public Point Position { get; set; }

    private TextBlock _nameText;
    private string _nodeText;

    private static Brush _backgroundBrush;

    static Node() 
    {
        Color color = new Color()
        {
            R = 50,
            G = 50,
            B = 50,
            A = 255
        };

        _backgroundBrush = new SolidColorBrush(color);
    }

    public Node(Point position)
    {
        Position = position;
        _parameters = new List<Parameter>();

        // Define visual appearance for node

        Background = _backgroundBrush;
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
        _parameters.Clear();

        if (_nodeText.Length > 0)
        {
            bool isReadingParameter = false;

            void AddInline() 
            {
                if (isReadingParameter)
                {
                    Parameter parameter = new Parameter(_nameBuilder.ToString());
                    _nameText.Inlines.Add(new InlineUIContainer(parameter) { BaselineAlignment = BaselineAlignment.Center });
                    _parameters.Add(parameter);
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

            for (int i = 0; i < _nodeText.Length; i++)
            {
                char character = _nodeText[i];
                if (character == '{' && !isReadingParameter)
                {
                    if (_nameBuilder.Length > 0)
                    {
                        AddInline();
                    }
                    isReadingParameter = true;
                }
                else if (isReadingParameter && character == '}')
                {
                    AddInline();
                    isReadingParameter = false;
                }
                else
                {
                    _nameBuilder.Append(character);
                }
            }

            AddInline();
        }
    }
}