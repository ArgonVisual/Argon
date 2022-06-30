using System.IO;
using System.Windows.Media;
using ArgonVisual.Widgets;

namespace ArgonVisual.TreeItems;

/// <summary>
/// Represents a folder that can only be placed inside of a solution.
/// </summary>
public class SolutionFolderTreeItem : ArgonTreeItem
{
    /// <summary>
    /// The solution directory.
    /// </summary>
    public DirectoryInfo DirectoryInfo { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="SolutionFolderTreeItem"/>.
    /// </summary>
    /// <param name="directory">The solution folder directory that this tree item represents.</param>
    /// <param name="editor">The <see cref="SolutionEditor"/> that owns this tree item.</param>
    public SolutionFolderTreeItem(DirectoryInfo directory, SolutionEditor editor) : base(directory.Name, editor)
    {
        IsExpanded = true;
        DirectoryInfo = directory;
    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.Folder;
}