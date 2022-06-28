using System;
using System.Windows;
using System.Windows.Controls;
using ArgonVisual.Helpers;
using ArgonVisual.Widgets;

namespace ArgonVisual.Views;

public class FunctionsView : ViewBase
{
    public FunctionsView(SolutionEditor solutionEditor) : base(solutionEditor)
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

    protected override string GetDefaultTitle()
    {
        return "Functions";
    }
}