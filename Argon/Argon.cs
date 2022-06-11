using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Argon.FileTypes;
using Argon.Helpers;
using Argon.Widgets;

namespace Argon;

/// <summary>
/// Contains the main function to startup and manage the lifetime of the app.
/// </summary>
public class Argon
{
    /// <summary>
    /// The window that is used for creating and opening an Argon solution.
    /// </summary>
    public static Window? SolutionPickerWindow { get; set; }

    /// <summary>
    /// The project creator window that is being shown.
    /// </summary>
    public static Window? ProjectCreatorWindow { get; set; }

    /// <summary>
    /// Keeps track of items that need to be saved to disk.
    /// </summary>
    private static List<ISaveable> _filesNeedingSave = new List<ISaveable>();

    [STAThread]
    public static void Main(string[] args) 
    {
        string? commandLine = args.Length > 0 ? args[0] : null;

        if (Debugger.IsAttached)
        {
            // Do not wrap in a tru-catch block so any errors that happen will be
            // sent straigth to the debugger to catch and show
            GuardedMain(commandLine);
        }
        else
        {
            try
            {
                GuardedMain(commandLine);
            }
            catch (Exception e)
            {
                // Show the error in a message box
                MessageBox.Show(e.ToString());
            }
        }
    }

    private static void GuardedMain(string? commandLine) 
    {
#if DEBUG
        // Set the commandline to this project for debugging
        // commandLine = $@"C:\Users\{Environment.UserName}\Desktop\UnrealArgon\UnrealEngine.argsln";
#endif

        // Create the Application
        Application app = new Application();
        Window mainWindow;

        // Check if the first argument contains
        // the filename of a solution to open
        if (commandLine is not null)
        {
            // Create window for editing the solution
            string filename = commandLine;
            mainWindow = SolutionEditor.CreateWindow(ArgonSolution.ReadSolution(filename));
        }
        else
        {
            // Create the solution picker
            // From here the user can either create a new solution
            // or open an existin one
            mainWindow = SolutionPicker.CreateWindow();
        }

        // Start the main loop and show the window
        app.Run(mainWindow);
    }

    /// <summary>
    /// Closes the solution picker window if it is valid.
    /// </summary>
    public static void CloseSolutionPickerWindow() 
    {
        if (SolutionPickerWindow is not null)
        {
            SolutionPickerWindow.Close();
            SolutionPickerWindow = null;
        }
    }

    /// <summary>
    /// Saves all unsaved items.
    /// </summary>
    public static void SaveAllItems() 
    {
        foreach (ISaveable item in _filesNeedingSave)
        {
            item.Save();
        }
        _filesNeedingSave.Clear();
    }

    /// <summary>
    /// Marks a file for saving.
    /// </summary>
    /// <param name="file">The file that needs to be saved</param>
    public static void MarkFileForSave(IFileHandle file) 
    {
        if (!_filesNeedingSave.Contains(file))
        {
            _filesNeedingSave.Add(file);
        }
    }
}
