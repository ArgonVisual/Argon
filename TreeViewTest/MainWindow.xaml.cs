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

        SolutionTreeItem solution = new SolutionTreeItem("Solution");
        FolderTreeItem folder = new FolderTreeItem("Folder");
        folder.TreeItems.Add(new FileTreeItem("File"));
        folder.TreeItems.Add(new FileTreeItem("File"));
        solution.TreeItems.Add(folder);
        solution.TreeItems.Add(new FileTreeItem("File"));

        TreeItems.Add(solution);
    }
}

public class TreeItem 
{
    public string Name { get; set; }

    public List<TreeItem> TreeItems { get; }

    public bool IsExpanded { get; set; }

    public virtual string NodeType => "Base";

    public TreeItem(string name) 
    {
        Name = name;
        TreeItems = new List<TreeItem>();
        IsExpanded = true;
    }
}

public class FolderTreeItem : TreeItem
{
    public override string NodeType => "Folder";

    public FolderTreeItem(string name) : base(name)
    {
    }
}

public class FileTreeItem : TreeItem
{
    public override string NodeType => "File";

    public FileTreeItem(string name) : base(name)
    {
    }
}

public class SolutionTreeItem : TreeItem
{
    public override string NodeType => "Solution";

    public SolutionTreeItem(string name) : base(name)
    {
    }
}