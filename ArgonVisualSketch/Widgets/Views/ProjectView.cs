using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ArgonVisual.Helpers;
using ArgonVisual.TreeItems;
using ArgonVisual.Widgets;

namespace ArgonVisual.Views;

public class ProjectView : ViewBase
{
    private TreeViewHeader _header;
    private TreeView _treeView;

    public ArgonProject? ShownProject { get; private set; }

    public ProjectView(SolutionEditor solutionEditor) : base(solutionEditor)
    {
        _treeView = new TreeView();

        ContextMenu contextMenu = new ContextMenu();

        TextMenuItem addFolderItem = new TextMenuItem("Add Folder", AddNewFolder);
        TextMenuItem addCodeFileItem = new TextMenuItem("Add Code File", AddNewCodeFile);

        contextMenu.Items.Add(addFolderItem);
        contextMenu.Items.Add(addCodeFileItem);

        _header = new TreeViewHeader(ShowProjectOptions)
        {
            Icon = ArgonStyle.Icons.Project,
            ContextMenu = contextMenu
        };
    }

    private void AddNewCodeFile()
    {
        
    }

    private void AddNewFolder()
    {
        if (ShownProject is not null && ShownProject.FileInfo.Directory is not null)
        {
            _treeView.Items.Add(ProjectFolderTreeItem.CreateNewFolderInDirectory(ShownProject.FileInfo.Directory, Editor));
        }
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

        // ProjectFolderTreeItem projectFolder = new ProjectFolderTreeItem("MyFolder", Editor);
        // projectFolder.Items.Add(new CodeFileTreeItem("MyFile", Editor) { IsSelected = true });
        // projectFolder.Items.Add(new CodeFileTreeItem("MyOtherFile", Editor));
        // 
        // _treeView.Items.Add(projectFolder);
        // _treeView.Items.Add(new CodeFileTreeItem("MySpecialFile", Editor));

        mainGrid.AddRowAuto(_header);

        mainGrid.AddRowFill(_treeView);

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

        if (project.FileInfo.Directory is not null)
        {
            PopulateDirectory(project.FileInfo.Directory, _treeView);
        }
    }

    private void PopulateDirectory(DirectoryInfo directoryInfo, ItemsControl itemsControl) 
    {
        IEnumerable<DirectoryInfo> directories = directoryInfo.EnumerateDirectories();
        foreach (DirectoryInfo directory in directories)
        {
            ProjectFolderTreeItem newFolder = new ProjectFolderTreeItem(directory, Editor);
            PopulateDirectory(directory, newFolder);
            itemsControl.Items.Add(newFolder);
        }
    }
}