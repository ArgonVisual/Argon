using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ArgonVisualX2.Windows;
/// <summary>
/// Interaction logic for SolutionEditorWindow.xaml
/// </summary>
public partial class SolutionEditorWindow : Window
{
    private static SolutionEditorWindow? _global;
    public static SolutionEditorWindow Global => _global ?? throw new NullReferenceException("SolutionEditorWindow has not been instanced.");

    public SolutionEditorWindow()
    {
        InitializeComponent();
        _global = this;
    }
}
