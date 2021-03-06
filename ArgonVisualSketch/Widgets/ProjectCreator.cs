using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArgonVisual.Helpers;
using static ArgonVisual.Helpers.WidgetHelper;

namespace ArgonVisual;

/// <summary>
/// Manages the creation of new projects.
/// </summary>
public class ProjectCreator : Grid
{
    private ListBox _projectTemplatesPanel;

    private DirectoryInfo _directory;
    private NameTextBox _projectNameText;

    private Action<ArgonProject> _finished;

    private ArgonSolution _solution;

    /// <summary>
    /// Initializes a new instance of <see cref="ProjectCreator"/>.
    /// </summary>
    /// <param name="directory">The directory to place the project in once it gets created. A subdirectory gets created in this directory in which the projecy file gets placed in,</param>
    /// <param name="defaultName">The default name of the project. The user can change this.</param>
    public ProjectCreator(DirectoryInfo directory, string defaultName, Action<ArgonProject> finished, ArgonSolution solution)
    {
        _directory = directory;
        _finished = finished;
        _solution = solution;
        _projectNameText = new NameTextBox("Project Name")
        {
            Text = defaultName
        };

        _projectTemplatesPanel = new ListBox()
        {
            Background = null,
            BorderBrush = null
        };

        Button createButton = new Button()
        {
            Margin = new Thickness(15),
            HorizontalAlignment = HorizontalAlignment.Center,
            Content = new TextBlock()
            {
                Text = "Create",
                Foreground = Brushes.Black,
                FontSize = 30,
                Margin = new Thickness(3)
            }
        };

        createButton.Click += CreateSelectedProjectTemplate;

        PopulateProjectTemplates();

        _projectTemplatesPanel.SelectedItem = _projectTemplatesPanel.Items[0];

        this.AddRowFill(_projectTemplatesPanel);
        this.AddRowAuto(_projectNameText);
        this.AddRowAuto(createButton);
    }

    private void CreateSelectedProjectTemplate(object sender, RoutedEventArgs e)
    {
        DirectoryInfo newDirectory = _directory.CreateSubdirectory(_projectNameText.Text);
        ArgonProject newProject = ArgonProject.Create(new FileInfo(Path.Combine(newDirectory.FullName, _projectNameText.Text) + ArgonFileExtensions.Project));
        _solution.AddProject(newProject);

        GetParentWindow(this)?.Close();

        _finished(newProject);
    }

    private void PopulateProjectTemplates()
    {
        AddProjectTemplate("Argon Project");
        AddProjectTemplate("C# Project - Console");
        AddProjectTemplate("C# Project - WPF");
    }

    private void AddProjectTemplate(string name)
    {
        _projectTemplatesPanel.Items.Add(new ListBoxItem()
        {
            Content = new TextBlock()
            {
                Text = name,
                FontSize = 30,
            }
        });
    }

    /// <summary>
    /// Shows a new window containing <see cref="ProjectCreator"/>.
    /// </summary>
    /// <param name="directory">The directory to place the project in once it is created.</param>
    /// <param name="defaultName">The default starting name for the project. ex: "NewProject".</param>
    /// <param name="finished">Gets called once the project has been created. ArgonProject: The new project that was created.</param>
    /// <param name="solution">The solution that the new project should be owned by.</param>
    public static void Show(DirectoryInfo directory, string defaultName, Action<ArgonProject> finished, ArgonSolution solution)
    {
        ProjectCreator solutionPicker = new ProjectCreator(directory, defaultName, finished, solution);
        Window window = new Window()
        {
            Title = "Argon - Solution Picker",
            Content = solutionPicker,
            Width = 1200,
            Height = 800,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
        };
        window.Show();
    }
}