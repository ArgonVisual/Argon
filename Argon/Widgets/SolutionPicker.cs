using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Argon.FileTypes;
using Argon.Helpers;
using Microsoft.Win32;

namespace Argon.Widgets;

/// <summary>
/// The widget that is used for creating a new solution or choosing from an existing one.
/// </summary>
public class SolutionPicker : ContentControl
{
    /// <summary>
    /// Initializes a new instance of <see cref="SolutionPicker"/>.
    /// </summary>
    public SolutionPicker()
    {
        TextButton createSolutionButton = new TextButton("Create new Solution")
        {
            Width = 300,
            Height = 80
        };
        createSolutionButton.Click += CreateNewSolution;

        TextButton openSolutionButton = new TextButton("Open Solution")
        {
            Width = 300,
            Height = 80
        };
        openSolutionButton.Click += ShowOpenSolutionDialog;

        Grid buttonsGrid = new Grid();

        buttonsGrid.AddColumnFill(createSolutionButton);
        buttonsGrid.AddColumnFill(openSolutionButton);

        Grid mainGrid = new Grid();

        mainGrid.AddRowAuto(new ArgonTextBlock()
        {

        });
        mainGrid.AddRowFill(buttonsGrid);

        Content = mainGrid;
    }

    private void ShowOpenSolutionDialog()
    {
        OpenFileDialog dialog = new OpenFileDialog()
        {
            Title = "Choose solution to open",
            DefaultExt = FileExtensions.Solution,
            Filter = $"Solution|*{FileExtensions.Solution}"
        };

        if (dialog.ShowDialog() ?? false)
        {
            string solutionFilename = dialog.FileName;
            ArgonSolution.ReadSolution(solutionFilename).CreateSolutionEditorWindow().Show();
            Argon.CloseSolutionPickerWindow();
        }
    }

    private void CreateNewSolution()
    {
        SaveFileDialog saveDialog = new SaveFileDialog()
        {
            Title = "Choose directory to save solution"
        };

        if (saveDialog.ShowDialog() ?? false)
        {
            ArgonSolution.CreateAndSaveBlank(Path.ChangeExtension(saveDialog.FileName, FileExtensions.Solution)).CreateSolutionEditorWindow().Show();
            Argon.CloseSolutionPickerWindow();
        }
    }

    /// <summary>
    /// Creates a window containing a <see cref="SolutionPicker"/>.
    /// </summary>
    /// <returns>The new window.</returns>
    public static Window CreateWindow() 
    {
        Window newWindow = new Window()
        {
            Title = "Argon - Open or Create Solution",
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Width = 800,
            Height = 600,
            Background = GlobalStyle.Background
        };

        newWindow.Content = new SolutionPicker();
        Argon.SolutionPickerWindow = newWindow;
        return newWindow;
    }
}