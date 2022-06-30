using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace ArgonVisual;

/// <summary>
/// Contains functions to ease the creation of brushes
/// </summary>
public static class BrushHelper
{
    /// <summary>
    /// Creates a solid colored brush from the red, green and blue values.
    /// </summary>
    /// <param name="red">The amount of red.</param>
    /// <param name="green">The amount of green.</param>
    /// <param name="blue">The amount of blue.</param>
    /// <returns>The new <see cref="SolidColorBrush"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SolidColorBrush MakeSolidBrush(byte red, byte green, byte blue)
    {
        return new SolidColorBrush(Color.FromRgb(red, green, blue));
    }
}
