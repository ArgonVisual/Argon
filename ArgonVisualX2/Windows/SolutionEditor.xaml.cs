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
public partial class SolutionEditor : Window
{
    private static SolutionEditor? _global;
    public static SolutionEditor Global => _global ?? throw new NullReferenceException("SolutionEditorWindow has not been instanced.");

    public SolutionFile Solution { get; }

    public ProjectFile? SelectedProject { get; set; }

    public CodeFile? SelectedCodeFile { get; set; }

    public SolutionEditor(SolutionFile solution)
    {
        _global = this;
        Solution = solution;

        InitializeComponent();

        // SolutionView.ShowSolution(solution);
    }
}
