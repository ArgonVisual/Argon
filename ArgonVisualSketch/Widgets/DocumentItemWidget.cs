using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System;
using ArgonVisual.DocumentItems;

namespace ArgonVisual.Widgets;
public class UserDefinedTypePreview : Border
{
    private bool _isSelected;

    private static Brush _classbackground = BrushHelper.MakeSolidBrush(34, 185, 147);
    private static Brush _classSelectedbackground = BrushHelper.MakeSolidBrush(177, 255, 225);

    // private static Brush _structBackground = BrushHelper.MakeSolidBrush(134, 193, 41);
    // private static Brush _structSelectedBackground = BrushHelper.MakeSolidBrush(208, 250, 137);

    private SimpleInlineEditableTextBox _nameText;

    public bool IsStruct;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (value)
            {
                Background = _classSelectedbackground;
                _nameText.Foreground = Brushes.Black;
            }
            else
            {
                Background = _classbackground;
                _nameText.Foreground = Brushes.White;
            }

            _isSelected = value;
        }
    }

    /// <summary>
    /// The <see cref="ArgonUserDefinedType"/> that this represents.
    /// </summary>
    public ArgonUserDefinedType UserDefinedType { get; }

    public UserDefinedTypePreview(ArgonUserDefinedType definedType, bool isStruct = false)
    {
        UserDefinedType = definedType;

        IsStruct = isStruct;

        Background = BrushHelper.MakeSolidBrush(40, 40, 45);

        MinWidth = 120;
        MinHeight = 40;

        BorderBrush = _classbackground;
        BorderThickness = new Thickness(2);
        Margin = new Thickness(5);
        CornerRadius = new CornerRadius(10);

        ContextMenu contextMenu = new ContextMenu();

        contextMenu.Items.Add(new TextMenuItem("Rename", RenameItem));

        ContextMenu = contextMenu;

        Child = _nameText = new SimpleInlineEditableTextBox()
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

    private void HandleNameChanged(string oldName, string newName)
    {
        UserDefinedType.Name = newName;
    }

    private void RenameItem()
    {
        _nameText.EnterEditMode();
    }
}