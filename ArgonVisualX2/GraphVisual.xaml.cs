using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArgonVisualX2;
/// <summary>
/// Interaction logic for GraphVisual.xaml
/// </summary>
public partial class GraphVisual : UserControl
{
    private bool _isPanning;
    private Point _mouseStartOffset;

    private Point _lastScreenOffset;

    private Point _screenOffset;

    public Point ScreenOffset
    {
        get => _screenOffset;
        set 
        {
            _screenOffset = value;

            NodePanel.Global.NodesOffset = value;
            NodePanel.Global.InvalidateVisual();
        }
    }

    public List<NodeData> Nodes { get; }

    public GraphVisual()
    {
        Nodes = new List<NodeData>();

        Nodes.Add(new NodeData("Get {Object} from {Array}") { Position = new Point(100, 100) });
        Nodes.Add(new NodeData("Normalize {String}") { Position = new Point(200, 200) });
        
        InitializeComponent();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_isPanning)
        {
            Point mousePosition = Mouse.GetPosition(this);
            ScreenOffset = new Point(mousePosition.X - _mouseStartOffset.X + _lastScreenOffset.X, mousePosition.Y - _mouseStartOffset.Y + _lastScreenOffset.Y);
        }
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right)
        {
            _mouseStartOffset = Mouse.GetPosition(this);
            _isPanning = true;
        }
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right)
        {
            _isPanning = false;
            _lastScreenOffset = ScreenOffset;
        }
    }
}
