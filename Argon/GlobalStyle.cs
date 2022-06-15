using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Argon;

/// <summary>
/// Contains colors and fonts that the whole application uses.
/// </summary>
public static class GlobalStyle 
{
    public const double FontSizeLarge = 28;
    public const double FontSizeNormal = 18;
    public const double FontSizeSmall = 15;

    private static string _iconsPath = @$"C:\Users\{Environment.UserName}\Desktop\Argon\Argon\Icons";

    public static ImageSource SolutionIcon = new BitmapImage(new Uri(Path.Combine(_iconsPath, "SolutionIcon.png"), UriKind.RelativeOrAbsolute));
    public static ImageSource FolderIcon = new BitmapImage(new Uri(Path.Combine(_iconsPath, "FolderIcon.png"), UriKind.RelativeOrAbsolute));
    public static ImageSource ProjectIcon = new BitmapImage(new Uri(Path.Combine(_iconsPath, "ProjectIcon.png"), UriKind.RelativeOrAbsolute));
    public static ImageSource CodeFileIcon = new BitmapImage(new Uri(Path.Combine(_iconsPath, "CodeFileIcon.png"), UriKind.RelativeOrAbsolute));

    public static Brush Text { get; } = NewSolidBrush(240, 240, 240);

    public static Brush Error { get; } = NewSolidBrush(110, 30, 30);

    public static Brush BackgroundDark { get; } = NewSolidBrush(20, 20, 20);
    public static Brush Background { get; } = NewSolidBrush(28, 28, 28);
    public static Brush Border { get; } = NewSolidBrush(60, 60, 60);

    public static Brush ButtonNormal { get; } = NewSolidBrush(50, 50, 50);
    public static Brush ButtonHover { get; } = NewSolidBrush(80, 80, 80);

    public static Brush Transparent { get; } = Brushes.Transparent;

    /// <summary>
    /// Creates a <see cref="SolidColorBrush"/> from the red, green and blue values.
    /// </summary>
    /// <param name="red">The amount of red.</param>
    /// <param name="green">The amount of green.</param>
    /// <param name="blue">The amount of blue.</param>
    /// <returns>The new <see cref="SolidColorBrush"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static SolidColorBrush NewSolidBrush(byte red, byte green, byte blue) 
    {
        return new SolidColorBrush(Color.FromRgb(red, green, blue));
    }
}