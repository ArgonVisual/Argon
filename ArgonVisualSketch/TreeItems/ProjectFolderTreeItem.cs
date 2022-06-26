using System.Windows.Media;

namespace ArgonVisualSketch.TreeItems;

public class ProjectFolderTreeItem : ArgonTreeItem 
{
    public ProjectFolderTreeItem(string title) : base(title)
    {
        IsExpanded = true;
    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.Folder;
}