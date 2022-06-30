using System.Windows;
using System.Windows.Controls;

namespace ArgonVisual.Widgets;

/// <summary>
/// Widget to edit the options for a <see cref="ArgonProject"/>.
/// </summary>
public class ProjectOptions : ContentControl
{
    /// <summary>
    /// The project that is being edited.
    /// </summary>
    public ArgonProject Project { get; }

    public ProjectOptions(ArgonProject project) 
    {
        Project = project;
        // THIS IS NOT IMPLEMENTED
    }

    /// <summary>
    /// Shows a new window containing <see cref="ProjectOptions"/> with the project to edit.
    /// </summary>
    /// <param name="project">The project to edit.</param>
    public static void Show(ArgonProject project) 
    {
        Window window = new Window()
        {
            Title = $"Argon Project Options - {project.Name}",
            Width = 700,
            Height = 500,
            Content = new ProjectOptions(project),
            ResizeMode = ResizeMode.NoResize,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };

        window.ShowDialog();
    }
}