using System;
using System.Windows;
using System.Windows.Controls;
using ArgonVisual.Helpers;
using ArgonVisual.TreeItems;
using ArgonVisual.Widgets;

namespace ArgonVisual.Views;

public class ProjectView : ViewBase
{
    private TreeViewHeader _header;

    public ArgonProject? ShownProject { get; private set; }

    public ProjectView(SolutionEditor solutionEditor) : base(solutionEditor)
    {
        _header = new TreeViewHeader(ShowProjectOptions)
        {
            Icon = ArgonStyle.Icons.Project
        };
    }

    private void ShowProjectOptions()
    {
        if (ShownProject is not null)
        {
            ProjectOptions.Show(ShownProject);
        }
    }

    protected override FrameworkElement GetBodyContent()
    {
        Grid mainGrid = new Grid();

        TreeView treeView = new TreeView();

        ProjectFolderTreeItem projectFolder = new ProjectFolderTreeItem("MyFolder", Editor);
        projectFolder.Items.Add(new CodeFileTreeItem("MyFile", Editor) { IsSelected = true });
        projectFolder.Items.Add(new CodeFileTreeItem("MyOtherFile", Editor));

        treeView.Items.Add(projectFolder);
        treeView.Items.Add(new CodeFileTreeItem("MySpecialFile", Editor));

        mainGrid.AddRowAuto(_header);

        mainGrid.AddRowFill(treeView);

        return mainGrid;
    }

    protected override string GetDefaultTitle()
    {
        return "Project";
    }

    public void ShowProject(ArgonProject project)
    {
        ShownProject = project;
        _header.Title = project.Name;
    }
}