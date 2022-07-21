using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScriptingDemo;

/// <summary>
/// Functions to make adding rows and columns to <see cref="Grid"/> easier.
/// </summary>
public static class WidgetHelper
{
    #region Add Row Functions

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddRowFill(this Grid grid, UIElement element)
    {
        grid.AddRow(element, new GridLength(1.0, GridUnitType.Star));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddRowFill(this Grid grid, double value, UIElement element)
    {
        grid.AddRow(element, new GridLength(value, GridUnitType.Star));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddRowAuto(this Grid grid, UIElement element)
    {
        grid.AddRow(element, GridLength.Auto);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddRowPixel(this Grid grid, double value, UIElement element)
    {
        grid.AddRow(element, new GridLength(value, GridUnitType.Pixel));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AddRow(this Grid grid, UIElement element, GridLength gridlength)
    {
        grid.RowDefinitions.Add(new RowDefinition() { Height = gridlength });
        Grid.SetRow(element, grid.RowDefinitions.Count - 1);
        grid.Children.Add(element);
    }

    #endregion

    #region Add Column Functions

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddColumnFill(this Grid grid, UIElement element)
    {
        grid.AddColumn(element, new GridLength(1.0, GridUnitType.Star));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddColumnFill(this Grid grid, double value, UIElement element)
    {
        grid.AddColumn(element, new GridLength(value, GridUnitType.Star));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddColumnAuto(this Grid grid, UIElement element)
    {
        grid.AddColumn(element, GridLength.Auto);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddColumnPixel(this Grid grid, double value, UIElement element)
    {
        grid.AddColumn(element, new GridLength(value, GridUnitType.Pixel));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AddColumn(this Grid grid, UIElement element, GridLength gridlength)
    {
        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = gridlength });
        Grid.SetColumn(element, grid.ColumnDefinitions.Count - 1);
        grid.Children.Add(element);
    }

    #endregion

    public static T? FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        //get parent item
        DependencyObject parentObject = VisualTreeHelper.GetParent(child);

        //we've reached the end of the tree
        if (parentObject == null) return null;

        //check if the parent matches the type we're looking for
        T? parent = parentObject as T;
        if (parent != null)
            return parent;
        else
            return FindParent<T>(parentObject);
    }

    public static Point GetLeftPositionRelativeTo(this UIElement child, UIElement parent)
    {
        return child.TransformToAncestor(parent).Transform(new Point(0, child.RenderSize.Height / 2));
    }

    public static Point GetRightPositionRelativeTo(this UIElement child, UIElement parent)
    {
        return child.TransformToAncestor(parent).Transform(new Point(child.RenderSize.Width, child.RenderSize.Height / 2));
    }

    public static Point GetCenterPositionRelativeTo(this UIElement child, UIElement parent) 
    {
        return child.TransformToAncestor(parent).Transform(new Point(child.RenderSize.Width / 2, child.RenderSize.Height / 2));
    }

    public static void DrawConnection(this DrawingContext dc, Point start, Point end, Pen? pen = null)
    {
        dc.DrawLine(pen ?? _whitePen, start, end);
    }

    private static Pen _whitePen = new Pen(Brushes.LightGray, 5);
}