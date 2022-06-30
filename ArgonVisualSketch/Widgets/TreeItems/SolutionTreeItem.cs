using System.Windows.Media;
using ArgonVisual.Widgets;

namespace ArgonVisual.TreeItems;

/// <summary>
/// Represents a <see cref="ArgonSolution"/>.
/// </summary>
public class SolutionTreeItem : ArgonTreeItem
{
    /// <summary>
    /// Initializes a new instance of <see cref="SolutionTreeItem"/>.
    /// </summary>
    /// <param name="solution">The <see cref="ArgonSolution"/> that this tree item represents.</param>
    /// <param name="editor">The <see cref="SolutionEditor"/> that owns this tree item.</param>
    public SolutionTreeItem(ArgonSolution solution, SolutionEditor editor) : base(solution.Name, editor)
    {
        
    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.Solution;
}