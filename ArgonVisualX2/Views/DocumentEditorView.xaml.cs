using System;
using System.Windows.Controls;
using ArgonVisualX2.Windows;

namespace ArgonVisualX2.Views;
/// <summary>
/// Interaction logic for DocumentEditorView.xaml
/// </summary>
public partial class DocumentEditorView : UserControl
{
    public static DocumentEditorView Global => SolutionEditor.Global?.DocumentEditorView ?? throw new NullReferenceException("DocumentEditorView has not been instanced.");

    public DocumentEditorView()
    {
        InitializeComponent();
    }
}
