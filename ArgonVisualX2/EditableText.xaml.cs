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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArgonVisualX2;
/// <summary>
/// Interaction logic for EditableText.xaml
/// </summary>
public partial class EditableText : UserControl
{
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(EditableText));
    
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private bool _isEditing;

    public bool IsEditing 
    {
        get => _isEditing;
        set 
        {
            _isEditing = value;
            EditableTextBlock.Visibility = value ? Visibility.Hidden : Visibility.Visible;
            EditableTextBox.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        } 
    }

    public EditableText()
    {
        InitializeComponent();
    }
}
