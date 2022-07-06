using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ArgonVisual.Views;

namespace ArgonVisual.Widgets;

public class FunctionPreview : Border
{
    public bool IsSelected
    {
        get => FunctionsView.SelectedFunctionPreview == this;
    }

    private static Brush _normalBackground = BrushHelper.MakeSolidBrush(40, 40, 45);
    private static Brush _hoverBackground = BrushHelper.MakeSolidBrush(60, 60, 65);
    private static Brush _selectedBackground = BrushHelper.MakeSolidBrush(80, 80, 85);

    private FunctionNameEditableText _nameText;

    /// <summary>
    /// The function that this widget is representing.
    /// </summary>
    public ArgonFunction Function { get; }

    /// <summary>
    /// The functions view that manages selecting
    /// </summary>
    public FunctionsView FunctionsView { get; }

    public FunctionPreview(ArgonFunction function, FunctionsView functionsView)
    {
        Function = function;
        FunctionsView = functionsView;

        Background = _normalBackground;
        HorizontalAlignment = HorizontalAlignment.Left;

        Margin = new Thickness(15, 16, 6, 0);

        CornerRadius = new CornerRadius(12);

        ContextMenu contextMenu = new ContextMenu();
        contextMenu.Items.Add(new TextMenuItem("Rename", Rename));
        ContextMenu = contextMenu;

        StackPanel mainPanel = new StackPanel();

        StackPanel topPanel = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(-10, -10, -10, 0)
        };

        topPanel.Children.Add(new Ellipse()
        {
            Fill = BrushHelper.MakeSolidBrush(72, 162, 62),
            Width = 15,
            Height = 15,
            Margin = new Thickness(3, -5, 3, 0)
        });

        topPanel.Children.Add(new FunctionModifier("Static", BrushHelper.MakeSolidBrush(75, 145, 200)));

        topPanel.Children.Add(new FunctionModifier("Inlined", BrushHelper.MakeSolidBrush(129, 74, 170)));

        topPanel.Children.Add(new FunctionModifier("My Category", BrushHelper.MakeSolidBrush(63, 65, 153)));

        mainPanel.Children.Add(topPanel);

        mainPanel.Children.Add(_nameText = new FunctionNameEditableText()
        {
            Text = function.Name,
            VerticalAlignment = VerticalAlignment.Top,
            FontSize = 22,
            Margin = new Thickness(8, -2, 8, 6)
        });

        _nameText.TextCommitted += HandleNameChanged;

        Child = mainPanel;
    }

    private void HandleNameChanged(string oldName, string newName)
    {
        Function.Name = newName;
    }

    public void Rename()
    {
        _nameText.EnterEditMode();
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        Background = _hoverBackground;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        if (!IsSelected)
        {
            Background = _normalBackground;
        }
    }

    public void RefreshAppearance() 
    {
        if (IsSelected)
        {
            Background = _selectedBackground;
        }
        else
        {
            Background = _normalBackground;
        }
    }

    public void Select() 
    {
        // Select this functionand update its appearance.
        FunctionsView.SelectedFunctionPreview = this;
        RefreshAppearance();
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        FunctionPreview? beforeSelectedFunction = FunctionsView.SelectedFunctionPreview;

        Select();

        // Refresh the appearance of the function that was selected before
        if (beforeSelectedFunction is not null)
        {
            beforeSelectedFunction.RefreshAppearance();
        }
    }
}