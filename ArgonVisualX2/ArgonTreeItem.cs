using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Serialization;
using ArgonVisualX2.Views;

namespace ArgonVisualX2;

#if false

public abstract class ArgonTreeItem : IComparable<ArgonTreeItem>
{
    public string HeaderName { get; set; }

    public List<ArgonTreeItem> TreeItems { get; }

    public Visibility AddFolderVisibility { get; set; }

    public ArgonTreeItem(string headerName) 
    {
        HeaderName = headerName;
        TreeItems = new List<ArgonTreeItem>();
        AddFolderVisibility = Visibility.Collapsed;
    }

    public int CompareTo(ArgonTreeItem? other)
    {
        if (other is not null)
        {
            return HeaderName.CompareTo(other.HeaderName);
        }

        return 0;
    }

    public virtual void Select() 
    {
        
    }

    public virtual void AddFolder() 
    {
    
    }

    public virtual void Rename() 
    {
        
    }
}

#endif

public class ArgonSolutionTreeItem : CustomTreeViewItem
{
    private static ImageSource _icon = new BitmapImage(new Uri("/ArgonVisualX2;component/Icons/ArgonProject.png", UriKind.Relative));
    public override ImageSource Icon => _icon;

    public ArgonSolutionTreeItem(string headerName) : base()
    {

    }
    
}

public class ArgonProjectTreeItem : CustomTreeViewItem
{
    private static ImageSource _icon = new BitmapImage(new Uri("/ArgonVisualX2;component/Icons/ArgonProject.png", UriKind.Relative));
    public override ImageSource Icon => _icon;

    public ProjectFile Project { get; }

    public ArgonProjectTreeItem(ProjectFile project) : base()
    {
        Project = project;
    }

    protected override void OnSelected(RoutedEventArgs e)
    {
        ProjectView.Global.ShowProject(Project);
    }
}

public abstract class ArgonFolderTreeItem : CustomTreeViewItem
{
    private static ImageSource _icon = new BitmapImage(new Uri("/ArgonVisualX2;component/Icons/Folder.png", UriKind.Relative));
    public override ImageSource Icon => _icon;

    public ArgonFolderTreeItem() : base()
    {

    }
}

public class ArgonProjectFolderTreeItem : ArgonFolderTreeItem
{
    public ArgonProjectFolderTreeItem(string headerName) : base()
    {

    }
}

public class ArgonSolutionFolderTreeItem : ArgonFolderTreeItem
{
    public ArgonSolutionFolderTreeItem(string headerName) : base()
    {

    }

    protected override void OnSelected(RoutedEventArgs e)
    {
        ProjectView.Global.ShowProject(null);
    }
}

public class ArgonCodeFileTreeItem : CustomTreeViewItem
{
    private static ImageSource _icon = new BitmapImage(new Uri("/ArgonVisualX2;component/Icons/CodeFile.png", UriKind.Relative));
    public override ImageSource Icon => _icon;

    public ArgonCodeFileTreeItem(string headerName) : base()
    {

    }
}