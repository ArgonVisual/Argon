using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Argon.FileTypes;
using Argon.Helpers;

namespace Argon.Widgets;

public class ArgonTreeView : TreeView
{
    public ArgonTreeView() 
    {
        Background = GlobalStyle.Transparent;
        BorderBrush = null;
        BorderThickness = new Thickness(0);
    }
}

/// <summary>
/// Base class for representing files and folders in a tree view.
/// </summary>
public abstract class ArgonTreeItem : TreeViewItem
{
    protected SolutionDirectoryManager DirectoryManager;

    private InlineEditableTextBox _nameText;

    public abstract string NameOfFile { get; }

    private StackPanel _innerPanel;

    public ArgonTreeItem(SolutionDirectoryManager directoryManager, string defaultName) 
    {
        _innerPanel = new StackPanel() 
        {
            Orientation = Orientation.Horizontal
        };

        _innerPanel.Children.Add(new Image() 
        {
            Source = GetIcon(),
            Width = 25,
            Height = 25,
            Margin = new Thickness(1)
        });

        _innerPanel.Children.Add(_nameText = new InlineEditableTextBox()
        {
            Text = defaultName,
            VerticalAlignment = VerticalAlignment.Center
        });

        Header = _innerPanel;

        _nameText.BindTextChanged(NameTextChanged);

        DirectoryManager = directoryManager;

        ContextMenu contextMenu = new ContextMenu();

        MenuItem renameItem = new MenuItem()
        {
            Header = "Rename"
        };
        renameItem.Click += (sender, args) => RenameItem();
        contextMenu.Items.Add(renameItem);

        if (this is not ArgonSolutionTreeItem)
        {
            MenuItem deleteItem = new MenuItem()
            {
                Header = this is ArgonProjectTreeItem ? "Remove" : "Delete"
            };
            deleteItem.Click += (sender, args) => RemoveOrDelete();
            contextMenu.Items.Add(deleteItem);
        }

        ExpandContextMenu(contextMenu);

        ContextMenu = contextMenu;
    }

    protected abstract ImageSource GetIcon();

    private void RemoveOrDelete() 
    {
        if (MessageBox.Show($"{NameOfFile} will be deleted", "Are you sure?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            RemoveOrDeleteItem();
        }
    }

    private void NameTextChanged(object sender, TextChangedEventArgs e)
    {
        if (IsValidName(_nameText.Text))
        {
            _nameText.TextBoxBackground = GlobalStyle.BackgroundDark;
        }
        else
        {
            _nameText.TextBoxBackground = GlobalStyle.Error;
        }
    }

    public void RenameItem()
    {
        _nameText.EnterEditMode();
    }

    protected virtual void ChangeName(string newName) { }

    protected virtual void RemoveOrDeleteItem() { }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.F2)
        {
            _nameText.EnterEditMode();
            e.Handled = true;
        }
        else if (_nameText.IsEditing && e.Key == Key.Enter)
        {
            _nameText.ExitEditMode();

            if (IsValidName(_nameText.Text))
            {
                ChangeName(_nameText.Text);
            }
            else
            {
                _nameText.Text = NameOfFile;
            }

            e.Handled = true;
        }
    }

    protected virtual void ExpandContextMenu(ContextMenu contextMenu) 
    {

    }

    protected virtual bool IsValidName(string newName) 
    {
        return true;
    }
}

public class ArgonSolutionTreeItem : ArgonTreeItem 
{
    /// <summary>
    /// The <see cref="ArgonSolution"/> that this is representing.
    /// </summary>
    public ArgonSolution Solution { get; }

    public override string NameOfFile => Solution.Name;

    public ArgonSolutionTreeItem(SolutionDirectoryManager directoryManager, ArgonSolution solution) : base(directoryManager, solution.Name)
    {
        IsExpanded = true;
        Solution = solution;
    }

    protected override void ExpandContextMenu(ContextMenu contextMenu)
    {
        MenuItem addProjectItem = new MenuItem()
        {
            Header = "Add New Project"
        };

        MenuItem addExistingProjectItem = new MenuItem()
        {
            Header = "Add Existing Project"
        };

        MenuItem addFolderItem = new MenuItem()
        {
            Header = "Add Folder"
        };

        addProjectItem.Click += AddProject;
        addExistingProjectItem.Click += AddExistingProject;
        addFolderItem.Click += AddFolder;

        contextMenu.Items.Add(addFolderItem);
        contextMenu.Items.Add(addProjectItem);
        contextMenu.Items.Add(addExistingProjectItem);
    }

