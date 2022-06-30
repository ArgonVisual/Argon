using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArgonVisual.Helpers;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ArgonVisual.Widgets;

/// <summary>
/// Manages the opening and creation of solutions.
/// </summary>
public class SolutionPicker : ContentControl
{
    private class SolutionCreator : Grid
    {
        private Action _cancelClicked;

        private NameTextBox _solutionDirectoryText;
        private NameTextBox _solutionNameText;

        public SolutionCreator(Action cancelClicked)
        {
            Margin = new Thickness(40);
            _cancelClicked = cancelClicked;

            Grid bottomPanel = new Grid()
            {
                VerticalAlignment = VerticalAlignment.Bottom
            };

            TextButton createButton = new TextButton("Create Solution");
            createButton.Click += AskUserForStartingProjectAndCreateSolution;
            TextButton cancelButton = new TextButton("Cancel");
            cancelButton.Click += (sender, args) => _cancelClicked();

            bottomPanel.AddColumnFill(createButton);
            bottomPanel.AddColumnFill(cancelButton);

            _solutionNameText = new NameTextBox("Solution Name")
            {
                Text = "MySolution"
            };

            Grid solutionDirectoryPanel = new Grid();

            TextButton chooseSolutionDirectoryButton = new TextButton("Choose");
            chooseSolutionDirectoryButton.Click += ChooseDirectoryForSolution;

            _solutionDirectoryText = new NameTextBox("Solution Directory")
            {
                Text = $@"C:\Users\{Environment.UserName}\Desktop"
            };

            solutionDirectoryPanel.AddColumnFill(_solutionDirectoryText);
            solutionDirectoryPanel.AddColumnAuto(chooseSolutionDirectoryButton);

            this.AddRowAuto(_solutionNameText);
            this.AddRowAuto(solutionDirectoryPanel);
            this.AddRowFill(bottomPanel);
        }

        private void ChooseDirectoryForSolution(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog();
            fileDialog.Title = "Choose ";
            fileDialog.IsFolderPicker = true;

            fileDialog.AddToMostRecentlyUsedList = false;
            fileDialog.AllowNonFileSystemItems = false;
            fileDialog.EnsureFileExists = true;
            fileDialog.EnsurePathExists = true;
            fileDialog.EnsureReadOnly = false;
            fileDialog.EnsureValidNames = true;
            fileDialog.Multiselect = false;
            fileDialog.ShowPlacesList = true;

            if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folder = fileDialog.FileName;
                _solutionDirectoryText.Text = folder;
            }
        }

        private bool ValidateSolutionOptions([NotNullWhen(false)] out string? message)
        {
            message = null;

            if (!Directory.Exists(_solutionDirectoryText.Text))
            {
                message = "Solution directory must exist";
                return false;
            }

            return true;
        }

        private void AskUserForStartingProjectAndCreateSolution(object sender, RoutedEventArgs e)
        {
            if (!ValidateSolutionOptions(out string message))
            {
                MessageBox.Show(message, "Argon");
                return;
            }

            string name = _solutionNameText.Text;
            DirectoryInfo directory = new DirectoryInfo(_solutionDirectoryText.Text).CreateSubdirectory(name);

            // Create the new solution
            ArgonSolution newSolution = ArgonSolution.Create(directory, name);

            // Ask the user to create a starting project
            // to be placed into the solution
            // Becuase a solution cannot have 0 projects
            ProjectCreator.Show(directory, name, (ArgonProject newProject) => CloseSolutionPicker(this, newSolution), newSolution);
        }
    }

    private static void CloseSolutionPicker(FrameworkElement caller, ArgonSolution? solutionToOpen = null)
    {
        if (solutionToOpen is not null)
        {
            SolutionEditor.Show(solutionToOpen);
        }

        WidgetHelper.GetParentWindow(caller)?.Close();
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SolutionPicker"/>.
    /// </summary>
    public SolutionPicker()
    {
        Content = CreateOpenOrCreateButtons();
    }

    private Grid CreateOpenOrCreateButtons()
    {
        Grid grid = new Grid();

        grid.AddColumnFill(CreateButton("Create new Solution", ShowSolutionCreator));
        grid.AddColumnFill(CreateButton("Open Solution", OpenSolution));

        return grid;
    }

    private void OpenSolution()
    {
        OpenFileDialog openDialog = new OpenFileDialog()
        {
            Title = "Open Solution",
            Filter = $"Solution|*{ArgonFileExtensions.Soluion}"
        };

        if (openDialog.ShowDialog() ?? false)
        {
            CloseSolutionPicker(this, ArgonSolution.Read(new FileInfo(openDialog.FileName)));
        }
    }

    private void ShowSolutionCreator()
    {
        Content = new SolutionCreator(ShowButtons);
    }

    private void ShowButtons()
    {
        Content = CreateOpenOrCreateButtons();
    }

    private Button CreateButton(string text, Action clickAction)
    {
        Button button = new Button()
        {
            Padding = new Thickness(10),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Content = new TextBlock()
            {
                Text = text,
                Foreground = Brushes.Black,
                FontSize = 40
            }
        };

        button.Click += (sender, args) => clickAction();

        return button;
    }

    /// <summary>
    /// Shows <see cref="SolutionPicker"/> in a new window.
    /// </summary>
    public static void Show()
    {
        SolutionPicker solutionPicker = new SolutionPicker();
        Window window = new Window()
        {
            Title = "Argon - Solution Picker",
            Content = solutionPicker,
            Width = 900,
            Height = 600,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
        };
        window.Show();
    }
}