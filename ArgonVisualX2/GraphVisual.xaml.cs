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
    public static readonly DependencyProperty ScreenOffsetProperty = DependencyProperty.Register("ScreenOffset", typeof(Point), typeof(NodePanel), new FrameworkPropertyMetadata(new Point(0, 0), HandleScreenOffsetChanged));

    private static void HandleScreenOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is GraphVisual graphVisual)
        {
            graphVisual.HandleScreenOffsetChanged((Point)e.NewValue);
        }
    }

    private void HandleScreenOffsetChanged(Point newOffset) 
    {
        NodePanel.Global.NodesOffset = newOffset;
    }

    public Point ScreenOffset
    {
        get => (Point)GetValue(ScreenOffsetProperty);
        set => SetValue(ScreenOffsetProperty, value);
    }

    public List<NodeData> Nodes { get; }

    public GraphVisual()
    {
        Nodes = new List<NodeData>();

        Nodes.Add(new NodeData("This is a node!") { Position = new Point(200, 200) });
        Nodes.Add(new NodeData("Nodes are the future of programming!") { Position = new Point(300, 300) });
        
        InitializeComponent();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        Point mousePosition = Mouse.GetPosition(this);
        ScreenOffset = mousePosition;
    }
}
