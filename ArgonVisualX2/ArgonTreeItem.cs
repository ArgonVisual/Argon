using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Serialization;

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

    public ArgonProjectTreeItem(string headerName) : base(headerName)
    {

    }
}

public class ArgonFolderTreeItem : ArgonTreeItem
{
    private static ImageSource _icon = new BitmapImage(new Uri("/ArgonVisualX2;component/Icons/Folder.png", UriKind.Relative));
    public override ImageSource Icon => _icon;

    public ArgonFolderTreeItem(string headerName) : base(headerName)
    {

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