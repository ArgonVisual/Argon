using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArgonVisual.Helpers;
using ArgonVisual.Widgets;
using static ArgonVisual.Helpers.WidgetHelper;

namespace ArgonVisual;

/// <summary>
/// Base class for a view.
/// </summary>
public abstract class ViewBase : Border
{
    private TextBlock _viewTitleText;

    /// <summary>
    /// The <see cref="SolutionEditor"/> that owns this.
    /// </summary>
    public SolutionEditor Editor { get; }

    private Border _contentBorder;

    /// <summary>
    /// The title of the view.
    /// </summary>
    public string Title 
    {
        get => _viewTitleText.Text;
        set => _viewTitleText.Text = value;
    }

    /// <summary>
    /// The content that this view displays.
    /// </summary>
    public UIElement ViewContent 
    {
        get => _contentBorder.Child;
        set => _contentBorder.Child = value;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ViewBase"/>.
    /// </summary>
    /// <param name="solutionEditor">The solution editor that owns this view.</param>
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

    /// <summary>
    /// Gets the initial content for this view.
    /// This can be changed later using <see cref="ViewContent"/>.
    /// </summary>
    /// <returns>The initial content.</returns>
    protected abstract FrameworkElement GetBodyContent();

    /// <summary>
    /// Gets the initial title for the view.
    /// This can be changed later using <see cref="Title"/>.
    /// </summary>
    /// <returns>The initial title.</returns>
    protected abstract string GetDefaultTitle();
}