    private void AddProject(object sender, RoutedEventArgs e)
    {
        IsExpanded = true;

        ArgonProjectTreeItem project = TreeViewUtils.CreateBlankProjectForDirectory(DirectoryManager, Solution, new DirectoryInfo(Solution.Directory));
        Items.Add(project);
        project.RenameItem();
    }

    private void AddExistingProject(object sender, RoutedEventArgs e)
    {
        IsExpanded = true;
    }

    private void AddFolder(object sender, RoutedEventArgs e)
    {
        IsExpanded = true;
        ArgonFolderTreeItem newItem = TreeViewUtils.CreateSolutionFolderItem(DirectoryManager, new DirectoryInfo(Solution.Directory));
        newItem.RenameItem();
        Items.Add(newItem);
    }

    protected override ImageSource GetIcon() => GlobalStyle.SolutionIcon;
}

/// <summary>
/// Represents a <see cref="ArgonProject"/> in a tree view.
/// </summary>
public class ArgonProjectTreeItem : ArgonTreeItem
{
    /// <summary>
    /// The <see cref="ArgonProject"/> that this is representing.
    /// </summary>
    public ArgonProject Project { get; }

    public override string NameOfFile => Project.Name;

    public ArgonProjectTreeItem(SolutionDirectoryManager directoryManager, ArgonProject project) : base(directoryManager, project.Name)
    {
        Project = project;
        IsExpanded = true;
        DirectoryInfo directoryInfo = new DirectoryInfo(project.Directory);
        IEnumerable<DirectoryInfo> directories = directoryInfo.EnumerateDirectories();
        foreach (DirectoryInfo directory in directories)
        {
            Items.Add(new ArgonProjectFolderTreeItem(DirectoryManager, directory));
        }
    }

    protected override void RemoveOrDeleteItem()
    {
        TreeViewUtils.RemoveFromParent(this);
        DirectoryManager.Editor.Solution.GetProjectListForDirectory(Project.Directory).Remove(Project);
        DirectoryManager.Editor.Solution.Save();
    }

    protected override void ChangeName(string newName)
    {
        Project.Rename(newName);
        DirectoryManager.Editor.Solution.Save();
    }

    protected override void ExpandContextMenu(ContextMenu contextMenu)
    {
        MenuItem addFolderItem = new MenuItem() 
        {
            Header = "Add Folder"
        };

        MenuItem addCodeFileItem = new MenuItem()
        {
            Header = "Add Code File"
        };

        addFolderItem.Click += AddFolder;

        addCodeFileItem.Click += AddCodeFile;

        contextMenu.Items.Add(addFolderItem);
        contextMenu.Items.Add(addCodeFileItem);
    }

    private void AddCodeFile(object sender, RoutedEventArgs e)
    {
        IsExpanded = true;
    }

    private void AddFolder(object sender, RoutedEventArgs e)
    {
        IsExpanded = true;

        ArgonFolderTreeItem newItem = TreeViewUtils.CreateProjectFolderItem(DirectoryManager, new DirectoryInfo(Project.Directory));
        Items.Add(newItem);
        newItem.RenameItem();
    }

    protected override ImageSource GetIcon() => GlobalStyle.ProjectIcon;
}

/// <summary>
/// Represents a folder in a treeview
/// </summary>
public abstract class ArgonFolderTreeItem : ArgonTreeItem
{
    protected DirectoryInfo Directory;

    public override string NameOfFile => Directory.Name;

    public ArgonFolderTreeItem(SolutionDirectoryManager directoryManager, DirectoryInfo directory) : base(directoryManager, directory.Name)
    {
        Directory = directory;

        // ===============================================================
        // Derived classes are responsible for populating there subfolders
        // ===============================================================


        // TreeViewUtils.RemoveFromParent(this);
        // IEnumerable<DirectoryInfo> directories = Directory.EnumerateDirectories();
        // foreach (DirectoryInfo subDirectory in directories)
        // {
        //     Items.Add(CreateFolder(subDirectory));
        // }



        // List<ArgonProject> projects = this is ArgonSolutionFolderTreeItem 
        //     ? DirectoryManager.Editor.Solution.SolutionProjects 
        //     // This is a folder within a folder so the parent we definitly not be the solution
        //     : DirectoryManager.Editor.Solution.GetProjectListForDirectory(Directory.Parent.FullName);
        // 
        // foreach (ArgonProject project in projects)
        // {
        //     if (Directory.FullName == project.Directory.SubstringBeforeLast(Path.DirectorySeparatorChar))
        //     {
        //         ArgonProjectTreeItem projectItem = new ArgonProjectTreeItem(DirectoryManager, project);
        //         Items.Add(projectItem);
        //     }
        // }
    }

