using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArgonVisualX2.Views;
/// <summary>
/// Interaction logic for ProjectView.xaml
/// </summary>
public partial class ProjectView : UserControl
{
    public string ProjectName { get; set; }

    public List<ArgonTreeItem> TreeItems { get; }

    public ProjectView()
    {
        InitializeComponent();

        TreeItems = new List<ArgonTreeItem>();

        TreeItems.Add(new ArgonProjectTreeItem("Minecraft"));

        ArgonProjectTreeItem fortniteTreeItem = new ArgonProjectTreeItem("Fortnite");

        fortniteTreeItem.TreeItems.Add(new ArgonCodeFileTreeItem("Battle Royale"));
        fortniteTreeItem.TreeItems.Add(new ArgonCodeFileTreeItem("Creative"));
        fortniteTreeItem.TreeItems.Add(new ArgonCodeFileTreeItem("Save the World"));

        TreeItems.Add(fortniteTreeItem);
        TreeItems.Add(new ArgonProjectTreeItem("Valorant"));
        TreeItems.Add(new ArgonProjectTreeItem("Argon"));
        TreeItems.Add(new ArgonFolderTreeItem("Roblox"));

        TreeItems.Sort();
    }
}
