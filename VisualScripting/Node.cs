using System.Windows;
using System.Windows.Controls;

namespace RigidScripting;

/// <summary>
/// Base class for all nodes
/// </summary>
public abstract class Node : ContentControl
{
    public Node() 
    {
        HorizontalAlignment = HorizontalAlignment.Center;
    }
}