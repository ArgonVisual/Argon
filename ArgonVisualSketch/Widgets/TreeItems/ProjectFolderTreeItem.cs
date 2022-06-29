using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using ArgonVisual.Helpers;
using ArgonVisual.Widgets;

namespace ArgonVisual.TreeItems;

/// <summary>
/// Represents a folder that can be placed in a <see cref="ArgonProject"/>.
/// This can contain more subfolders or <see cref="ArgonCodeFile"/>s.
/// </summary>
public class ProjectFolderTreeItem : ArgonTreeItem 
{
    /// <summary>
    /// The directory that this item is representing.
    /// </summary>
    public DirectoryInfo DirectoryInfo { get; }

    public ProjectFolderTreeItem(DirectoryInfo directoryInfo, SolutionEditor editor) : base(directoryInfo.Name, editor)
    {
        DirectoryInfo = directoryInfo;
        IsExpanded = true;

        ContextMenu contextMenu = new ContextMenu();

        TextMenuItem addFolderItem = new TextMenuItem("Add Folder", AddNewFolder);
        TextMenuItem addCodeFileItem = new TextMenuItem("Add Code File", AddNewCodeFile);
        contextMenu.Items.Add(addFolderItem);
        contextMenu.Items.Add(addCodeFileItem);

        ContextMenu = contextMenu;
    }

    public static ProjectFolderTreeItem CreateNewFolderInDirectory(DirectoryInfo directory, SolutionEditor editor) 
    {
        string folderName = PathHelper.MakeUniqueFolderName(directory.FullName, PathHelper.DefaultFolderName);
        DirectoryInfo subDirectory = directory.CreateSubdirectory(folderName);
        return new ProjectFolderTreeItem(subDirectory, editor);
    }

    private void AddNewFolder()
    {
        Items.Add(CreateNewFolderInDirectory(DirectoryInfo, Editor));
    }

    private void AddNewCodeFile()
    {

    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.Folder;
}