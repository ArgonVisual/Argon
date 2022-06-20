using System;
using System.Windows;
using ArgonVertical.Static;

namespace ArgonVertical;

public static class ArgonVertical
{
    [STAThread]
    public static void Main() 
    {
        Application app = new Application();
        Window rootWindow = new Window() 
        {
            Title = "Argon Vertical",
            Content = new GraphPanel()
        };
        app.Run(rootWindow);
    }
}