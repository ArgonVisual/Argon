using System.Windows;
using System.Windows.Controls;

namespace ArgonVisual.Views;

public class FunctionsView : ViewBase
{
    public FunctionsView() 
    {
        
    }

    protected override FrameworkElement GetBodyContent()
    {
        Grid grid = new Grid();
        WrapPanel functionsPanel = new WrapPanel();

        functionsPanel.Children.Add(new FunctionPreview() { IsSelected = true });
        functionsPanel.Children.Add(new FunctionPreview());
        functionsPanel.Children.Add(new FunctionPreview());

        grid.AddRowFill(functionsPanel);

        return grid;
    }

    protected override string Getitle()
    {
        return "Functions";
    }
}