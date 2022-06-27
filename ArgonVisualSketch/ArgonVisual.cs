using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ArgonVisual.Views;

namespace ArgonVisual;

public class ArgonVisual 
{
    [STAThread]
    public static void Main()
    {
        Application app = new Application();
        Window rootWindow = new Window() 
        {
            Background = Brushes.Black,
            Title = "Argon Visual",
            Width = 1400,
            Height = 800,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SnapsToDevicePixels = true,
            Content = CreateViewLayout()
        };

        ResourceDictionary resources = new ResourceDictionary();

        Style textBlockStyle = new Style(typeof(TextBlock));
        textBlockStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.White));
        textBlockStyle.Setters.Add(new Setter(TextBlock.FontFamilyProperty, ArgonStyle.Fonts.Normal));
        resources.Add(typeof(TextBlock), textBlockStyle);

        Style treeViewStyle = new Style(typeof(TreeView));
        treeViewStyle.Setters.Add(new Setter(TreeView.BackgroundProperty, Brushes.Transparent));
        treeViewStyle.Setters.Add(new Setter(TreeView.BorderBrushProperty, null));
        resources.Add(typeof(TreeView), treeViewStyle);

        SolidColorBrush selectionBrush = BrushHelper.MakeSolidBrush(34, 77, 138);
        resources.Add(SystemColors.InactiveSelectionHighlightBrushKey, selectionBrush);
        resources.Add(SystemColors.HighlightBrushKey, selectionBrush);

        rootWindow.Resources = resources;

        app.Run(rootWindow);
    }

    private static FrameworkElement CreateViewLayout() 
    {
        Grid grid = new Grid() 
        {
            Margin = new Thickness(0, 0, 0, 0)
        };

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
        grid.AddColumnPixel(350, rightPanel);
        return grid;
    }
}