    protected override void RemoveOrDeleteItem()
    {
        System.IO.Directory.Delete(Directory.FullName , true);
        TreeViewUtils.RemoveFromParent(this);
    }

    protected override void ChangeName(string newName)
    {
        PathHelper.RenameDirectory(Directory, newName);
    }

    protected override bool IsValidName(string newName)
    {
        return !Directory.Parent.EnumerateDirectories().Any((directory) => directory.Name == newName);
    }

    protected override void ExpandContextMenu(ContextMenu contextMenu)
    {
        MenuItem addFolderItem = new MenuItem()
        {
            Header = "Add Folder"
        };

        addFolderItem.Click += AddFolder;
        contextMenu.Items.Add(addFolderItem);
    }

    protected abstract void AddFolder(object sender, RoutedEventArgs e);

    protected override ImageSource GetIcon() => GlobalStyle.FolderIcon;
}

public class ArgonProjectFolderTreeItem : ArgonFolderTreeItem
{
    public ArgonProjectFolderTreeItem(SolutionDirectoryManager directoryManager, DirectoryInfo directory) : base(directoryManager, directory)
    {

    }

    protected override void ExpandContextMenu(ContextMenu contextMenu)
    {
        base.ExpandContextMenu(contextMenu);

        MenuItem addCodeFileItem = new MenuItem()
        {
            Header = "Add Code File"
        };
        addCodeFileItem.Click += AddCodeFile;
        contextMenu.Items.Add(addCodeFileItem);
    }

    private void AddCodeFile(object sender, RoutedEventArgs e)
    {
        IsExpanded = true;
    }

    protected override void AddFolder(object sender, RoutedEventArgs e)
    {
        IsExpanded = true;

        ArgonFolderTreeItem newItem = TreeViewUtils.CreateProjectFolderItem(DirectoryManager, Directory);
        Items.Add(newItem);
        newItem.RenameItem();
    }
}

public class ArgonSolutionFolderTreeItem : ArgonFolderTreeItem
{
    private SolutionDirectory _solutionDirectory;

    public ArgonSolutionFolderTreeItem(SolutionDirectoryManager directoryManager, SolutionDirectory solutionDirectory) : base(directoryManager, solutionDirectory.Directory)
    {
        IsExpanded = true;
        _solutionDirectory = solutionDirectory;

        // Show the folders in the folder first then the projects
        foreach (SolutionDirectory subDirectory in _solutionDirectory.SubDirectories)
        {
            Items.Add(new ArgonSolutionFolderTreeItem(directoryManager, subDirectory));
        }

        // Add the projects
        foreach (ArgonProject project in _solutionDirectory.Projects)
        {
            Items.Add(new ArgonProjectTreeItem(directoryManager, project));
        }
    }

    protected override void ExpandContextMenu(ContextMenu contextMenu)
    {
        base.ExpandContextMenu(contextMenu);

        MenuItem addProjectItem = new MenuItem()
        {
            Header = "Add New Project"
        };

        MenuItem addExistingProjectItem = new MenuItem()
        {
            Header = "Add Existing Project"
        };

        addProjectItem.Click += AddProject;
        addExistingProjectItem.Click += AddExistingProject;

        contextMenu.Items.Add(addProjectItem);
        contextMenu.Items.Add(addExistingProjectItem);
    }

    private void AddProject(object sender, RoutedEventArgs e)
    {
        IsExpanded = true;

        ArgonProjectTreeItem project = TreeViewUtils.CreateBlankProjectForDirectory(DirectoryManager, DirectoryManager.Editor.Solution, Directory);
        Items.Add(project);
        project.RenameItem();
    }

    private void AddExistingProject(object sender, RoutedEventArgs e)
    {
        IsExpanded = true;
    }

    protected override void ChangeName(string newName)
    {
        base.ChangeName(newName);
        if (_solutionDirectory.Directory.Parent is null)
        {
            throw new Exception();
        }
        _solutionDirectory.Directory = new DirectoryInfo(Path.Combine(_solutionDirectory.Directory.Parent.FullName, newName));
        DirectoryManager.Editor.Solution.Save(); // Save the new name of the file for in the SolutionDirectories
    }

