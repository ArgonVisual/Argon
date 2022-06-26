using System.Windows.Media;

namespace ArgonVisualSketch.TreeItems;

public class SolutionFolderTreeItem : ArgonTreeItem
{
    public SolutionFolderTreeItem(string title) : base(title)
    {
        IsExpanded = true;
    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.Folder;
}