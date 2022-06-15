using System;
using System.Windows;

namespace ArgonUserInterfacePrototype;

public class Argon
{
    [STAThread]
    public static void Main() 
    {
        Application app = new Application();
        Window rootWindow = new Window() 
        {
            Title = "Argon Editor Prototype",
            Content = new SolutionEditor()
        };
        app.Run(rootWindow);
    }
}