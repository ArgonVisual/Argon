using System.Windows;
using System.Windows.Controls;
using ArgonVisualSketch.TreeItems;

namespace ArgonVisualSketch.Views;

public class DocumentEditorView : ViewBase
{
    public DocumentEditorView() 
    {
        
    }

    protected override FrameworkElement GetBodyContent()
    {
        return new FrameworkElement();
    }

    protected override string Getitle()
    {
        return "Document Editor";
    }
}