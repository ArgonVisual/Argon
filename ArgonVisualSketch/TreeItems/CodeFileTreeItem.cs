using System.Windows.Media;

namespace ArgonVisualSketch.TreeItems;

public class CodeFileTreeItem : ArgonTreeItem
{
    public CodeFileTreeItem(string title) : base(title)
    {
        
    }

    protected override ImageSource GetIcon()
    {
        return ArgonStyle.Icons.CodeFile;
    }
}