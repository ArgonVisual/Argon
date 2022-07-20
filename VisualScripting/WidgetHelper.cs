using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace RigidScripting;

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
}