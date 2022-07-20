﻿using System.Collections.ObjectModel;

namespace ScriptingDemo;

public class NodeCollection : Collection<Node>
{
    public Node Owner { get; }

    public NodeCollection(Node owner)
    {
        Owner = owner;
    }

    protected override void InsertItem(int index, Node item)
    {
        item.ParentNode = Owner;
        item.ParentCollection = this;
        base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        Node item = this[index];
        item.ParentNode = null;
        item.ParentCollection = null;

        base.RemoveItem(index);
    }

    protected override void ClearItems()
    {
        foreach (Node item in this)
        {
            item.ParentNode = null;
            item.ParentCollection = null;
        }

        base.ClearItems();
    }
}