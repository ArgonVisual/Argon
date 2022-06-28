using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ArgonVisual.Views;
using ArgonVisual.Widgets;

namespace ArgonVisual;

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