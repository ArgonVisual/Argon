using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ArgonVisual.Views;
using ArgonVisual.Widgets;

namespace ArgonVisual.TreeItems;

/// <summary>
/// Represents a <see cref="ArgonProject"/> in a tree view.
/// </summary>
public class ProjectTreeItem : ArgonTreeItem
{
    /// <summary>
    /// The project that this tree item is representing.
    /// </summary>
    public ArgonProject Project { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ProjectTreeItem"/>.
    /// </summary>
    /// <param name="project">The project to represent.</param>
    /// <param name="editor">The <see cref="SolutionEditor"/> that owns this tree item.</param>
    public ProjectTreeItem(ArgonProject project, SolutionEditor editor) : base(project.Name, editor)
    {
        Project = project;
    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.Project;

    protected override void OnSelected(RoutedEventArgs e)
    {
        Editor.FindView<ProjectView>()?.ShowProject(Project);
    }

    protected override string? IsValidName(string name)
    {
        if (Project.FileInfo.Directory is not null
         && Project.FileInfo.Directory.Parent is not null)
        {
            if (Project.FileInfo.Directory.Parent.GetDirectories().Any((info) => info.Name.Equals(name, System.StringComparison.Ordinal)))
            {
                return $"A project named \"{name}\" already exists in the same directory";
            }
        }

        return null;
    }

    protected override void RenameItemInternal(string newName)
    {
        Project.Rename(newName);
    }
}