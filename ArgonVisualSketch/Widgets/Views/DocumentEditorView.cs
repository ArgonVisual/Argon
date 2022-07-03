using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArgonVisual.DocumentItems;
using ArgonVisual.Helpers;
using ArgonVisual.Widgets;

namespace ArgonVisual.Views;

/// <summary>
/// View that shows the graph panel and a tab bar.
/// </summary>
public class DocumentEditorView : ViewBase
{
    public ArgonCodeFile? ShownCodeFile { get; private set; }

    private Grid _grid;
    private TextBlock _titleText;

    private StackPanel _definedTypesPanel;

    public DocumentEditorView(SolutionEditor solutionEditor) : base(solutionEditor)
    {
        _grid = new Grid();

        Background = BrushHelper.MakeSolidBrush(30, 30, 34);
        _definedTypesPanel = new StackPanel()
        {
            Background = Brushes.Transparent,
            Orientation = Orientation.Horizontal,
            MinHeight = 50
        };

        ContextMenu contextMenu = new ContextMenu();

        contextMenu.Items.Add(new TextMenuItem("Add New Type", AddNewType));

        _definedTypesPanel.ContextMenu = contextMenu;

        _grid.AddRowAuto(_titleText = new TextBlock()
        {
            Text = "Selected file name",
            FontSize = 15,
            HorizontalAlignment = HorizontalAlignment.Center
        });
        _grid.AddRowAuto(new ScrollViewer() 
        {
            Content = _definedTypesPanel,
            VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
        });
        _grid.AddRowFill(new GraphPanel() { Margin = new Thickness(5) });
    }

    private void AddNewType()
    {
        if (ShownCodeFile is not null)
        {
            ArgonUserDefinedType definedType = new ArgonUserDefinedType("MyClass");
            ShownCodeFile.DefinedTypes.Add(definedType);
            _definedTypesPanel.Children.Add(new UserDefinedTypePreview(definedType));
        }
    }

    protected override FrameworkElement GetBodyContent()
    {
        return _grid;
    }

    protected override string GetDefaultTitle()
    {
        return "Document Editor";
    }

    public void ShowCodeFile(ArgonCodeFile? codeFile) 
    {
        if (ShownCodeFile is not null)
        {
            ArgonCodeFile.Save(ShownCodeFile);
        }

        ShownCodeFile = codeFile;

        _definedTypesPanel.Children.Clear();

        if (codeFile is not null)
        {
            _titleText.Text = codeFile.Name;
            foreach (ArgonUserDefinedType definedType in codeFile.DefinedTypes)
            {
                _definedTypesPanel.Children.Add(new UserDefinedTypePreview(definedType));
            }
        }
        else
        {
            _titleText.Text = string.Empty;
        }
    }
}