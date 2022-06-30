using System;
using System.Windows;
using System.Windows.Controls;
using ArgonVisual.Widgets;

namespace ArgonVisual.Views;

/// <summary>
/// The the selected classes/structs properties.
/// </summary>
public class PropertiesView : ViewBase
{
    public PropertiesView(SolutionEditor solutionEditor) : base(solutionEditor)
    {

    }

    protected override FrameworkElement GetBodyContent()
    {
        return new TextBlock()
        {
            Text = "Tab Body!"
        };
    }

    protected override string GetDefaultTitle()
    {
        return "Properties";
    }
}