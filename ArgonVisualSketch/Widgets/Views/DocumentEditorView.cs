using System.Windows;
using System.Windows.Controls;
using ArgonVisual.Widgets;

namespace ArgonVisual.Views;

/// <summary>
/// View that shows the graph panel and a tab bar.
/// </summary>
public class DocumentEditorView : ViewBase
{
    public DocumentEditorView(SolutionEditor solutionEditor) : base(solutionEditor)
    {

    }

    protected override FrameworkElement GetBodyContent()
    {
        TabControl tabControl = new TabControl()
        { 
            Padding = new Thickness(0),
            BorderBrush = null
        };

        tabControl.Items.Add(new DocumentTabItem("MyFile"));
        tabControl.Items.Add(new DocumentTabItem("MyOtherFile"));
        tabControl.Items.Add(new DocumentTabItem("MySpecialFile"));

        return tabControl;
    }

    protected override string GetDefaultTitle()
    {
        return "Document Editor";
    }
}