using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Serialization;
using ArgonVisualX2.Views;

namespace ArgonVisualX2;
public abstract class ArgonTreeItem : IComparable<ArgonTreeItem>
{
    public string HeaderName { get; set; }

    public abstract ImageSource Icon { get; }

    public List<ArgonTreeItem> TreeItems { get; }

    public ArgonTreeItem(string headerName) 
    {
        HeaderName = headerName;
        TreeItems = new List<ArgonTreeItem>();
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
}



public class ArgonSolutionTreeItem : ArgonTreeItem
{
    private static ImageSource _icon = new BitmapImage(new Uri("/ArgonVisualX2;component/Icons/ArgonProject.png", UriKind.Relative));
    public override ImageSource Icon => _icon;

    public ArgonSolutionTreeItem(string headerName) : base(headerName)
    {
        HeaderName = headerName;
    }
    
}

public class ArgonProjectTreeItem : ArgonTreeItem
{
    private static ImageSource _icon = new BitmapImage(new Uri("/ArgonVisualX2;component/Icons/ArgonProject.png", UriKind.Relative));
    public override ImageSource Icon => _icon;

    public ProjectFile Project { get; }

    public ArgonProjectTreeItem(ProjectFile project) : base(project.Name)
    {
        Project = project;
    }

    public override void Select()
    {
        ProjectView.Global.ShowProject(Project);
    }
}

public abstract class ArgonFolderTreeItem : ArgonTreeItem
{
    private static ImageSource _icon = new BitmapImage(new Uri("/ArgonVisualX2;component/Icons/Folder.png", UriKind.Relative));
    public override ImageSource Icon => _icon;

    public ArgonFolderTreeItem(string headerName) : base(headerName)
    {

    }
}

public class ArgonProjectFolderTreeItem : ArgonFolderTreeItem
{
    public ArgonProjectFolderTreeItem(string headerName) : base(headerName)
    {

    }
}

public class ArgonSolutionFolderTreeItem : ArgonFolderTreeItem
{
    public ArgonSolutionFolderTreeItem(string headerName) : base(headerName)
    {

    }

    public override void Select()
    {
        ProjectView.Global.ShowProject(null);
    }
}

public class ArgonCodeFileTreeItem : ArgonTreeItem
{
    private static ImageSource _icon = new BitmapImage(new Uri("/ArgonVisualX2;component/Icons/CodeFile.png", UriKind.Relative));
    public override ImageSource Icon => _icon;

    public ArgonCodeFileTreeItem(string headerName) : base(headerName)
    {

    }
}