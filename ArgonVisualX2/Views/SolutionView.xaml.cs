using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ArgonVisualX2.Windows;

namespace ArgonVisualX2.Views;
/// <summary>
/// Interaction logic for SolutionView.xaml
/// </summary>
public partial class SolutionView : UserControl
{
    public static SolutionView Global => SolutionEditor.Global?.SolutionView ?? throw new NullReferenceException("SolutionView has not been instanced.");

    public string SolutionName { get; set; }

    public object SelectedItem => ((TreeView)(ViewControl.ViewContent)).SelectedItem;

    public SolutionView()
    {
        InitializeComponent();
        this.DataContext = this;

        SolutionName = string.Empty;

        ArgonSolutionFolderTreeItem folder = new ArgonSolutionFolderTreeItem("CoreFolder");
        ArgonSolutionFolderTreeItem subFolder = new ArgonSolutionFolderTreeItem("ContentFolder");

        folder.Items.Add(subFolder);

        SolutionTreeView.Items.Add(folder);
    }

#if false

    public void ShowSolution(SolutionFile solution) 
    {
        SolutionName = solution.Name;
    
        foreach (ProjectFile project in solution.Projects)
        {
            TreeItems.Add(new ArgonProjectTreeItem(project));
        }
    }
    
    private void TreeItemSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is ArgonTreeItem argonTreeItem)
        {
            argonTreeItem.Select();
        }
    }
    
    private void SolutionItemSelected(object sender, RoutedEventArgs e)
    {
        if (SolutionEditor.Global?.ProjectView is not null)
        {
            SolutionEditor.Global?.ProjectView.ShowProject(null);
        }
    
    private void SolutionItemClicked(object sender, MouseButtonEventArgs e)
    {
        if (sender is TreeViewItem treeItem)
        {
            treeItem.IsSelected = true;
        }
    }

#endif
}