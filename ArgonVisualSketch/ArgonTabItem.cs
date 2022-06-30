﻿using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using ArgonVisual.Helpers;
using ArgonVisual.Widgets;

namespace ArgonVisual;

/// <summary>
/// Represents an item in a <see cref="ArgonCodeFile"/>
/// </summary>
public class DocumentTabItem : TabItem
{
    private static Brush _textBrush = Brushes.Black;

    public DocumentTabItem(string title) 
    {
        Header = new TextBlock()
        {
            Text = title,
            FontSize = 16,
            Foreground = _textBrush
        };

        Grid grid = new Grid()
        {
            Background = BrushHelper.MakeSolidBrush(30, 30, 34)
        };
        StackPanel stackPanel = new StackPanel() 
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(5, 3, 5, 0)
        };

        stackPanel.Children.Add(new DocumentItemWidget("MyClass") { IsSelected = true });
        stackPanel.Children.Add(new DocumentItemWidget("MyOtherClass"));
        stackPanel.Children.Add(new DocumentItemWidget("MyStruct", true));

        grid.AddRowAuto(stackPanel);
        grid.AddRowFill(new GraphPanel() { Margin = new Thickness(5) });

        Content = grid;
    }
}