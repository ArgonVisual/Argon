using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ArgonVisual.Helpers;
using ArgonVisual.TreeItems;
using ArgonVisual.Widgets;

namespace ArgonVisual.Views;

/// <summary>
/// Shows the selected project from <see cref="SolutionView"/>.
/// </summary>
public class ProjectView : ViewBase
{
    private TreeViewHeader _header;
    private TreeView? _treeView;

    private ContentControl _treeViewContent;

    public ArgonProject? ShownProject { get; private set; }

    public ProjectView(SolutionEditor solutionEditor) : base(solutionEditor)
    {
        _treeViewContent = new ContentControl();

        ContextMenu contextMenu = new ContextMenu();

        contextMenu.Items.Add(new TextMenuItem("Add Folder", AddNewFolder));
        contextMenu.Items.Add(new TextMenuItem("Add Code File", AddNewCodeFile));

        _header = new TreeViewHeader(ShowProjectOptions)
        {
            Icon = ArgonStyle.Icons.Project,
            ContextMenu = contextMenu
        };
    }

    private bool CanCreateItem() 
    {
        return _treeView is not null && ShownProject is not null && ShownProject.FileInfo.Directory is not null;
    }

    private void AddNewCodeFile()
    {
        if (CanCreateItem())
        {
            _treeView.Items.Add(CodeFileTreeItem.CreateNewCodeFileInDirectory(ShownProject.FileInfo.Directory, Editor));
        }
    }

    private void AddNewFolder()
    {
        if (CanCreateItem())
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

    private TreeView MakeTreeViewForProject(ArgonProject project) 
    {
        TreeView treeView = new TreeView();

        if (project.FileInfo.Directory is not null)
        {
            PopulateDirectory(project.FileInfo.Directory, treeView);
        }

        return treeView;
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

        mainGrid.AddRowFill(_treeViewContent);

        return mainGrid;
    }

    protected override string GetDefaultTitle()
    {
        return "Project";
    }

    public void ShowProject(ArgonProject project)
    {
        if (ShownProject != project)
         {
            ShownProject = project;
            _header.Title = project.Name;

            if (project.TreeView is null && project.FileInfo.Directory is not null)
            {
                _treeView = project.TreeView = MakeTreeViewForProject(project);
            }

            _treeViewContent.Content = project.TreeView;
        }
    }

    private void PopulateDirectory(DirectoryInfo directoryInfo, ItemsControl itemsControl) 
    {
        itemsControl.Items.Clear();

        IEnumerable<DirectoryInfo> directories = directoryInfo.EnumerateDirectories();
        foreach (DirectoryInfo directory in directories)
        {
            ProjectFolderTreeItem newFolder = new ProjectFolderTreeItem(directory, Editor);
            PopulateDirectory(directory, newFolder);
            itemsControl.Items.Add(newFolder);
        }

        IEnumerable<FileInfo> codeFiles = directoryInfo.EnumerateFiles($"*{ArgonFileExtensions.CodeFile}");
        foreach (FileInfo codeFile in codeFiles)
        {
            CodeFileTreeItem newFolder = new CodeFileTreeItem(ArgonCodeFile.Read(codeFile), Editor);
            itemsControl.Items.Add(newFolder);
        }
    }
}