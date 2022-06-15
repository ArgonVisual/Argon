using System.Windows.Controls;

namespace ArgonUserInterfacePrototype;

public class SolutionEditor : ContentControl
{
    public SolutionEditor() 
    {
        Grid mainGrid = new Grid();

        mainGrid.AddColumnPixel(250, new SolutionExplorer());
        mainGrid.AddColumnFill(new GraphPanel());

        Content = mainGrid;
    }
}
