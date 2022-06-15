using System;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Argon.FileTypes;
using Argon.Helpers;

namespace Argon.Widgets;

public class CodeFileEditor : ContentControl
{
    private FileItemsSelector _classesViewer;
    private CodeFileGraphEditor _graphEditor;

    private ClassMembersSelector _classMembersView;
    private ClassProperties _classPropertiesView;

    private ArgonTextBlock _filenameText;

    public SolutionEditor Editor;

    /// <summary>
    /// The current file that is being edited.
    /// </summary>
    public ArgonCodeFile? CurrentFile { get; private set; }

    public CodeFileEditor(SolutionEditor editor) 
    {
        Editor = editor;

        Grid mainGrid = new Grid();

        Grid classItemsPanel = new Grid();
        classItemsPanel.AddRowAuto(_filenameText = new ArgonTextBlock() 
        {
            Text = "Select a file"
        });
        classItemsPanel.AddRowFill(_classesViewer = new FileItemsSelector(this));


        Grid farRightPanel = new Grid();

        farRightPanel.AddRowPixel(100, _classPropertiesView = new ClassProperties());
        farRightPanel.AddRowFill(_graphEditor = new CodeFileGraphEditor());


        mainGrid.AddColumnPixel(200, classItemsPanel);
        mainGrid.AddColumnPixel(200, _classMembersView = new ClassMembersSelector(this));
        mainGrid.AddColumnFill(farRightPanel);

        Content = mainGrid;

        Application.Current.Exit += SaveFileBeforeExit;
    }

    private void SaveFileBeforeExit(object sender, ExitEventArgs e)
    {
        if (CurrentFile is not null)
        {
            CurrentFile.Save();
        }
    }

    public void ShowFile(ArgonCodeFile codeFile)
    {
        if (CurrentFile is not null)
        {
            CurrentFile.Save();
        }

        CurrentFile = codeFile;
        _filenameText.Text = codeFile.Name;
        _classesViewer.ShowFile(codeFile);
    }

    public void ShowClass(ArgonClass argonClass) 
    {
        _classPropertiesView.ShowClass(argonClass);
    }
}

public class CodeFileGraphEditor : ContentControl 
{
    public CodeFileGraphEditor() 
    {
        Content = new ArgonTextBlock()
        {
            Text = "This is the graph"
        };
    }
}

/// <summary>
/// Shows detialed information about a class, appears ontop of the graph editor
/// </summary>
public class ClassProperties : ContentControl
{
    ArgonTextBlock _nameText;

    public ClassProperties() 
    {
        _nameText = new ArgonTextBlock()
        {
            Text = "Select a Class",
            FontSize = 30,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        Content = _nameText;
    }

    public void ShowClass(ArgonClass argonClass) 
    {
        _nameText.Text = argonClass.Name;
    }
}

/// <summary>
/// Shows the functions and properties in a class
/// </summary>
public class ClassMembersSelector : Border
{
    private CodeFileEditor _codeFileEditor;

    private ListBox _functions;
    private ListBox _properties;

    public ClassMembersSelector(CodeFileEditor fileEditor)
    {
        _codeFileEditor = fileEditor;

        Background = GlobalStyle.Transparent;
        BorderBrush = GlobalStyle.Border;
        BorderThickness = new Thickness(2);

        Margin = new Thickness(4);

        _functions = new ListBox()
        {
            Background = null,
        };
        _properties = new ListBox()
        {
            Background = null,
        };

        ContextMenu functionsContextMenu = new ContextMenu();
        MenuItem addFunctionItem = new MenuItem() 
        {
            Header = "Add Function"
        };
        addFunctionItem.Click += AddFunction;
        functionsContextMenu.Items.Add(addFunctionItem);


        ContextMenu propertiesContextMenu = new ContextMenu();
        MenuItem addPropertyItem = new MenuItem()
        {
            Header = "Add Property"
        };
        addPropertyItem.Click += AddProperty;
        propertiesContextMenu.Items.Add(addPropertyItem);

        _functions.ContextMenu = functionsContextMenu;
        _properties.ContextMenu = propertiesContextMenu;

        Grid mainGrid = new Grid();

        mainGrid.AddRowFill(_functions);
        mainGrid.AddRowFill(_properties);

        Child = mainGrid;
    }

