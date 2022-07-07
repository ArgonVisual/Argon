using System;
using System.Collections.Generic;
using System.ComponentModel;
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
/// Interaction logic for SolutionView.xaml
/// </summary>
public partial class SolutionView : UserControl
{
    public string SolutionName { get; set; }

    public List<ArgonTreeItem> TreeItems { get; }

    public SolutionView()
    {
        InitializeComponent();
        this.DataContext = this;

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