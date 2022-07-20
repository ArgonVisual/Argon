using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphTest;

public class Graph : Border
{
    private static Graph? _global;

    public static Graph Global => _global ?? throw new InvalidOperationException("Graph has not been instanced");

    public Node? SelectedNode { get; private set; }

    // Only Node should call this. To actually select a node, call Node.Select();
    public void SelectNode(Node nodeToSelect) 
    {
        if (SelectedNode != null)
        {
            SelectedNode.Deselect();
        }

        SelectedNode = nodeToSelect;
        ((MainWindow)Application.Current.MainWindow).ShowNodeOptions(nodeToSelect);
    }

    // Only nodes should be placed in this collection. If not then errors will happen!
    public UIElementCollection Nodes => _nodePanel.Children;

    public IEnumerable<Node> NodesToDrag { get; set; }

    private NodePanel _nodePanel;
    private ConnectionRenderer _connectionRenderer;
    private Grid _grid;

    public Parameter? HoveredParameter { get; set; }
    public Parameter? DraggedParameter { get; set; }

    private Node? _draggedNode;
    private Point _mouseOffsetInNode;

    private bool _isPanning;
    private Point _mouseStart;
    private Point _lastGraphOffset;
    private Point _currentGraphOffset;

    private Point? _lastNodeDragMousePosition;

    public Graph() 
    {
        ClipToBounds = true;

        _global = this;

        Background = Brushes.Gray;

        _grid = new Grid();

        _grid.Children.Add(_connectionRenderer = new ConnectionRenderer(this));
        _grid.Children.Add(_nodePanel = new NodePanel());

        Child = _grid;
    }

    public void StartDraggingNode(Node node) 
    {
        if (!_isPanning)
        {
            _draggedNode = node;
            _mouseOffsetInNode = Mouse.GetPosition(_draggedNode);
        }
    }

    public void StopDraggingNode() 
    {
        if (_draggedNode is not null)
        {
            _draggedNode = null;
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_isPanning)
        {
            Point mousePosition = Mouse.GetPosition(this);
            _nodePanel.ScreenOffset = _currentGraphOffset = new Point(mousePosition.X - _mouseStart.X + _lastGraphOffset.X, mousePosition.Y - _mouseStart.Y + _lastGraphOffset.Y);
            InvalidateGraph();
        }
        else if (_draggedNode is not null)
        {
            Point mousePosition = Mouse.GetPosition(this);

            if (_lastNodeDragMousePosition == null)
            {
                _lastNodeDragMousePosition = mousePosition;
            }

            if (_lastNodeDragMousePosition is Point validPoint)
            {
                Point draggedNodePosition = _draggedNode.TransformToAncestor(this).Transform(new Point(0, 0));

                bool isTooHigh = _draggedNode.DirectParentNodes.Any((node) =>
                {
                    Point nodePosition = node.TransformToAncestor(this).Transform(new Point(0, 0));
                    return mousePosition.Y < nodePosition.Y + (node.ActualHeight / 2) + node.ActualHeight + 30;
                });

                foreach (var node in NodesToDrag)
                {
                    Vector relativeOffset = Point.Subtract(mousePosition, validPoint);
                    node.Position = new Point(node.Position.X + relativeOffset.X, node.Position.Y + (isTooHigh ? 0 : relativeOffset.Y));
                }
            }

            _lastNodeDragMousePosition = mousePosition;
            InvalidateGraph();
        }
        else if (DraggedParameter != null)
        {
            _connectionRenderer.InvalidateVisual();
        }
    }

    public void InvalidateGraph() 
    {
        _nodePanel.InvalidateArrange();
        _connectionRenderer.InvalidateVisual();
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right)
        {
            _mouseStart = Mouse.GetPosition(this);
            _isPanning = true;
        }
        else if (e.ChangedButton == MouseButton.Middle)
        {
            Point mousePosition = Mouse.GetPosition(this);
            Node node = new Node(new Point(mousePosition.X - _lastGraphOffset.X, mousePosition.Y - _lastGraphOffset.Y));
            Nodes.Add(node);
            _connectionRenderer.InvalidateVisual();
        }
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right)
        {
            _isPanning = false;
            _lastGraphOffset = _currentGraphOffset;
        }
        else if (DraggedParameter != null)
        {
            DraggedParameter.StopDragging();
            DraggedParameter = null;
        }
        else
        {
            StopDraggingNode();
        }

        _lastNodeDragMousePosition = null;

        _connectionRenderer.InvalidateVisual();
    }

    public IEnumerable<Parameter> GetAllParameters() 
    {
        for (int n = 0; n < Nodes.Count; n++)
        {
            IReadOnlyList<Parameter> parameters = ((Node)Nodes[n]).Parameters;
            for (int p = 0; p < parameters.Count; p++)
            {
                yield return parameters[p];
            }
        }
    }
}