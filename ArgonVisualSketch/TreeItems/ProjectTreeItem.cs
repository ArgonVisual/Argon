﻿using System.Windows.Media;

namespace ArgonVisualSketch.TreeItems;

public class ProjectTreeItem : ArgonTreeItem
{
    public ProjectTreeItem(string title) : base(title)
    {
        
    }

    protected override ImageSource GetIcon() => ArgonStyle.Icons.ArgonProject;
}