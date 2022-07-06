using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System;
using ArgonVisual.DocumentItems;
using System.Windows.Input;
using ArgonVisual.Views;

namespace ArgonVisual.Widgets;

public class ArgonClassPreview : Border
{
    private static Brush _classbackground = BrushHelper.MakeSolidBrush(34, 185, 147);
    private static Brush _classSelectedbackground = BrushHelper.MakeSolidBrush(177, 255, 225);

    // private static Brush _structBackground = BrushHelper.MakeSolidBrush(134, 193, 41);
    // private static Brush _structSelectedBackground = BrushHelper.MakeSolidBrush(208, 250, 137);

    private EditableText _nameText;

    private static Brush _normalBackground = BrushHelper.MakeSolidBrush(30, 30, 35);
    private static Brush _hoverBackground = BrushHelper.MakeSolidBrush(50, 50, 55);
    private static Brush _selectedBackground = BrushHelper.MakeSolidBrush(80, 80, 85);

    /// <summary>
    /// The document editor that is shoing this class preview
    /// </summary>
    public DocumentEditorView DocumentEditor { get; }

    public bool IsStruct;

    /// <summary>
    /// The <see cref="ArgonClass"/> that this is representing.
    /// </summary>
    public ArgonClass Class { get; }

    public ArgonClassPreview(ArgonClass definedType, DocumentEditorView documentEditor, bool isStruct = false)
    {
        DocumentEditor = documentEditor;
        Class = definedType;

        IsStruct = isStruct;

        Background = _normalBackground;

        MinWidth = 120;
        MinHeight = 40;

        BorderBrush = _classbackground;
        BorderThickness = new Thickness(2);
        Margin = new Thickness(5);
        CornerRadius = new CornerRadius(10);

        ContextMenu contextMenu = new ContextMenu();

        contextMenu.Items.Add(new TextMenuItem("Rename", RenameItem));

        ContextMenu = contextMenu;

        Child = _nameText = new EditableText()
        {
            Text = definedType.Name,
            FontSize = 20,
            Margin = new Thickness(5, 2, 5, 5),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            FontFamily = ArgonStyle.Fonts.Bold
        };

        _nameText.TextCommitted += HandleNameChanged;
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        if (!IsSelected())
        {
            Background = _hoverBackground;
        }
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        if (!IsSelected())
        {
            Background = _normalBackground;
        }
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        DocumentEditor.SelectClassPreview(this);
    }

    public void Select() 
    {
        Background = _selectedBackground;
    }

    public void Deselect() 
    {
        Background = _normalBackground;
    }

    public bool IsSelected() => DocumentEditor.SelectedClassPreview == this;

    private void HandleNameChanged(string oldName, string newName)
    {
        Class.Name = newName;
    }

    private void RenameItem()
    {
        _nameText.EnterEditMode();
    }
}