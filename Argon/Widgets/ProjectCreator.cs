using System;
using System.Windows;
using System.Windows.Controls;

namespace Argon.Widgets;

/// <summary>
/// Widget that lets the user choose from many
/// project templates.
/// </summary>
public class ProjectCreator : Border
{
    /// <summary>
    /// Initializes a new instance of <see cref="ProjectCreator"/>.
    /// </summary>
    public ProjectCreator() 
    {
        Background = GlobalStyle.Background;

        TextButton createBlankProjectButton = new TextButton("Create Blank Project") 
        {
            Width = 500,
            Height = 100
        };
        createBlankProjectButton.Click += CreateBlankProject;

        Child = createBlankProjectButton;
    }

    private void CreateBlankProject()
    {

    }

    /// <summary>
    /// Creates a new window containing <see cref="ProjectCreator"/>.
    /// </summary>
    /// <returns>The new window.</returns>
    public static Window CreateWindow() 
    {
        Window newWindow = new Window() 
        {
            Title = "Project Creator",
            Content = new ProjectCreator(),
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Width = 800,
            Height = 600,
        };

        return newWindow;
    }
}