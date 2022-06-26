using System.Windows;
using System.Windows.Controls;
using ArgonVisualSketch.TreeItems;

namespace ArgonVisualSketch.Views;

public class SolutionView : ViewBase
{
    public SolutionView() 
    {
        MinHeight = 250;
    }

    protected override FrameworkElement GetBodyContent()
    {
        TreeView treeView = new TreeView();

        SolutionFolderTreeItem solutionFolder = new SolutionFolderTreeItem("MyFolder");
        solutionFolder.Items.Add(new ProjectTreeItem("MyOtherProject"));
        solutionFolder.Items.Add(new ProjectTreeItem("MySpecialProject"));

        treeView.Items.Add(solutionFolder);
        treeView.Items.Add(new ProjectTreeItem("MyProject"));

        return treeView;
    }

    protected override string Getitle()
    {
        return "MySolution";
    }
}