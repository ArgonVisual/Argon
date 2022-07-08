using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ArgonVisualX2.Windows;
/// <summary>
/// Interaction logic for SolutionPickerWindow.xaml
/// </summary>
public partial class SolutionPicker : Window
{
    public SolutionPicker()
    {
        InitializeComponent();

        SolutionLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    }

    private void OpenSolutionClicked(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog() 
        { 
            Title = "Open Solution",
            CheckFileExists = true,
            CheckPathExists = true,
            Multiselect = false,
            Filter = $"Solution (*{SolutionFile.Extension})|*{SolutionFile.Extension}"
        };

        if (dialog.ShowDialog() ?? false)
        {
            try
            {
                SolutionFile solution = SolutionFile.Read(new FileInfo(dialog.FileName));
                ShowEditorForSolution(solution);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Argon", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    private void OpenFolderClicked(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("This is not implemented yet.", "Argon");
    }

    private void OpenLocationBrowser(object sender, RoutedEventArgs e)
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog() 
        {
            Title = "Choose Solution Directory",
            IsFolderPicker = true,
            AddToMostRecentlyUsedList = false,
            AllowNonFileSystemItems = false,
            EnsurePathExists = true,
            EnsureReadOnly = false,
            EnsureValidNames = true,
            Multiselect = false,
            ShowPlacesList = true
        };

        if (Directory.Exists(SolutionLocation.Text))
        {
            dialog.InitialDirectory = SolutionLocation.Text;
        }

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            SolutionLocation.Text = dialog.FileName;
        }
    }

    private void CreateSolutionClicked(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SolutionName.Text))
        {
            MessageBox.Show("Solution name cannot be empty.", "Argon");
            return;
        }

        if (!Directory.Exists(SolutionLocation.Text))
        {
            MessageBox.Show("Solution location must exist.", "Argon");
            return;
        }

        try
        {
            SolutionFile solution = SolutionFile.Create(SolutionLocation.Text, SolutionName.Text);
            ShowEditorForSolution(solution);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Argon", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void ShowEditorForSolution(SolutionFile solution) 
    {
        SolutionEditor editor = new SolutionEditor(solution);
        editor.Show();
        Application.Current.MainWindow = editor;
        Close();
    }
}
