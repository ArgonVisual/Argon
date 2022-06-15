using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ArgonUserInterfacePrototype;

public static class ArgonIcons
{
    private static string _iconsPath = @$"C:\Users\{Environment.UserName}\Desktop\Argon\ArgonUserInterfacePrototype\Icons\";

    public static ImageSource SolutionIcon { get; } = new BitmapImage(new Uri(_iconsPath + "SolutionIcon.png", UriKind.Absolute));
    public static ImageSource FolderIcon { get; } = new BitmapImage(new Uri(_iconsPath + "FolderIcon.png", UriKind.Absolute));
    public static ImageSource ProjectIcon { get; } = new BitmapImage(new Uri(_iconsPath + "ProjectIcon.png", UriKind.Absolute));
    public static ImageSource FileIcon { get; } = new BitmapImage(new Uri(_iconsPath + "FileIcon.png", UriKind.Absolute));
    public static ImageSource ClassIcon { get; } = new BitmapImage(new Uri(_iconsPath + "ClassIcon.png", UriKind.Absolute));
    public static ImageSource EnumIcon { get; } = new BitmapImage(new Uri(_iconsPath + "EnumIcon.png", UriKind.Absolute));
    public static ImageSource StructIcon { get; } = new BitmapImage(new Uri(_iconsPath + "StructIcon.png", UriKind.Absolute));
    public static ImageSource FunctionIcon { get; } = new BitmapImage(new Uri(_iconsPath + "FunctionIcon.png", UriKind.Absolute));
    public static ImageSource PropertyIcon { get; } = new BitmapImage(new Uri(_iconsPath + "PropertyIcon.png", UriKind.Absolute));
}
