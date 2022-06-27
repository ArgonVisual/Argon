using System.Windows.Media;

namespace ArgonVisual.TreeItems;

public class SolutionTreeItem : ArgonTreeItem
{
    public SolutionTreeItem(string title) : base(title)
    {
    
    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.Solution;
}