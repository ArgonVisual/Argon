using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArgonVisual.DocumentItems;
using ArgonVisual.Helpers;
using ArgonVisual.Widgets;

namespace ArgonVisual.Views;

/// <summary>
/// Shows functions inside of the selected class or struct
/// </summary>
public class FunctionsView : ViewBase
{
    /// <summary>
    /// The current class that functions are being shown for
    /// </summary>
    public ArgonClass? ShownClass { get; private set; }

    private StackPanel _functionsPanel;

    public FunctionPreview? SelectedFunctionPreview { get; set; }

    public FunctionsView(SolutionEditor solutionEditor) : base(solutionEditor)
    {
        _functionsPanel = new StackPanel();
    }

    protected override FrameworkElement GetBodyContent()
    {
        Grid grid = new Grid()
        {
            Background = Brushes.Transparent
        };
        // TODO: place buttons above panel to sort from: public, protected, private and user defined categories

        ContextMenu contextMenu = new ContextMenu();
        contextMenu.Items.Add(new TextMenuItem("Add New Function", AddNewFunction));

        grid.ContextMenu = contextMenu;

        grid.AddRowFill(_functionsPanel);

        return grid;
    }

    private void AddNewFunction()
    {
        if (ShownClass is not null)
        {
            ArgonFunction newFunction = new ArgonFunction("NewFunction");
            ShownClass.Functions.Add(newFunction);
            FunctionPreview functionPreview = new FunctionPreview(newFunction, this);
            _functionsPanel.Children.Add(functionPreview);
            functionPreview.Rename();
        }
    }

    public void ShowFunctionsForClass(ArgonClass? argonClass) 
    {
        ShownClass = argonClass;

        _functionsPanel.Children.Clear();

        if (argonClass is not null)
        {
            for (int i = 0; i < argonClass.Functions.Count; i++)
            {
                FunctionPreview functionPreview = new FunctionPreview(argonClass.Functions[i], this);
                if (i == 0)
                {
                    functionPreview.Select();
                }
                _functionsPanel.Children.Add(functionPreview);
            }
        }
    }

    protected override string GetDefaultTitle()
    {
        return "Functions";
    }
}