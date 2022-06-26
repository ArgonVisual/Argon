using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArgonVisualSketch.Views;

namespace ArgonVisualSketch;

public class ArgonVisualSketch 
{
    [STAThread]
    public static void Main()
    {
        Application app = new Application();
        Window rootWindow = new Window() 
        {
            Title = "Argon Visual Sketch",
            Width = 1400,
            Height = 800,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SnapsToDevicePixels = true,
            Content = CreateViewLayout()
        };

        ResourceDictionary resources = new ResourceDictionary();

        Style textBlockStyle = new Style(typeof(TextBlock));
        textBlockStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.White));
        resources.Add(typeof(TextBlock), textBlockStyle);

        Style treeViewStyle = new Style(typeof(TreeView));
        treeViewStyle.Setters.Add(new Setter(TreeView.BackgroundProperty, Brushes.Transparent));
        treeViewStyle.Setters.Add(new Setter(TreeView.BorderBrushProperty, null));
        resources.Add(typeof(TreeView), treeViewStyle);

        rootWindow.Resources = resources;

        app.Run(rootWindow);
    }

    private static FrameworkElement CreateViewLayout() 
    {
        Grid grid = new Grid();

        Grid leftPanel = new Grid();
        leftPanel.AddRowAuto(new SolutionView());
        leftPanel.AddRowFill(new ProjectView());

        Grid middlePanel = new Grid();
        middlePanel.AddRowFill(new DocumentEditorView());
        middlePanel.AddRowAuto(new PropertiesView());

        Grid rightPanel = new Grid();
        rightPanel.AddRowFill(new FunctionsView());

        grid.AddColumnAuto(leftPanel);
        grid.AddColumnFill(middlePanel);
        grid.AddColumnAuto(rightPanel);
        return grid;
    }
}