using System.Windows.Input;
using System.Windows.Media;
using ArgonVisual.Views;
using ArgonVisual.Widgets;

namespace ArgonVisual.TreeItems;

public class ProjectTreeItem : ArgonTreeItem
{
    public ArgonProject Project { get; }

    public ProjectTreeItem(ArgonProject project, SolutionEditor editor) : base(project.Name, editor)
    {
        Project = project;
    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.Project;

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        Editor.FindView<ProjectView>()?.ShowProject(Project);
    }
}