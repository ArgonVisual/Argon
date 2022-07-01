using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ArgonVisual.Widgets;

namespace ArgonVisual.TreeItems;

/// <summary>
/// Base class for a Argon tree item.
/// </summary>
public abstract class ArgonTreeItem : TreeViewItem
{
    public string Title 
    {
        get => _titleText.Text;
        set => _titleText.Text = value;
    }

    private SimpleInlineEditableTextBox _titleText;

    public SolutionEditor Editor;

    private string _removalText => this is ProjectTreeItem ? "Remove" : "Delete";

    public ArgonTreeItem(string title, SolutionEditor editor) 
    {
        Editor = editor;

        ContextMenu contextMenu = new ContextMenu();
        contextMenu.Items.Add(new TextMenuItem("Rename", RenameItem));
        contextMenu.Items.Add(new TextMenuItem(_removalText, DeleteItem));
        ContextMenu = contextMenu;

        StackPanel stackPanel = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(1, 1, 10, 1)
        };
        
        stackPanel.Children.Add(new Image()
        {
            Source = GetIcon(),
            Width = 20,
            Height = 20,
            Margin = new Thickness(3, 3, 6, 3)
        });

        stackPanel.Children.Add(_titleText = new SimpleInlineEditableTextBox()
        {
            Text = title,
            FontSize = 18,
            VerticalAlignment = VerticalAlignment.Center
        });

        _titleText.TextCommitted += HandleNameChanged;

        Header = stackPanel;
    }

    public void RenameItem()
    {
        _titleText.EnterEditMode();
    }

    private void DeleteItem() 
    {
        if (MessageBox.Show($"{Title} will be {_removalText}d", "Argon", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
        {
            try
            {
                DeleteItemInternal();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Argon", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    protected virtual void DeleteItemInternal() 
    {
        throw new NotImplementedException();
    }

    private void HandleNameChanged(string beforeName, string newName) 
    {
        if (!beforeName.Equals(newName, StringComparison.Ordinal))
        {
            try
            {
                string? errorMessage = IsValidName(newName);
                if (errorMessage is not null)
                {
                    MessageBox.Show(errorMessage, "Argon", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _titleText.Text = beforeName;
                }
                else
                {
                    RenameItemInternal(newName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Argon - Failed To Rename Item", MessageBoxButton.OK, MessageBoxImage.Warning);
                _titleText.Text = beforeName;
            }
        }
    }

    protected virtual void RenameItemInternal(string newName) 
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks if the name is valid
    /// </summary>
    /// <param name="name">The name to check</param>
    /// <returns>If the name is not valid then returns the error message, else returns null.</returns>
    protected virtual string? IsValidName(string name) => null;

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        IsSelected = true;
        e.Handled = true;
    }

    protected override void OnUnselected(RoutedEventArgs e)
    {
        _titleText.CommitText();
    }

    protected abstract ImageSource GetIcon();
}