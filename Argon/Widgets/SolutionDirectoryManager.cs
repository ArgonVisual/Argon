using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Argon.FileTypes;
using Argon.Helpers;
using Microsoft.Win32;

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
    public string RootDirectory => Editor.Solution.Directory;

    public SolutionEditor Editor { get; }

    private ArgonTreeView _treeView;
    private ArgonSolutionTreeItem _solutionItem;

    /// <summary>
    /// Initializes a new instance of <see cref="SolutionDirectoryManager"/>.
    /// </summary>
    public SolutionDirectoryManager(SolutionEditor editor) 
    {
        Editor = editor;

        BorderBrush = GlobalStyle.Border;
        BorderThickness = new Thickness(2);
        Background = GlobalStyle.Transparent;

        _treeView = new ArgonTreeView();

        _solutionItem = new ArgonSolutionTreeItem(this, Editor.Solution);
        _treeView.Items.Add(_solutionItem);

        PopulateTreeView();

        Child = _treeView;
    }

    /// <summary>
    /// Populate the <see cref="_treeView"/> with all of the existing projects and folders.
    /// </summary>
    private void PopulateTreeView() 
    {
        // Folders in solution - show first
        foreach (SolutionDirectory solutionDirectory in Editor.Solution.SolutionDirectories)
        {
            _solutionItem.Items.Add(new ArgonSolutionFolderTreeItem(this, solutionDirectory));
        }

        // Projects in solution
        foreach (ArgonProject project in Editor.Solution.SolutionProjects)
        {
            if (Editor.Solution.Directory == project.Directory.SubstringBeforeLast(Path.DirectorySeparatorChar))
            {
                ArgonProjectTreeItem projectItem = new ArgonProjectTreeItem(this, project);
                _solutionItem.Items.Add(projectItem);
            }
        }
    }
}