using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ArgonVisual;

/// <summary>
/// Contains brushes, icons and fonts that are used throughout Argon.
/// </summary>
public static class ArgonStyle
{
    /// <summary>
    /// Fonts used in Argon
    /// </summary>
    public class Fonts 
    {
        public static FontFamily Normal = new FontFamily("Segoe UI");
        public static FontFamily Bold = new FontFamily("Segoe UI Semibold");
    }

    public static Brush Background = BrushHelper.MakeSolidBrush(25, 25, 28);
    public static Brush ViewTitleBackground = BrushHelper.MakeSolidBrush(45, 45, 48);
    public static Brush ViewBorder = BrushHelper.MakeSolidBrush(38, 38, 45);

    /// <summary>
    /// Icons used to represent types of files
    /// </summary>
    public static class Icons 
    {
        private static string _iconsPath = @$"C:\Users\{Environment.UserName}\Desktop\Argon\ArgonVisualSketch\Icons\";
        /// <summary>The icon for a <see cref="ArgonSolution"/>.</summary>
        public static ImageSource Solution { get; } = new BitmapImage(new Uri(_iconsPath + "Solution.png", UriKind.Absolute));
        /// <summary>The icon for a solution or project folder.</summary>
        public static ImageSource Folder { get; } = new BitmapImage(new Uri(_iconsPath + "Folder.png", UriKind.Absolute));
        /// <summary>The icon for a <see cref="ArgonCodeFile"/></summary>
        public static ImageSource CodeFile { get; } = new BitmapImage(new Uri(_iconsPath + "CodeFile.png", UriKind.Absolute));
        /// <summary>The icon for a <see cref="ArgonProject"/></summary>
        public static ImageSource Project { get; } = new BitmapImage(new Uri(_iconsPath + "ArgonProject.png", UriKind.Absolute));
    }

    /// <summary>
    /// Initializes <paramref name="resources"/> with the style that Argon uses.
    /// </summary>
    /// <param name="resources">The <see cref="ResourceDictionary"/> to initialize.</param>
    public static void Initialize(ResourceDictionary resources) 
    {
        Style windowBlockStyle = new Style(typeof(Window));
        windowBlockStyle.Setters.Add(new Setter(Window.SnapsToDevicePixelsProperty, true));
        windowBlockStyle.Setters.Add(new Setter(Window.BackgroundProperty, ArgonStyle.Background));
        resources.Add(typeof(Window), windowBlockStyle);

        Style textBlockStyle = new Style(typeof(TextBlock));
        textBlockStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.White));
        textBlockStyle.Setters.Add(new Setter(TextBlock.FontFamilyProperty, ArgonStyle.Fonts.Normal));
        resources.Add(typeof(TextBlock), textBlockStyle);

        Style treeViewStyle = new Style(typeof(TreeView));
        treeViewStyle.Setters.Add(new Setter(TreeView.BackgroundProperty, Brushes.Transparent));
        treeViewStyle.Setters.Add(new Setter(TreeView.BorderBrushProperty, null));
        resources.Add(typeof(TreeView), treeViewStyle);

        SolidColorBrush selectionBrush = BrushHelper.MakeSolidBrush(34, 77, 138);
        resources.Add(SystemColors.InactiveSelectionHighlightBrushKey, selectionBrush);
        resources.Add(SystemColors.HighlightBrushKey, selectionBrush);
    }
}