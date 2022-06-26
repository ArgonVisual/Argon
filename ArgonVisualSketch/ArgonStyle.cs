using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ArgonVisualSketch;

public static class ArgonStyle
{
    public const bool IsDarkMode = true;

    public static Brush ViewBodyBackground = BrushHelper.MakeSolidBrush(25, 25, 28);
    public static Brush ViewTitleBackground = BrushHelper.MakeSolidBrush(45, 45, 48);
    public static Brush ViewBorder = BrushHelper.MakeSolidBrush(38, 38, 45);

    public static class Icons 
    {
        private static string _iconsPath = @$"C:\Users\{Environment.UserName}\Desktop\Argon\ArgonVisualSketch\Icons\";
        public static ImageSource Solution { get; } = new BitmapImage(new Uri(_iconsPath + "Solution.png", UriKind.Absolute));
        public static ImageSource Folder { get; } = new BitmapImage(new Uri(_iconsPath + "Folder.png", UriKind.Absolute));
        public static ImageSource CodeFile { get; } = new BitmapImage(new Uri(_iconsPath + "CodeFile.png", UriKind.Absolute));
        public static ImageSource ArgonProject { get; } = new BitmapImage(new Uri(_iconsPath + "ArgonProject.png", UriKind.Absolute));
    }
}