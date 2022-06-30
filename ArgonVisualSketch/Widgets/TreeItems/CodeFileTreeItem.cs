using System.Windows.Media;
using ArgonVisual.Widgets;

namespace ArgonVisual.TreeItems;

/// <summary>
/// Represents a code file in a argon tree item.
/// </summary>
public class CodeFileTreeItem : ArgonTreeItem
{
    /// <summary>
    /// Initializes a new instance of <see cref="CodeFileTreeItem"/>.
    /// </summary>
    /// <param name="codeFile">The <see cref="ArgonCodeFile"/> that represents this.</param>
    /// <param name="editor">The editor that owns the tree item.</param>
    public CodeFileTreeItem(ArgonCodeFile codeFile, SolutionEditor editor) : base(codeFile.Name, editor)
    {
        
    }

    protected override ImageSource GetIcon()
    {
        return ArgonStyle.Icons.CodeFile;
    }
}