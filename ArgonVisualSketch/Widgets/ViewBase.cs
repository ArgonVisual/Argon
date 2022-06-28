using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArgonVisual.Helpers;
using ArgonVisual.Widgets;
using static ArgonVisual.Helpers.WidgetHelper;

namespace ArgonVisual;

public abstract class ViewBase : Border
{
    private TextBlock _viewTitleText;

    /// <summary>
    /// The <see cref="SolutionEditor"/> that owns this.
    /// </summary>
    public SolutionEditor Editor { get; }

    private Border _contentBorder;

    public string Title 
    {
        get => _viewTitleText.Text;
        set => _viewTitleText.Text = value;
    }

    public ViewBase(SolutionEditor solutionEditor) 
    {
        Editor = solutionEditor;
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
                Text = GetDefaultTitle(),
                FontSize = 15,
                Margin = new Thickness(2)
            }
        });

        grid.AddRowFill(_contentBorder = new Border()
        {
            Background = ArgonStyle.Background,
            MinHeight = 80
        });

        Loaded += GenerateContent;

        Child = grid;
    }

    private void GenerateContent(object sender, RoutedEventArgs e)
    {
        _contentBorder.Child = GetBodyContent();
    }

    protected abstract FrameworkElement GetBodyContent();

    protected abstract string GetDefaultTitle();
}