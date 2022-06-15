using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ArgonUserInterfacePrototype;

public abstract class ArgonItem : TreeViewItem
{
    public ArgonItem(string name)
    {
        IsExpanded = true;

        StackPanel panel = new StackPanel()
        {
            Orientation = Orientation.Horizontal
        };
        panel.Children.Add(new Image()
        {
            Source = Icon,
            Height = 20
        });
        panel.Children.Add(new TextBlock()
        {
            Text = name,
            VerticalAlignment = VerticalAlignment.Center
        });

        Header = panel;
    }

    protected abstract ImageSource Icon { get; }
}

public class SolutionItem : ArgonItem
{
    public SolutionItem(string name) : base(name)
    {

    }

    protected override ImageSource Icon => ArgonTreeItemsIcons.SolutionIcon;
}

public class ProjectItem : ArgonItem
{
    public ProjectItem(string name) : base(name)
    {

    }

    protected override ImageSource Icon => ArgonTreeItemsIcons.ProjectIcon;
}

public class CodeFileItem : ArgonItem
{
    public CodeFileItem(string name) : base(name)
    {

    }

    protected override ImageSource Icon => ArgonTreeItemsIcons.FileIcon;
}

public abstract class FolderItem : ArgonItem
{
    public FolderItem(string name) : base(name)
    {

    }

    protected sealed override ImageSource Icon => ArgonTreeItemsIcons.FolderIcon;
}

public class SolutionFolderItem : FolderItem
{
    public SolutionFolderItem(string name) : base(name)
    {

    }
}

public class ProjectFolderItem : FolderItem
{
    public ProjectFolderItem(string name) : base(name)
    {

    }
}

public abstract class DataTypeItem : ArgonItem
{
    private static Brush _background = new SolidColorBrush(Color.FromRgb(230, 230, 230));

    public DataTypeItem(string name) : base(name)
    {
        // Background = _background;
    }
}

public class ClassItem : DataTypeItem
{
    public ClassItem(string name) : base(name)
    {

    }

    protected override ImageSource Icon => ArgonTreeItemsIcons.ClassIcon;
}

public class StructItem : DataTypeItem
{
    public StructItem(string name) : base(name)
    {

    }

    protected override ImageSource Icon => ArgonTreeItemsIcons.StructIcon;
}

public class EnumItem : DataTypeItem
{
    public EnumItem(string name) : base(name)
    {

    }

    protected override ImageSource Icon => ArgonTreeItemsIcons.EnumIcon;
}

public class ArgonTreeItemsIcons
{
    private static string _iconsPath = @$"C:\Users\{Environment.UserName}\Desktop\Argon\ArgonUserInterfacePrototype\Icons";
    public static ImageSource SolutionIcon { get; } = new BitmapImage(new Uri(Path.Combine(_iconsPath, "SolutionIcon.png"), UriKind.RelativeOrAbsolute));
    public static ImageSource FolderIcon { get; } = new BitmapImage(new Uri(Path.Combine(_iconsPath, "FolderIcon.png"), UriKind.RelativeOrAbsolute));
    public static ImageSource ProjectIcon { get; } = new BitmapImage(new Uri(Path.Combine(_iconsPath, "ProjectIcon.png"), UriKind.RelativeOrAbsolute));
    public static ImageSource FileIcon { get; } = new BitmapImage(new Uri(Path.Combine(_iconsPath, "FileIcon.png"), UriKind.RelativeOrAbsolute));
    public static ImageSource ClassIcon { get; } = new BitmapImage(new Uri(Path.Combine(_iconsPath, "ClassIcon.png"), UriKind.RelativeOrAbsolute));
    public static ImageSource EnumIcon { get; } = new BitmapImage(new Uri(Path.Combine(_iconsPath, "EnumIcon.png"), UriKind.RelativeOrAbsolute));
    public static ImageSource StructIcon { get; } = new BitmapImage(new Uri(Path.Combine(_iconsPath, "StructIcon.png"), UriKind.RelativeOrAbsolute));
}