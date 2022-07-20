using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

namespace GraphTest;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public List<ParameterData> ParameterItems { get; set; }

    private Node? _selectedNode;

    public MainWindow()
    {
        InitializeComponent();

        MainGraph.Nodes.Add(new Node(new Point(100, 100)));
        MainGraph.Nodes.Add(new Node(new Point(200, 200)));

        ParameterListView.ItemsSource = ParameterItems = new List<ParameterData>();
    }

    public void ShowNodeOptions(Node node) 
    {
        _selectedNode = node;

        NodeNameText.IsEnabled = true;
        NodeNameText.Text = node.NodeText;
        RefreshNodeParameters();

        WarningText.Visibility = _selectedNode.HasConnection ? Visibility.Visible : Visibility.Collapsed;
    }

    private void RefreshNodeParameters() 
    {
        ParameterItems.Clear();

        if (_selectedNode != null)
        {
            foreach (Parameter parameter in _selectedNode.Parameters)
            {
                ParameterItems.Add(new ParameterData()
                {
                    Name = parameter.ParameterName
                });
            }
        }

        ICollectionView view = CollectionViewSource.GetDefaultView(ParameterListView.ItemsSource);
        view.Refresh();
    }

    private void NodeNameText_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_selectedNode != null)
        {
            if (!_selectedNode.HasConnection)
            {
                _selectedNode.SetNodeTitle(NodeNameText.Text);
                RefreshNodeParameters();

                WarningText.Visibility = Visibility.Collapsed;
            }
            else
            {
                WarningText.Visibility = Visibility.Visible;
                NodeNameText.Text = _selectedNode.NodeText;
            }
        }
    }
}

public class ParameterData
{
    public string Name { get; set; }

    public ParameterType Type { get; set; }

    public ParameterData() 
    {
        Name = string.Empty;
    }
}

public enum ParameterType 
{
    InAndOut,
    OnlyOut,
}