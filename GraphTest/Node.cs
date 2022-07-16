﻿using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Text;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Linq;

namespace GraphTest;

public class Node : Border
{
    public IReadOnlyList<Parameter> Parameters => _parameters;
    private List<Parameter> _parameters;

    public Point Position { get; set; }

    private TextBlock _nameText;
    private string _nodeText;

    private static Brush _backgroundBrush;

    private bool _parentNodesNeedRefresh;

    private IEnumerable<Node>? _directParentNodes;
    public IEnumerable<Node> DirectParentNodes
    {
        get 
        {
            if (_directParentNodes == null || _parentNodesNeedRefresh)
            {
                _directParentNodes = GetDirectParentNodes();
            }

            return _directParentNodes;
        }
    }

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
            Margin = new Thickness(5, 2.5, 5, 2.5)
        };

        PopulateInlines();
    }

    // Only Node or Parameter should call this
    public void AddChildNodesInternal(List<Node> nodes)
    {
        foreach (Parameter parameter in Parameters)
        {
            parameter.AddChildNodes(nodes);
        }
    }

    public IEnumerable<Node> GetChildNodes()
    {
        List<Node> nodes = new List<Node>();

        AddChildNodesInternal(nodes);

        return nodes.Distinct();
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            Graph.Global.StartDraggingNode(this);
            IEnumerable<Node> childNodes = GetChildNodes();
            Graph.Global.NodesToDrag = childNodes;
        }
    }

    private IEnumerable<Node> GetDirectParentNodes()
    {
        List<Node> parentNodes = new List<Node>();

        foreach (Parameter parameter in Parameters)
        {
            if (parameter.ConnectedIn != null)
            {
                Node? parentNode = parameter.ConnectedIn.ParentNode;
                if (parentNode != null)
                {
                    parentNodes.Add(parentNode);
                }
            }
        }

        return parentNodes.Distinct();
    }

    public void PopulateInlines()
    {
        StringBuilder _nameBuilder = new StringBuilder();

        _nameText.Inlines.Clear();
        _parameters.Clear();
        _parentNodesNeedRefresh = true;

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