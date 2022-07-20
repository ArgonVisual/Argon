using System.Windows;
using System.Windows.Media;

namespace RigidScripting;

public static class VisualStyle 
{
    public const double ThicknessAmount = 4;
    public const double ThicknessSmallAmount = 2;
    public static Thickness Thickness => new Thickness(ThicknessSmallAmount, ThicknessAmount, ThicknessSmallAmount, ThicknessAmount);
    public static Thickness ThicknessSmall => new Thickness(ThicknessSmallAmount);

    public const double CornerRadiusAmount = 8;
    public static CornerRadius CornerRadius => new CornerRadius(CornerRadiusAmount);

    public static Brush NodeBackground { get; } = BrushHelper.MakeSolidBrush(60, 60, 60);
    public static Brush NodeHoveredBackground { get; } = BrushHelper.MakeSolidBrush(80, 80, 80);

    public static Brush GraphBackground { get; } = BrushHelper.MakeSolidBrush(35, 35, 35);

    public static Brush FalseBrush { get; } = BrushHelper.MakeSolidBrush(211, 57, 57);
    public static Brush TrueBrush { get; } = BrushHelper.MakeSolidBrush(57, 178, 57);
}