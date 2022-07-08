using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ArgonVisualX2.Windows;

namespace ArgonVisualX2;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        SolutionPickerWindow window = new SolutionPickerWindow();
        window.Show();
    }
}
