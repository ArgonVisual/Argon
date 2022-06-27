using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace ArgonVisual;

public static class BrushHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SolidColorBrush MakeSolidBrush(byte red, byte green, byte blue)
    {
        return new SolidColorBrush(Color.FromRgb(red, green, blue));
    }
}
