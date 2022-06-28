using System.Windows;
using System.Windows.Controls;

namespace ArgonVisual.Widgets;

public class ProjectOptions : ContentControl
{
    public ArgonProject Project { get; }

    public ProjectOptions(ArgonProject project) 
    {
        Project = project;
    }

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