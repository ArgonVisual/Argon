using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ArgonVisual.Helpers;
using ArgonVisual.Views;

namespace ArgonVisual.Widgets;

/// <summary>
/// Widget for editing the contents of a solution.
/// </summary>
public class SolutionEditor : Grid
{
    /// <summary>
    /// The <see cref="ArgonSolution"/> that this is editing.
    /// </summary>
    public ArgonSolution Solution { get; }

    private List<ViewBase> _views;

    /// <summary>
    /// Initializes a new instance of <see cref="SolutionEditor"/> with the solution to edit.
    /// </summary>
    /// <param name="solution">The solution to edit.</param>
    private SolutionEditor(ArgonSolution solution)
    {
        Solution = solution;
        _views = new List<ViewBase>();

        Grid leftPanel = new Grid();
        leftPanel.AddRowAuto(AddView(new SolutionView(this)));
        leftPanel.AddRowFill(AddView(new ProjectView(this)));

        Grid middlePanel = new Grid();
        middlePanel.AddRowFill(AddView(new DocumentEditorView(this)));
        middlePanel.AddRowAuto(AddView(new PropertiesView(this)));

        Grid rightPanel = new Grid();
        rightPanel.AddRowFill(AddView(new FunctionsView(this)));

        this.AddColumnAuto(leftPanel);
        this.AddColumnFill(middlePanel);
        this.AddColumnPixel(350, rightPanel);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ViewBase AddView(ViewBase view) 
    {
        _views.Add(view);
        return view;
    }

    /// <summary>
    /// Finds the first instance of type <typeparamref name="T"/> in this editor.
    /// </summary>
    /// <typeparam name="T">The type of view to find.</typeparam>
    /// <returns>The found view. Null if not found.</returns>
    public T? FindView<T>() where T : ViewBase
    {
        return _views.FirstOrDefault((view) => view is T) as T;
    }

    /// <summary>
    /// Shows a new window containing <see cref="SolutionEditor"/> with the solution to edit.
    /// </summary>
    /// <param name="solution">The soution to edit.</param>
    public static void Show(ArgonSolution solution)
    {
        Window window = new Window()
        {
            Title = $"Argon - {solution.Name}",
            Width = 1300,
            Height = 800,
            Content = new SolutionEditor(solution)
        };
        window.Show();
    }
}