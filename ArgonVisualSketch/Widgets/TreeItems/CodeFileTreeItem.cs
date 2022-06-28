using System.Windows.Media;
using ArgonVisual.Widgets;

namespace ArgonVisual.TreeItems;

public class CodeFileTreeItem : ArgonTreeItem
{
    public CodeFileTreeItem(string title, SolutionEditor editor) : base(title, editor)
    {
        
    }

    protected override ImageSource GetIcon()
    {
        return ArgonStyle.Icons.CodeFile;
    }
}