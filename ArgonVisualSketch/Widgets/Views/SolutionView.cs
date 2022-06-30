using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ArgonVisual.Helpers;
using ArgonVisual.TreeItems;
using ArgonVisual.Widgets;

namespace ArgonVisual.Views;

/// <summary>
/// Shows all projects and solution folders in a solution.
/// The selected project gets shown in <see cref="ProjectView"/>.
/// </summary>
public class SolutionView : ViewBase
{
    private TreeViewHeader _header;
    private TreeView _treeView;

    public SolutionView(SolutionEditor solutionEditor) : base(solutionEditor)
    {
        MinHeight = 250;

        _treeView = new TreeView();

        ContextMenu contextMenu = new ContextMenu();
        contextMenu.Items.Add(new TextMenuItem("Add New Project", AddNewProject));

        _header = new TreeViewHeader(ShowSolutionOptions)
        {
            Title = Editor.Solution.Name,
            Icon = ArgonStyle.Icons.Solution,
            ContextMenu = contextMenu
        };
    }

    private void AddNewProject()
    {
        if (Editor.Solution.FileInfo.Directory is not null)
        {
            ProjectCreator.Show(Editor.Solution.FileInfo.Directory, "NewProject", HandleNewProjectCreated, Editor.Solution);
        }
    }

    private void HandleNewProjectCreated(ArgonProject newProject)
    {
        _treeView.Items.Add(new ProjectTreeItem(newProject, Editor));
    }

    private void ShowSolutionOptions()
    {
        MessageBox.Show("Not implemented", "Argon");
    }

    protected override FrameworkElement GetBodyContent()
    {
        Grid mainGrid = new Grid();

        mainGrid.AddRowAuto(_header);

        IReadOnlyList<ArgonProject> projects = Editor.Solution.Projects;

        for (int i = 0; i < projects.Count; i++)
        {
            _treeView.Items.Add(new ProjectTreeItem(projects[i], Editor));
        }

        ProjectTreeItem firstProject = (ProjectTreeItem)_treeView.Items[0];
        Editor.FindView<ProjectView>()?.ShowProject(firstProject.Project);
        firstProject.IsSelected = true;

        mainGrid.AddRowFill(_treeView);

        return mainGrid;
    }

    protected override string GetDefaultTitle()
    {
        return $"Solution";
    }
}