    protected override void AddFolder(object sender, RoutedEventArgs e)
    {
        IsExpanded = true;

        ArgonFolderTreeItem newItem = TreeViewUtils.CreateSolutionFolderItem(DirectoryManager, Directory);
        Items.Add(newItem);
        newItem.RenameItem();
    }
}

/// <summary>
/// Represents a <see cref="ArgonCodeFile"/> in a treeview.
/// </summary>
public class ArgonCodeFileTreeItem : ArgonTreeItem
{
    private ArgonCodeFile _codeFile;

    public override string NameOfFile => _codeFile.Name;

    public ArgonCodeFileTreeItem(SolutionDirectoryManager directoryManager, ArgonCodeFile codeFile) : base(directoryManager, codeFile.Name)
    {
        _codeFile = codeFile;
    }

    protected override void RemoveOrDeleteItem()
    {
        File.Delete(_codeFile.Filename);
        TreeViewUtils.RemoveFromParent(this);
    }

    protected override ImageSource GetIcon() => GlobalStyle.CodeFileIcon;
}

public static class TreeViewUtils 
{
    /// <summary>
    /// This is the default name used when creating a new folder
    /// </summary>
    const string FolderStartName = "NewFolder";

    public static ArgonFolderTreeItem CreateSolutionFolderItem(SolutionDirectoryManager directoryManager, DirectoryInfo directory) 
    {
        // This code finds a unique name for the new folder by searching in the existing folder names and making sure that none of them are equal to NewFolder
        // If there is a duplicate then a number gets appended to the end ex: NewFolder1, NewFolder2, NewFolder3 until there is not a duplicate name
        
        string resultName = FolderStartName;
        int counter = 0;
        while (Directory.Exists(Path.Combine(directory.FullName, resultName)))
        {
            counter++;
            resultName = FolderStartName + counter.ToString();
        }
        DirectoryInfo subDirectory = directory.CreateSubdirectory(resultName);
        SolutionDirectory solutionDirectory = new SolutionDirectory(subDirectory);

        // True if this folder is directly in the solution (not in any subfolders)
        // If this is true then that means that the parent directory is the same as the solution
        bool IsDirectlyInSolution = directory.FullName == directoryManager.Editor.Solution.Directory;


        if (IsDirectlyInSolution)
        {
            // Add the new solution directory to the solution so it can be serialized
            directoryManager.Editor.Solution.SolutionDirectories.Add(solutionDirectory);
        }
        else
        {
            // This folder is a sub folder of another folder that is in the solution
            // I need to find the SolutionDirectory that represents the parent of this folder
            // so i can add it to that to not screw up the hierarchy

            // the directory function parameter is the parent directory of this folder

            // Found the solution directory that represents the parent of this folder
            SolutionDirectory parentSolutionDirectory = directoryManager.Editor.Solution.GetSolutionDirectory(directory);
            // Add this solution directory add a sub directory of its parent
            parentSolutionDirectory.SubDirectories.Add(solutionDirectory);
        }

        directoryManager.Editor.Solution.Save(); // Save the new directory
        return new ArgonSolutionFolderTreeItem(directoryManager, solutionDirectory);
    }

    public static ArgonFolderTreeItem CreateProjectFolderItem(SolutionDirectoryManager directoryManager, DirectoryInfo directory)
    {
        string resultName = FolderStartName;
        int counter = 0;
        while (Directory.Exists(Path.Combine(directory.FullName, resultName)))
        {
            counter++;
            resultName = FolderStartName + counter.ToString();
        }
        DirectoryInfo subDirectory = directory.CreateSubdirectory(resultName);

        return new ArgonProjectFolderTreeItem(directoryManager, subDirectory);
    }

    public static ArgonProjectTreeItem CreateBlankProjectForDirectory(SolutionDirectoryManager directoryManager, ArgonSolution solution, DirectoryInfo directory) 
    {
        ArgonProject project = ArgonProject.CreateAndSaveBlank(directory.FullName, "NewProject");
        ArgonProjectTreeItem projectItem = new ArgonProjectTreeItem(directoryManager, project);

        solution.GetProjectListForDirectory(directory.FullName).Add(project);
        solution.Save();

        return projectItem;
    }

    public static ArgonCodeFileTreeItem CreateCodeFileItem() 
    {
        throw new NotImplementedException();
    }

    public static void RemoveFromParent(TreeViewItem treeItem) 
    {
        if (treeItem.Parent is ItemsControl itemsControl)
        {
            itemsControl.Items.Remove(treeItem);
        }
    }
}