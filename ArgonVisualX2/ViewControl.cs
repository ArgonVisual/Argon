using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

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
        get => (object)GetValue(ViewContentProperty);
        set => SetValue(ViewContentProperty, value);
    }

    public ViewControl()
    {
        this.DataContext = this;
    }
}