    private void AddProperty(object sender, RoutedEventArgs e)
    {

    }

    private void AddFunction(object sender, RoutedEventArgs e)
    {

    }
}

/// <summary>
/// Shows all classes inside of a file
/// </summary>
public class FileItemsSelector : ContentControl
{
    private ListBox _innerPanel;
    public CodeFileEditor CodeFileEditor { get; }

    public FileItemsSelector(CodeFileEditor fileEditor) 
    {
        CodeFileEditor = fileEditor;

        _innerPanel = new ListBox()
        {
            Background = null,
            BorderBrush = GlobalStyle.Border,
            BorderThickness = new Thickness(2),
            Margin = new Thickness(5)
        };

        Content = _innerPanel;

        ContextMenu contextMenu = new ContextMenu();

        MenuItem addContainerItem = new MenuItem() 
        {
            Header = "Add Container"
        };
        addContainerItem.Click += AddContainer;
        contextMenu.Items.Add(addContainerItem);

        ContextMenu = contextMenu;

        _innerPanel.SelectionChanged += (sender, args) =>
        {
            if (_innerPanel.SelectedItem is ClassBoxItem boxItem)
            {
                CodeFileEditor.ShowClass(boxItem.Class);
            }
        };
    }

    private void AddContainer(object sender, RoutedEventArgs e)
    {
        if (CodeFileEditor.CurrentFile is not null)
        {
            ArgonClass codeContainer = new ArgonClass("NewContainer");
            CodeFileEditor.CurrentFile.Containers.Add(codeContainer);
            ClassBoxItem containerWidget = new ClassBoxItem(this, codeContainer);
            containerWidget.Rename();
            _innerPanel.Items.Add(containerWidget);
        }
    }

    public void ShowFile(ArgonCodeFile codeFile) 
    {
        _innerPanel.Items.Clear();

        foreach (ArgonClass container in codeFile.Containers)
        {
            _innerPanel.Items.Add(new ClassBoxItem(this, container));
        }
    }

    public void DeleteItem(ClassBoxItem item) 
    {
        if (MessageBox.Show("Are you sure you want to delete this item?", "Argon Editor", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            if (CodeFileEditor.CurrentFile is not null)
            {
                _innerPanel.Items.Remove(item);
                CodeFileEditor.CurrentFile.Containers.Remove(item.Class);
            }
        }
    }
}

public class ClassBoxItem : ListBoxItem
{
    public ArgonClass Class { get; }
    private FileItemsSelector _containersViewer;
    private InlineEditableTextBox _nameText;

    public ClassBoxItem(FileItemsSelector containersViewer, ArgonClass container) 
    {
        Class = container;
        _containersViewer = containersViewer;

        Content = _nameText = new InlineEditableTextBox()
        {
            Text = container.Name
        };

        ContextMenu contextMenu = new ContextMenu();

        MenuItem deleteItem = new MenuItem() 
        {
            Header = "Delete"
        };
        deleteItem.Click += DeleteItem;
        contextMenu.Items.Add(deleteItem);

        MenuItem renameItem = new MenuItem()
        {
            Header = "Rename"
        };
        renameItem.Click += RenameItemClicked;
        contextMenu.Items.Add(renameItem);

        ContextMenu = contextMenu;
    }

    private void RenameItemClicked(object sender, RoutedEventArgs e)
    {
        Rename();
    }

    private void DeleteItem(object sender, RoutedEventArgs e)
    {
        _containersViewer.DeleteItem(this);
    }

    public void Rename() 
    {
        _nameText.EnterEditMode();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _nameText.ExitEditMode();
            Class.Name = _nameText.Text;
            _containersViewer.CodeFileEditor.CurrentFile?.Save();
        }
    }
}