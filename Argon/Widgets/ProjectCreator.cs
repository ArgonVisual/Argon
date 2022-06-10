using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Argon.FileTypes;
using Argon.Helpers;

namespace Argon.Widgets;

/// <summary>
/// Widget that lets the user choose from many
/// project templates.
/// </summary>
public class ProjectCreator : Border
{
    /// <summary>
    /// This gets called when the new project has been created.
    /// </summary>
    public Action<ArgProject> Completed { get; }

    /// <summary>
    /// The directory to save the project to.
    /// </summary>
    private string _saveDirectory;

    private ArgTextBox _projectNameText;

    /// <summary>
    /// Initializes a new instance of <see cref="ProjectCreator"/>.
    /// </summary>
    /// <param name="saveDirectory">If a project is created, then this is the directory to save the project.</param>
    public ProjectCreator(string saveDirectory, Action<ArgProject> completedCallback)
    {
        _saveDirectory = saveDirectory;
        Completed = completedCallback;

        BorderThickness = new Thickness(2);
        BorderBrush = GlobalStyle.Border;
        Background = GlobalStyle.Background;

        Grid mainGrid = new Grid() 
        {
            Margin = new Thickness(10)
        };

        TextButton createBlankProjectButton = new TextButton("Create Blank Project")
        {
            Width = 500,
            Height = 100
        };
        createBlankProjectButton.Click += CreateBlankProject;

        mainGrid.AddRowAuto(new ArgTextBlock("Choose a Project Template")
        {
            FontSize = 25
        });

        Grid projectTempatesGrid = new Grid() 
        {
            Margin = new Thickness(20)
        };

        projectTempatesGrid.AddRowAuto(createBlankProjectButton);

        mainGrid.AddRowFill(projectTempatesGrid);

        mainGrid.AddRowAuto(new ArgTextBlock("Project Name") 
        {
            FontSize = GlobalStyle.FontSizeSmall,
            HorizontalAlignment = HorizontalAlignment.Left
        });


        _projectNameText = new ArgTextBox()
        {
            Text = "NewProject"
        };
        mainGrid.AddRowAuto(_projectNameText);


        TextButton cancelButton = new TextButton("Cancel")
        {
            Width = 300,
            Padding = new Thickness(5),
        };
        cancelButton.Click += CloseProjectCreatorWindow;
        mainGrid.AddRowAuto(cancelButton);

        Child = mainGrid;
    }

    private void CloseProjectCreatorWindow()
    {
        if (Argon.ProjectCreatorWindow is not null)
        {
            Argon.ProjectCreatorWindow.Close();
            Argon.ProjectCreatorWindow = null;
        }
    }

    private void CreateBlankProject()
    {
        ArgProject blankProject = ArgProject.CreateAndSaveBlank(_saveDirectory, _projectNameText.Text);
        Completed(blankProject);
        CloseProjectCreatorWindow();
    }

    /// <summary>
    /// Creates a new window containing <see cref="ProjectCreator"/>.
    /// </summary>
    /// <returns>The new window.</returns>
    public static Window CreateWindow(string saveDirectory, Action<ArgProject> completedCallback) 
    {
        Window newWindow = new Window() 
        {
            Title = "Project Creator",
            Content = new ProjectCreator(saveDirectory, completedCallback),
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Width = 800,
            Height = 600,
            ResizeMode = ResizeMode.NoResize,
            WindowStyle = WindowStyle.None,
            
            AllowsTransparency = true
        };
        Argon.ProjectCreatorWindow = newWindow;

        return newWindow;
    }
}