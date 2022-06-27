using System.Windows.Media;

namespace ArgonVisual.TreeItems;

public class SolutionFolderTreeItem : ArgonTreeItem
{
    public SolutionFolderTreeItem(string title) : base(title)
    {
        IsExpanded = true;
    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.Folder;
}