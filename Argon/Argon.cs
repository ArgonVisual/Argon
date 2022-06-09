using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Argon;

/// <summary>
/// Contains the main function to startup the app
/// </summary>
public class Argon
{
    public static void Main() 
    {
        // Create the Application
        Application app = new Application();
        // The main window for the app
        Window mainWindow = CreateOpenOrCreateProject();
        // Start the main loop and show the window
        app.Run(mainWindow);
    }

    /// <summary>
    /// Creates a window for editing a solution.
    /// </summary>
    /// <param name="solution">The solution that this window should be editing.</param>
    /// <returns>The created window.</returns>
    public static Window CreateEditorWindow(ArgonSolution solution) 
    {
        Window newWindow = new Window();

        return newWindow;
    }

    /// <summary>
    /// Creates a window that lets the user choose from an existing project or create a new one from a template.
    /// </summary>
    /// <returns>The created window.</returns>
    public static Window CreateOpenOrCreateProject() 
    {
        Window newWindow = new Window();

        return newWindow;
    }
}
