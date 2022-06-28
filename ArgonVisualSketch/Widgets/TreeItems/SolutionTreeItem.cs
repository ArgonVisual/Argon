using System.Windows.Media;
using ArgonVisual.Widgets;

namespace ArgonVisual.TreeItems;

public class SolutionTreeItem : ArgonTreeItem
{
    public SolutionTreeItem(string title, SolutionEditor editor) : base(title, editor)
    {
        
    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.Solution;
}