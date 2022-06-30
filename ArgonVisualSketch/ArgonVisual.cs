using System;
using System.Diagnostics;
using System.Windows;
using ArgonVisual.Widgets;

namespace ArgonVisual;

/// <summary>
/// Contains the startup for the application.
/// </summary>
public class ArgonVisual 
{
    [STAThread]
    public static void Main()
    {
        if (Debugger.IsAttached)
        {
            GuardedMain();
        }
        else
        {
            try
            {
                GuardedMain();
            }
            catch (Exception e)
            {
                ReportCrash(e);
            }
        }
    }

    private static void GuardedMain() 
    {
        Application app = new Application();
        ArgonStyle.Initialize(app.Resources);
        SolutionPicker.Show();
        app.Run();
    }

    private static void ReportCrash(Exception exception) 
    {
        MessageBox.Show(exception.ToString());
    }
}