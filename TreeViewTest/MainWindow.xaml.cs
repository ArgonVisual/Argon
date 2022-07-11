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

namespace TreeViewTest;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public List<TreeItem> TreeItems { get; }

    public MainWindow()
    {
        InitializeComponent();

        TreeItems = new List<TreeItem>();

        TreeItem solution = new TreeItem("Solution");
        TreeItem folder = new TreeItem("Folder");
        folder.TreeItems.Add(new TreeItem("Project"));
        folder.TreeItems.Add(new TreeItem("Project"));
        solution.TreeItems.Add(folder);
        solution.TreeItems.Add(new TreeItem("Project"));

        TreeItems.Add(solution);
    }
}

public class TreeItem 
{
    public string Name { get; set; }

    public List<TreeItem> TreeItems { get; }

    public bool IsExpanded { get; set; }

    public TreeItem(string name) 
    {
        Name = name;
        TreeItems = new List<TreeItem>();
        IsExpanded = true;
    }
}