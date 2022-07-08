using System;
using System.Collections.Generic;
using System.Windows.Controls;
using ArgonVisualX2.Windows;

namespace ArgonVisualX2.Views;
/// <summary>
/// Interaction logic for SolutionView.xaml
/// </summary>
public partial class SolutionView : UserControl
{
    public static SolutionView Global => SolutionEditor.Global?.SolutionView ?? throw new NullReferenceException("SolutionView has not been instanced.");

    public string SolutionName { get; set; }

    public List<ArgonTreeItem> TreeItems { get; }

    public SolutionView()
    {
        InitializeComponent();
        this.DataContext = this;

        TreeItems = new List<ArgonTreeItem>();
    }

    public void ShowSolution(SolutionFile solution) 
    {
        SolutionName = solution.Name;

        foreach (ProjectFile project in solution.Projects)
        {
            TreeItems.Add(new ArgonProjectTreeItem(project));
        }
    }

    private void TreeItemSelectionChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is ArgonTreeItem argonTreeItem)
        {
            argonTreeItem.Select();
        }
    }

    private void SolutionItemSelected(object sender, System.Windows.RoutedEventArgs e)
    {
        if (SolutionEditor.Global?.ProjectView is not null)
        {
            SolutionEditor.Global?.ProjectView.ShowProject(null);
        }
    }
}