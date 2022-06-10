using System.IO;
using System.Windows;
using System.Windows.Controls;
using Argon.FileTypes;

namespace Argon.Widgets;

public class ArgTreeView : TreeView
{
    public ArgTreeView() 
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

    public ArgonTreeItem(SolutionDirectoryManager directoryManager) 
    {
        DirectoryManager = directoryManager;

        ContextMenu contextMenu = new ContextMenu();

        MenuItem deleteItem = new MenuItem()
        {
            Header = this is ArgonProjectTreeItem ? "Remove" : "Delete"
        };
        deleteItem.Click += (sender, args) => RemoveOrDeleteItem();

        contextMenu.Items.Add(deleteItem);

        ContextMenu = contextMenu;
    }

    protected abstract void RemoveOrDeleteItem();
}

/// <summary>
/// Represents a <see cref="ArgProject"/> in a tree view.
/// </summary>
public class ArgonProjectTreeItem : ArgonTreeItem
{
    /// <summary>
    /// The <see cref="ArgProject"/> that this is representing.
    /// </summary>
    public ArgProject Project { get; }

    public ArgonProjectTreeItem(SolutionDirectoryManager directoryManager, ArgProject project) : base(directoryManager)
    {
        Project = project;
        Header = new ArgTextBlock(project.Name);
    }

    protected override void RemoveOrDeleteItem()
    {
        DirectoryManager.RemoveProject(this);
    }
}

/// <summary>
/// Represents a folder in a treeview
/// </summary>
public class ArgonFolderTreeItem : ArgonTreeItem
{
    private DirectoryInfo _directory;

    public ArgonFolderTreeItem(SolutionDirectoryManager directoryManager, DirectoryInfo directory) : base(directoryManager)
    {
        _directory = directory;
        Header = new ArgTextBlock(_directory.Name);
    }

    protected override void RemoveOrDeleteItem()
    {
        _directory.Delete();
    }
}

/// <summary>
/// Represents a <see cref="ArgCodeFile"/> in a treeview.
/// </summary>
public class ArgonCodeFileTreeItem : ArgonTreeItem
{
    private ArgCodeFile _codeFile;

    public ArgonCodeFileTreeItem(SolutionDirectoryManager directoryManager, ArgCodeFile codeFile) : base(directoryManager)
    {
        _codeFile = codeFile;
        Header = new ArgTextBlock(_codeFile.Name);
    }

    protected override void RemoveOrDeleteItem()
    {
        File.Delete(_codeFile.Filename);
    }
}