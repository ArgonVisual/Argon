using System.Windows.Media;
using ArgonVisual.Widgets;

namespace ArgonVisual.TreeItems;

public class ProjectFolderTreeItem : ArgonTreeItem 
{
    public ProjectFolderTreeItem(string title, SolutionEditor editor) : base(title, editor)
    {
        IsExpanded = true;
    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.Folder;
}