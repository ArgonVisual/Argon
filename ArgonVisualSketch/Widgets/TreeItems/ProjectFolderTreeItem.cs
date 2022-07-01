using System.IO;
using System.Linq;
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
    public DirectoryInfo DirectoryInfo { get; private set; }

    /// <summary>
    /// Initializes a new <see cref="ProjectFolderTreeItem"/>.
    /// </summary>
    /// <param name="directoryInfo">The project folder directory that this represents.</param>
    /// <param name="editor">The <see cref="SolutionEditor"/> that owns this.</param>
    public ProjectFolderTreeItem(DirectoryInfo directoryInfo, SolutionEditor editor) : base(directoryInfo.Name, editor)
    {
        DirectoryInfo = directoryInfo;
        IsExpanded = true;

        ContextMenu.Items.Add(new TextMenuItem("Add Folder", AddNewFolder));
        ContextMenu.Items.Add(new TextMenuItem("Add Code File", AddNewCodeFile));
    }

    public static ProjectFolderTreeItem CreateNewFolderInDirectory(DirectoryInfo directory, SolutionEditor editor) 
    {
        string folderName = PathHelper.MakeUniqueFolderName(directory.FullName, PathHelper.DefaultFolderName);
        DirectoryInfo subDirectory = directory.CreateSubdirectory(folderName);
        ProjectFolderTreeItem newFolderItem = new ProjectFolderTreeItem(subDirectory, editor);
        newFolderItem.RenameItem();
        newFolderItem.IsSelected = true;
        return newFolderItem;
    }

    protected override void RenameItemInternal(string newName)
    {
        string newDirectory = PathHelper.RenameFolder(DirectoryInfo.FullName, newName);
        DirectoryInfo = new DirectoryInfo(newDirectory);
    }

    protected override string? IsValidName(string name)
    {
        if (DirectoryInfo.Parent is not null
         && DirectoryInfo.Parent.GetDirectories().Any((info) => info.Name == name))
        {
            return $"A folder named \"{name}\" already exists.";
        }

        return null;
    }

    protected override void DeleteItemInternal()
    {
        DirectoryInfo.Delete(true);
        if (Parent is ItemsControl itemsControl)
        {
            itemsControl.Items.Remove(this);
        }
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