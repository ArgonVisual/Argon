using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ArgonVisualX2.Windows;

namespace ArgonVisualX2.Views;
/// <summary>
/// Interaction logic for ProjectView.xaml
/// </summary>
public partial class ProjectView : UserControl
{
    public static ProjectView Global => SolutionEditor.Global?.ProjectView ?? throw new NullReferenceException("ProjectView has not been instanced.");

    public static readonly DependencyProperty ProjectNameProperty = DependencyProperty.Register("ProjectName", typeof(string), typeof(ProjectView));

    public string ProjectName
    {
        get => (string)GetValue(ProjectNameProperty);
        set => SetValue(ProjectNameProperty, value);
    }

    public static readonly DependencyProperty TreeViewVisibilityProperty = DependencyProperty.Register("TreeViewVisibility", typeof(Visibility), typeof(ProjectView), new PropertyMetadata(Visibility.Hidden));

    public Visibility TreeViewVisibility
    {
        get => (Visibility)GetValue(TreeViewVisibilityProperty);
        set => SetValue(TreeViewVisibilityProperty, value);
    }

    public ProjectView()
    {
        InitializeComponent();
    }

    public void ShowProject(ProjectFile? project)
    {
        SolutionEditor.Global.SelectedProject = project;
        TreeViewVisibility = project is not null ? Visibility.Visible : Visibility.Hidden;
        if (project is not null)
        {
            ProjectName = project.Name;
        }
    }
}
