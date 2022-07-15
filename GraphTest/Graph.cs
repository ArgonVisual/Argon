using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphTest;

public class Graph : Border
{
    private static Graph? _global;

    public static Graph Global => _global ?? throw new InvalidOperationException("Graph has not been instanced");

    // Only nodes should be placed in this collection. If not then errors will happen!
    public UIElementCollection Nodes => _nodePanel.Children;

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

    public Graph() 
    {
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
            _draggedNode.Position = new Point((mousePosition.X - _mouseOffsetInNode.X) - _lastGraphOffset.X, (mousePosition.Y - _mouseOffsetInNode.Y) - _lastGraphOffset.Y);
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
        else
        {
            StopDraggingNode();
            DraggedParameter = null;
        }

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