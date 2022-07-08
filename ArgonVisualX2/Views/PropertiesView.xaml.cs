using System;
using System.Windows.Controls;
using ArgonVisualX2.Windows;

namespace ArgonVisualX2.Views;
/// <summary>
/// Interaction logic for PropertiesView.xaml
/// </summary>
public partial class PropertiesView : UserControl
{
    public static PropertiesView Global => SolutionEditor.Global?.PropertiesView ?? throw new NullReferenceException("PropertiesView has not been instanced.");

    public PropertiesView()
    {
        InitializeComponent();
    }
}
