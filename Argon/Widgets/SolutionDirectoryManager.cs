using System;
using System.Windows;
using System.Windows.Controls;

namespace Argon.Widgets;

/// <summary>
/// Widget for managing the directory structure of a solution
/// </summary>
public class SolutionDirectoryManager : Border
{
    /// <summary>
    /// Initializes a new instance of <see cref="SolutionDirectoryManager"/>.
    /// </summary>
    public SolutionDirectoryManager() 
    {
        BorderBrush = GlobalStyle.Border;
        BorderThickness = new Thickness(2);
        Background = GlobalStyle.Transparent;
        ContextMenu contextMenu = new ContextMenu();

        MenuItem createProjectItem = new MenuItem() 
        {
            Header = "Create New Project"
        };
        createProjectItem.Click += ShowProjectCreator;

        contextMenu.Items.Add(createProjectItem);

        ContextMenu = contextMenu;

        ArgonTreeView treeView = new ArgonTreeView();

        treeView.Items.Add(new ArgonTreeViewItem("UnrealProject"));
        treeView.Items.Add(new ArgonTreeViewItem("UnrealProject"));

        Child = treeView;
    }

    private void ShowProjectCreator(object sender, RoutedEventArgs e)
    {
        ProjectCreator.CreateWindow().ShowDialog();
    }
}