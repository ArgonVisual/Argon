using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static ArgonVisualSketch.WidgetHelper;

namespace ArgonVisualSketch;

public abstract class ViewBase : Border
{
    private TextBlock _viewTitleText;

    public ViewBase() 
    {

        Background = ArgonStyle.ViewBorder;

        Grid grid = new Grid() 
        {
            Margin = new Thickness(0, 0, 3, 3)
        };

        grid.AddRowAuto(new Border() 
        {
            Background = ArgonStyle.ViewTitleBackground,
            MinWidth = 300,
            Child = _viewTitleText = new TextBlock()
            {
                Text = Getitle(),
                FontSize = 15,
                Margin = new Thickness(2)
            }
        });

        grid.AddRowFill(new Border()
        {
            Background = ArgonStyle.ViewBodyBackground,
            Child = GetBodyContent(),
            MinHeight = 80
        });

        Child = grid;
    }

    protected abstract FrameworkElement GetBodyContent();

    protected abstract string Getitle();

    public void RefreshTitle() 
    {
        
    }
}