using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Argon.FileTypes;
using Argon.Helpers;

namespace Argon.Widgets;

/// <summary>
/// This widget is used for editing the contents of a solution.
/// It contains a directory explorer and shows code files in a graph panel.
/// </summary>
public class SolutionEditor : Border
{
    /// <summary>
    /// The <see cref="ArgonSolution"/> that this widget is editing.
    /// </summary>
    public ArgonSolution Solution { get; }

    /// <summary>
    /// Initializes a new instance of 
    /// </summary>
    /// <param name="solution"></param>
    public SolutionEditor(ArgonSolution solution) 
    {
        Solution = solution;

        Background = GlobalStyle.Background;

        Grid mainGrid = new Grid();

        mainGrid.AddRowAuto(GenerateMenu());

        Grid horizontalPanel = new Grid() 
        {
            Margin = new Thickness(5)
        };

        Grid projectsTree = new Grid();
        projectsTree.AddRowAuto(new ArgonTextBlock() 
        {
            Text = solution.Name,
            FontSize = 40,
            Margin = new Thickness(5),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        });
        projectsTree.AddRowFill(new SolutionDirectoryManager(this));

        horizontalPanel.AddColumnPixel(300, projectsTree);

        mainGrid.AddRowFill(horizontalPanel);

        Child = mainGrid;
    }

    private Menu GenerateMenu() 
    {
        Menu menu = new Menu();

        menu.Items.Add(GenerateFileMenuItem());

        return menu;
    }

    private MenuItem GenerateFileMenuItem() 
    {
        MenuItem fileItem = new MenuItem()
        {
            Header = "File",
        };

        return fileItem;
    }

    /// <summary>
    /// Creates a window that contains the <see cref="SolutionEditor"/> widget that is bound to the <paramref name="solution"/>.
    /// </summary>
    /// <param name="solution">The solution that should be edited.</param>
    /// <returns>The new window.</returns>
    public static Window CreateWindow(ArgonSolution solution) 
    {
        Window newWindow = new Window()
        {
            Title = "Argon - Editor",
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Content = new SolutionEditor(solution),
            Width = 1400,
            Height = 800
        };

        return newWindow;
    }
}