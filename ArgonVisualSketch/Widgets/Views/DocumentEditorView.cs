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

    public ArgonClassPreview? SelectedClassPreview { get; private set; }

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
            ArgonClass definedType = new ArgonClass("MyClass");
            ShownCodeFile.DefinedTypes.Add(definedType);
            _definedTypesPanel.Children.Add(new ArgonClassPreview(definedType, this));
        }
    }

    public void SelectClassPreview(ArgonClassPreview classPreview) 
    {
        if (SelectedClassPreview is not null)
        {
            SelectedClassPreview.Deselect();
        }

        SelectedClassPreview = classPreview;
        SelectedClassPreview.Select();

        Editor.FindView<FunctionsView>()?.ShowFunctionsForClass(classPreview.Class);
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
            for (int i = 0; i < codeFile.DefinedTypes.Count; i++)
            {
                ArgonClass definedType = codeFile.DefinedTypes[i];
                ArgonClassPreview definedTypePreview = new ArgonClassPreview(definedType, this);
                if (i == 0)
                {
                    SelectClassPreview(definedTypePreview);
                }
                _definedTypesPanel.Children.Add(definedTypePreview);
            }
        }
        else
        {
            _titleText.Text = string.Empty;
        }
    }
}