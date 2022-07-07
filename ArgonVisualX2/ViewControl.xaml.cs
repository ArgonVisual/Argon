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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArgonVisualX2;
/// <summary>
/// Interaction logic for ViewControl.xaml
/// </summary>
[ContentProperty("ViewContent")]
public partial class ViewControl : UserControl
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ViewControl));

    public static readonly DependencyProperty ViewContentProperty = DependencyProperty.Register("ViewContent", typeof(object), typeof(ViewControl));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public object ViewContent
    {
        get => (string)GetValue(ViewContentProperty);
        set => SetValue(ViewContentProperty, value);
    }

    public ViewControl()
    {
        this.DataContext = this;
        InitializeComponent();
    }
}
