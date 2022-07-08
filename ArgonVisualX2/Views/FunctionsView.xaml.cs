using System;
using System.Windows.Controls;
using ArgonVisualX2.Windows;

namespace ArgonVisualX2.Views;
/// <summary>
/// Interaction logic for FunctionsView.xaml
/// </summary>
public partial class FunctionsView : UserControl
{
    public static FunctionsView Global => SolutionEditor.Global?.FunctionsView ?? throw new NullReferenceException("FunctionsView has not been instanced.");

    public FunctionsView()
    {
        InitializeComponent();
    }
}
