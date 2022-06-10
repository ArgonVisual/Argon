using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Argon.FileTypes;

namespace Argon.Widgets;

/// <summary>
/// Widget for managing the directory structure of a solution
/// </summary>
public class SolutionDirectoryManager : Border
{
    /// <summary>
    /// The directory that this widget is showing projects for.
    /// All projects in this folder/subfolders should be shown.
    /// </summary>
    public string RootDirectory => _editor.Solution.Directory;

    private SolutionEditor _editor;

    private ArgTreeView _treeView;

    /// <summary>
    /// Initializes a new instance of <see cref="SolutionDirectoryManager"/>.
    /// </summary>
    public SolutionDirectoryManager(SolutionEditor editor) 
    {
        _editor = editor;

        BorderBrush = GlobalStyle.Border;
        BorderThickness = new Thickness(2);
        Background = GlobalStyle.Transparent;

        // Create context menu
        ContextMenu contextMenu = new ContextMenu();
        MenuItem createProjectItem = new MenuItem() 
        {
            Header = "Create New Project"
        };
        createProjectItem.Click += ShowProjectCreator;
        contextMenu.Items.Add(createProjectItem);
        ContextMenu = contextMenu;

        _treeView = new ArgTreeView();

        PopulateTreeView();

        Child = _treeView;
    }

    /// <summary>
    /// Populate the <see cref="_treeView"/> with all of the existing projects and folders.
    /// </summary>
    private void PopulateTreeView() 
    {
        foreach (ArgProject project in _editor.Solution.Projects)
        {
            ArgonProjectTreeItem projectItem = new ArgonProjectTreeItem(this, project);
            _treeView.Items.Add(projectItem);
        }
    }

    private void ShowProjectCreator(object sender, RoutedEventArgs e)
    {
        // TOOD: figure out where the user clicked to place the new project
        // in the appropriate directory
        ProjectCreator.CreateWindow(RootDirectory, AddProject).ShowDialog();
    }

    /// <summary>
    /// Adds a <see cref="ArgProject"/> to the treeview.
    /// </summary>
    /// <param name="project">The project to add.</param>
    public void AddProject(ArgProject project)
    {
        ArgonProjectTreeItem projectItem = new ArgonProjectTreeItem(this, project);
        _treeView.Items.Add(projectItem);

        _editor.Solution.Projects.Add(project);
        _editor.Solution.MarkForSave();
    }

    /// <summary>
    /// Removes a <see cref="ArgonProjectTreeItem"/> from the treeview.
    /// </summary>
    /// <param name="projectItem">The project item to remove.</param>
    public void RemoveProject(ArgonProjectTreeItem projectItem) 
    {
        _treeView.Items.Remove(projectItem);

        _editor.Solution.Projects.Remove(projectItem.Project);
        _editor.Solution.MarkForSave();
    }
}