using System.Windows;
using System.Windows.Controls;
using ArgonVisual.TreeItems;

namespace ArgonVisual.Views;

public class ProjectView : ViewBase
{
    public ProjectView() 
    {
        
    }

    protected override FrameworkElement GetBodyContent()
    {
        TreeView treeView = new TreeView();

        ProjectFolderTreeItem projecyFolder = new ProjectFolderTreeItem("MyFolder");
        projecyFolder.Items.Add(new CodeFileTreeItem("MyFile") { IsSelected = true });
        projecyFolder.Items.Add(new CodeFileTreeItem("MyOtherFile"));

        treeView.Items.Add(projecyFolder);
        treeView.Items.Add(new CodeFileTreeItem("MySpecialFile"));

        return treeView;
    }

    protected override string Getitle()
    {
        return "MyProject";
    }
}