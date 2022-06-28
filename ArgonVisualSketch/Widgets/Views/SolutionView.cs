using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ArgonVisual.Helpers;
using ArgonVisual.TreeItems;
using ArgonVisual.Widgets;

namespace ArgonVisual.Views;

public class SolutionView : ViewBase
{
    private TreeViewHeader _header;

    public SolutionView(SolutionEditor solutionEditor) : base(solutionEditor)
    {
        MinHeight = 250;

        _header = new TreeViewHeader(OpenSolutionOptions)
        {
            Title = Editor.Solution.Name,
            Icon = ArgonStyle.Icons.Solution
        };
    }

    private void OpenSolutionOptions()
    {
        throw new NotImplementedException();
    }

    protected override FrameworkElement GetBodyContent()
    {
        Grid mainGrid = new Grid();

        mainGrid.AddRowAuto(_header);

        TreeView treeView = new TreeView();

        IReadOnlyList<ArgonProject> projects = Editor.Solution.Projects;

        for (int i = 0; i < projects.Count; i++)
        {
            treeView.Items.Add(new ProjectTreeItem(projects[i], Editor));
        }

        ProjectTreeItem firstProject = (ProjectTreeItem)treeView.Items[0];
        Editor.FindView<ProjectView>()?.ShowProject(firstProject.Project);
        firstProject.IsSelected = true;

        mainGrid.AddRowFill(treeView);

        return mainGrid;
    }

    protected override string GetDefaultTitle()
    {
        return $"Solution";
    }
}